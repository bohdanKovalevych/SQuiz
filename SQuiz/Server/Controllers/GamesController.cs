using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQuiz.Infrastructure.Interfaces;
using SQuiz.Shared;
using SQuiz.Shared.Dtos.Dashboards;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Interfaces;
using SQuiz.Shared.Models;

namespace SQuiz.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class GamesController : ControllerBase
    {
        private readonly ISQuizContext _context;
        private readonly IMapper _mapper;
        private readonly IPointsCounter _pointsCounter;

        public GamesController(ISQuizContext context,
            IMapper mapper,
            IPointsCounter pointsCounter)
        {
            _context = context;
            _mapper = mapper;
            _pointsCounter = pointsCounter;
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
                .Select(x => x.StartedBy.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

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
            var playerId = Request.Cookies["playerId"];

            var isCorrect = answerDto.AnswerId != null && await _context.Questions
                .AnyAsync(x => x.CorrectAnswerId == answerDto.AnswerId);
            var points = 0;

            if (isCorrect)
            {
                var question = await _context.Questions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Answers
                        .Any(x => x.Id == answerDto.AnswerId));

                points = _pointsCounter.GetPoints(answerDto.TimeToSolve, question);
            }

            var playerAnswer = new PlayerAnswer()
            {
                Id = Guid.NewGuid().ToString(),
                AnswerId = answerDto.AnswerId,
                PlayerId = playerId,
                Points = points
            };

            _context.PlayerAnswers.Add(playerAnswer);
            await _context.SaveChangesAsync();

            return Ok();
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
            var playerId = Request.Cookies["playerId"];
            var lastQuestion = Request.Cookies["questionIndex"];

            lastQuestion ??= "0";

            var index = int.Parse(lastQuestion);

            var question = await _context.Questions
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Quiz
                    .QuizGames
                    .Any(x => x.Players
                    .Any(x => x.Id == playerId))
                        && x.Order == index);

            if (question == null)
            {
                await ComputeAndSavePoints(playerId);
                Response.Cookies.Delete("questionIndex");
                return NoContent();
            }

            Response.Cookies.Append("questionIndex", $"{index + 1}");

            question.Answers = question.Answers
                .OrderBy(x => x.Order)
                .ToList();

            var questionDto = _mapper.Map<GameQuestionDto>(question);

            return Ok(questionDto);
        }

        private async Task ComputeAndSavePoints(string playerId)
        {
            var player = await _context.Players
                .Include(x => x.PlayerAnswers)
                .FirstOrDefaultAsync(x => x.Id == playerId);

            if (player == null)
            {
                return;
            }

            player.Points = player.PlayerAnswers.Sum(x => x.Points);

            await _context.SaveChangesAsync();
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

            Response.Cookies.Append("playerId", playerId);

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
            Response.Cookies.Append("playerId", player.Id);

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
