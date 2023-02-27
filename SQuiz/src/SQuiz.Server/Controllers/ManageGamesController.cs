using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Application.ManageGames.EditQuizGame;
using SQuiz.Server.Extensions;
using SQuiz.Shared;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Models;
using System.Security.Claims;

namespace SQuiz.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ManageGamesController : ControllerBase
    {
        private readonly ISQuizContext _context;
        private readonly IMediator _mediator;

        public ManageGamesController(ISQuizContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetGameOptions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _context.RegularQuizGames
                .Where(x => x.StartedById == userId)
                .Select(x => new RegularGameOptionDto()
                {
                    QuizId = x.QuizId,
                    EndDate = x.DateEnd,
                    Id = x.Id,
                    QuestionCount = x.Quiz.Questions.Count,
                    ShortId = x.ShortId,
                    Name = x.Name,
                    StartDate = x.DateStart
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpPut("{resourceId}")]
        [Authorize(Policies.QuizModerator)]
        public async Task<IActionResult> EditQuizGame(
            [FromRoute] string resourceId,
            [FromBody] StartRegularGameDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue("preferred_username");
            var name = User.FindFirstValue("name");
            model.QuizId = resourceId;
            
            var startRegularGameCommand = new EditRegularQuizGameCommand()
            {
                Model = model,
                UserId = userId,
                CreateModeratorIfNotExist = async () =>
                {
                    _context.Moderators.Add(new Moderator()
                    {
                        Id = userId,
                        Email = User.FindFirstValue("preferred_username"),
                        Name = User.FindFirstValue("name"),
                    });
                    await _context.SaveChangesAsync();
                }
            };

            var result = await _mediator.Send(startRegularGameCommand);

            return result.MatchAction();
        }
    }
}
