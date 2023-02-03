using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Games.ComputeAndSavePoints;
using SQuiz.Application.Games.GetQuestion;
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
            var baseQuery = _context.QuizGames
                .AsNoTracking()
                .Include(x => x.StartedBy)
                .Where(x => x.Quiz.IsPublic)
                .Take(5);

            var popularGames = await baseQuery
                .OrderByDescending(x => x.Players.Count)
                .ProjectTo<GameOptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var newGames = await baseQuery
                .OrderByDescending(x => x.DateCreated)
                .ProjectTo<GameOptionDto>(_mapper.ConfigurationProvider)
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
            var result = await _context.QuizGames
                .Select(x => new GameOptionDto()
                {
                    QuizId = x.QuizId,
                    EndDate = x.DateEnd,
                    Id = x.Id,
                    QuestionCount = x.Quiz.Questions.Count,
                    ShortId = x.ShortId,
                    Name = x.Quiz.Name,
                    StartDate = x.DateStart
                })
                .FirstOrDefaultAsync(x => x.ShortId == shortId);

            return Ok(result);
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

        [HttpGet("scores/{playerId}")]
        public async Task<IActionResult> GetScores(string playerId)
        {
            var result = await _context.Players.AsNoTracking()
                .Where(x => x.QuizGame.Players.Any(x => x.Id == playerId))
                .OrderByDescending(x => x.Points)
                .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("questions")]
        [Authorize(Policies.PlayerInGame)]
        public async Task<IActionResult> GetQuestion()
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
            var getQuestionCommand = new GetQuestionCommand(playerId, index)
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
            var game = await _context.QuizGames.AsNoTracking().FirstOrDefaultAsync(x => x.Players.Any(x => x.Id == playerId));
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
            var gameId = gameDto.ShortId;
            var name = gameDto.Name;
            var game = await _context.QuizGames.AsNoTracking().FirstOrDefaultAsync(x => x.ShortId == gameId);

            if (game == null)
            {
                return NotFound();
            }

            if (ValidateGame(game) is string validationMessage)
            {
                return BadRequest(validationMessage);
            }

            if (await _context.Players.AnyAsync(x => x.Name == name))
            {
                return BadRequest($"This name '{name}' is already in use");
            }

            var player = new Player()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                QuizGameId = game.Id
            };

            _context.Players.Add(player);

            await _context.SaveChangesAsync();
            Response.Cookies.Append(Constants.CookiesKey.PlayerId, player.Id);

            return Ok();
        }

        private string? ValidateGame(QuizGame game)
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
