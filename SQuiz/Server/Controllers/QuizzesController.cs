using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Extensions;
using SQuiz.Shared.Models;
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
        private readonly IMapper _mapper;

        public QuizzesController(ISQuizContext quizContext, IMapper mapper,
            IQuizService quizService)
        {
            _quizContext = quizContext;
            _quizService = quizService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizzes()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

        [HttpGet("{resourceId}")]
        [Authorize(Policies.QuizAuthor)]
        public async Task<IActionResult> GetQuiz(string resourceId)
        {
            var quiz = await _quizContext.Quizzes
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                .Include(x => x.Questions)
                    .ThenInclude(x => x.CorrectAnswer)
                .OrderByDescending(x => x.DateUpdated)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == resourceId);

            if (quiz == null)
            {
                return NotFound();
            }

            _quizService.ReorderQuestions(quiz);
            _quizService.ReorderAnswers(quiz);

            var quizDto = _mapper.Map<QuizDetailsDto>(quiz);

            if (quizDto == null)
            {
                return BadRequest();
            }

            _quizService.SetCorrectAnswersFromEntity(quiz, quizDto.Questions);

            return Ok(quizDto);
        }

        [HttpPut("{resourceId}")]
        [Authorize(Policies.QuizAuthor)]
        public async Task<IActionResult> UpdateQuiz(string resourceId, [FromBody] EditQuizDto model)
        {
            var quiz = await _quizContext.Quizzes
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == resourceId);

            if (quiz == null)
            {
                return NotFound();
            }

            var questionsToRemove = _quizService.GetQuestionsToRemove(quiz, model);
            var answersToRemove = _quizService.GetAnswersToRemove(quiz, model);

            _quizContext.Questions.UpdateRange(questionsToRemove);
            await _quizContext.SaveChangesAsync();

            _quizContext.Answers.RemoveRange(answersToRemove);
            _quizContext.Questions.RemoveRange(questionsToRemove);
            await _quizContext.SaveChangesAsync();

            foreach (var q in quiz.Questions)
            {
                _quizContext.Entry(q).State = EntityState.Detached;
                foreach (var a in q.Answers)
                {
                    _quizContext.Entry(a).State = EntityState.Detached;
                }
            }

            _mapper.Map(model, quiz);
            quiz.DateUpdated = DateTime.Now;

            _quizService.AssignOrderToQuestions(quiz);
            _quizService.AssignOrderToAnswers(quiz);

            _quizService.AssignIdsToQuestionsAndAnswers(quiz,
                x => _quizContext.Entry(x).State = EntityState.Modified,
                x => _quizContext.Entry(x).State = EntityState.Added);

            _quizContext.Quizzes.Update(quiz);

            await _quizContext.SaveChangesAsync();

            _quizService.SetCorrectAnswersFromModel(quiz, model.Questions);

            await _quizContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(EditQuizDto quizDto)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var quiz = _mapper.Map<Quiz>(quizDto);

            quiz.Id = Guid.NewGuid().ToString();
            quiz.AuthorId = userid;

            _quizService.AssignIdsToQuestionsAndAnswers(quiz,
                x => _quizContext.Entry(x).State = EntityState.Modified,
                x => _quizContext.Entry(x).State = EntityState.Added);

            _quizService.AssignOrderToQuestions(quiz);
            _quizService.AssignOrderToAnswers(quiz);
            _quizContext.Quizzes.Add(quiz);

            await _quizContext.SaveChangesAsync();

            _quizService.SetCorrectAnswersFromModel(quiz, quizDto.Questions);

            await _quizContext.SaveChangesAsync();

            return Ok();
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
