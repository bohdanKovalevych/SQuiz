using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Games.ComputeAndSavePoints;
using SQuiz.Application.Games.GetQuestion;
using SQuiz.Application.Games.JoinGame;
using SQuiz.Application.Games.ResetPoints;
using SQuiz.Application.Games.SendAnswer;
using SQuiz.Application.Interfaces;
using SQuiz.Server.Extensions;
using SQuiz.Shared;
using SQuiz.Shared.Dtos.Dashboards;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Models;
using Constants = SQuiz.Client.Constants;

namespace SQuiz.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class GamesController : ControllerBase
    {
        private readonly ISQuizContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GamesController(ISQuizContext context,
            IMapper mapper,
            IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicGames()
        {
            var baseQuery = _context.RegularQuizGames
                .AsNoTracking()
                .Include(x => x.StartedBy)
                .Where(x => x.Quiz.IsPublic)
                .Take(5);

            var popularGames = await baseQuery
                .OrderByDescending(x => x.Players.Count)
                .ProjectTo<RegularGameOptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var newGames = await baseQuery
                .OrderByDescending(x => x.DateCreated)
                .ProjectTo<RegularGameOptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var popularUsers = await baseQuery
                .OrderByDescending(x => x.Players.Count)
                .Select(x => x.StartedBy.Name ?? string.Empty)
                .Distinct()
                .Take(5)
                .ToListAsync() ?? new List<string>();

            var result = new NotAthorizedDashboardDto()
            {
                NewGames = newGames,
                PopularGames = popularGames,
                PopularUsers = popularUsers
            };

            return Ok(result);
        }

        [HttpGet("{shortId}")]
        public async Task<IActionResult> GetGame(int shortId)
        {
            var quizGame = await _context.QuizGames
                .Include(x => x.Quiz)
                    .ThenInclude(x => x.Questions)
                .FirstOrDefaultAsync(x => x.ShortId == shortId);
            var dto = _mapper.Map(quizGame, quizGame?.GetType(), typeof(GameOptionDto));
            Response.Headers.Append(Constants.HeadersKey.ResponseEntityType, dto?.GetType().FullName);
            return Ok(dto);
        }

        [HttpPost("answers")]
        [Authorize(Policies.PlayerInGame)]
        public async Task<IActionResult> SendAnswer(SendAnswerDto answerDto)
        {
            string? playerId = Request.Cookies[Constants.CookiesKey.PlayerId];
            var sendAnswerCommand = new SendAnswerCommand(playerId, answerDto);
            var result = await _mediator.Send(sendAnswerCommand);

            return result.MatchAction();
        }

        [HttpGet("scores/{gameShortId:int}")]
        public async Task<IActionResult> GetScores(int gameShortId)
        {
            var result = await _context.Players.AsNoTracking()
                .Where(x => x.QuizGame.ShortId == gameShortId)
                .OrderByDescending(x => x.Points)
                .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{gameShortId}/questions")]
        [Authorize(Policies.PlayerInGame)]
        public async Task<IActionResult> GetQuestion(int gameShortId)
        {
            string playerId = Request?.Cookies[Constants.CookiesKey.PlayerId] ?? throw new ArgumentException();
            string? lastQuestion = Request?.Cookies[Constants.CookiesKey.QuestionIndex];
            lastQuestion ??= "0";
            bool parsed = int.TryParse(lastQuestion, out var index);

            if (!parsed)
            {
                return BadRequest();
            }

            var resetPoints = new ResetPointsCommand(playerId);
            var savePoints = new ComputeAndSavePointsCommand(playerId);
            var getQuestionCommand = new GetQuestionCommand(gameShortId, index)
            {
                OnEndQuiz = async () =>
                {
                    Response.Cookies.Delete(Constants.CookiesKey.QuestionIndex);
                    await _mediator.Send(savePoints);
                },
                OnStartQuiz = async () => await _mediator.Send(resetPoints),
                OnIndexChanged = i => Response.Cookies.Append(Constants.CookiesKey.QuestionIndex, i.ToString())
            };

            var result = await _mediator.Send(getQuestionCommand);

            return result.MatchAction();
        }

        [HttpGet("players/{id}")]
        public async Task<IActionResult> GetPlayer(string id)
        {
            var player = await _context.Players.FirstOrDefaultAsync(x => x.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<PlayerDto>(player);

            return Ok(model);
        }

        [HttpPost("rejoin")]
        public async Task<IActionResult> RejoinGame([FromBody] string playerId)
        {
            var game = await _context.RegularQuizGames.AsNoTracking().FirstOrDefaultAsync(x => x.Players.Any(x => x.Id == playerId));
            var playerExists = await _context.Players.AnyAsync(x => x.Id == playerId);

            if (game == null || !playerExists)
            {
                return NotFound();
            }

            if (ValidateGame(game) is string validationMessage)
            {
                return BadRequest(validationMessage);
            }

            Response.Cookies.Append(Constants.CookiesKey.PlayerId, playerId);

            return Ok();
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinGame([FromBody] JoinGameDto gameDto)
        {
            var playerId = Guid.NewGuid().ToString();
            var joinGameCommand = new JoinRegularGameCommand()
            {
                GameDto = gameDto,
                GetPlayerId = () => playerId,
                ValidateGame = ValidateGame
            };
            var result = await _mediator.Send(joinGameCommand);
            result.IfSucc(x => Response.Cookies.Append(Constants.CookiesKey.PlayerId, playerId));
            
            return result.MatchAction();
        }

        private string? ValidateGame(RegularQuizGame game)
        {
            if (game.DateEnd < DateTime.Now)
            {
                return "It is too late";
            }
            if (game.DateStart > DateTime.Now)
            {
                return $"It is too early. Wait for {game.DateStart - DateTime.Now}";
            }   

            return null;
        }
    }
}
