using LanguageExt;
using LanguageExt.Common;
using LanguageExt.SomeHelp;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Application.Quizzes.CreateQuiz;
using SQuiz.Application.Quizzes.EditModerators;
using SQuiz.Application.Quizzes.GetModerators;
using SQuiz.Application.Quizzes.GetQuiz;
using SQuiz.Application.Quizzes.UpdateQuiz;
using SQuiz.Server.Extensions;
using SQuiz.Shared;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Extensions;
using System.Security.Claims;

namespace SQuiz.Server.Controllers
{
    [Authorize()]
    [ApiController]
    [Route("[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly ISQuizContext _quizContext;
        private readonly IQuizService _quizService;
        private readonly IMediator _mediator;

        public QuizzesController(ISQuizContext quizContext,
            IQuizService quizService,
            IMediator mediator)
        {
            _quizContext = quizContext;
            _quizService = quizService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizzes()
        {
            string authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var quizzes = await _quizContext.Quizzes.WithAuthor(authorId)
                .Select(x => new QuizOptionDto()
                {
                    Id = x.Id,
                    ShortId = x.ShortId,
                    AuthorId = x.AuthorId,
                    Description = x.Description,
                    IsPublic = x.IsPublic,
                    Name = x.Name,
                    QuestionsCount = x.Questions.Count,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                })
                .OrderByDescending(x => x.DateUpdated)
                .ToListAsync();

            return Ok(quizzes);
        }

        [AllowAnonymous]
        [HttpGet("moderators")]
        public async Task<IActionResult> GetModerators([FromQuery] string? q)
        {
            var command = new GetModeratorsCommand(q);
            var result = await _mediator.Send(command);

            return result.MatchAction();
        }

        [HttpGet("{resourceId}")]
        [Authorize(Policies.QuizAuthor)]
        public async Task<IActionResult> GetQuiz(string resourceId)
        {
            var command = new GetQuizCommand(resourceId);

            var result = await _mediator.Send(command);

            return result.MatchAction();
        }

        [HttpPut("{resourceId}")]
        [Authorize(Policies.QuizAuthor)]
        public async Task<IActionResult> UpdateQuiz(string resourceId, [FromBody] EditQuizDto model)
        {
            model.Id = resourceId;

            var updateModeratorsCommand = new EditModeratorsCommand(model);
            var updateQuizCommand = new UpdateQuizCommand(model);

            var updateResult = await _mediator.Send(updateQuizCommand)
                .Bind(async _ => await _mediator.Send(updateModeratorsCommand));

            return updateResult.MatchAction();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(EditQuizDto quizDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var createQuizCommand = new CreateQuizCommand(quizDto, userId);
            var updateModeratorsCommand = new EditModeratorsCommand(quizDto);
            
            var updateResult = await _mediator.Send(createQuizCommand)
               .Bind(async _ => await _mediator.Send(updateModeratorsCommand));

            return updateResult.MatchAction();
        }

        [HttpDelete("{resourceId}")]
        [Authorize(Policies.QuizAuthor)]
        public async Task<IActionResult> DeleteQuiz(string resourceId)
        {
            var quiz = await _quizContext.Quizzes
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == resourceId);

            if (quiz == null)
            {
                return Ok();
            }

            var empty = new EditQuizDto();
            var questions = _quizService.GetQuestionsToRemove(quiz, empty);
            var answers = _quizService.GetAnswersToRemove(quiz, empty);

            await _quizContext.SaveChangesAsync();

            _quizContext.Answers.RemoveRange(answers);
            _quizContext.Questions.RemoveRange(questions);
            _quizContext.Quizzes.Remove(quiz);

            await _quizContext.SaveChangesAsync();

            return Ok();
        }
    }
}
