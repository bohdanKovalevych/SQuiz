using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQuiz.Infrastructure.Interfaces;
using SQuiz.Shared;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Extensions;
using SQuiz.Shared.Models;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace SQuiz.Server.Controllers
{
    [Authorize()]
    [ApiController]
    [Route("[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly ISQuizContext _quizContext;
        private readonly IMapper _mapper;

        public QuizzesController(ISQuizContext quizContext, IMapper mapper)
        {
            _quizContext = quizContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizzes()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var quizzes = await _quizContext.Quizzes.WithAuthor(authorId)
                .Select(x => new QuizOptionDto()
                {
                    AuthorId = x.AuthorId,
                    Description = x.Description,
                    IsPublic = x.IsPublic,
                    Name = x.Name,
                    QuestionsCount = x.Questions.Count,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                })
                .OrderBy(x => x.DateUpdated)
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
                .OrderByDescending(x => x.DateUpdated)
                .FirstOrDefaultAsync(x => x.Id == resourceId);
            
            if (quiz == null)
            {
                return NotFound();
            }
            
            quiz.Questions = quiz.Questions
                .OrderBy(x => x.Order)
                .ToList();

            foreach (var question in quiz.Questions)
            {
                question.Answers = question.Answers
                    .OrderBy(x => x.Order)
                    .ToList();
            }

            var quizDto = _mapper.Map<QuizDetailsDto>(quiz);
            
            return Ok(quizDto);
        }

        [HttpPut("{resourceId}")]
        [Authorize(Policies.QuizAuthor)]
        public async Task<IActionResult> UpdateQuiz(string resourceId)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(EditQuizDto quizDto)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var quiz = _mapper.Map<Quiz>(quizDto);

            quiz.Id = Guid.NewGuid().ToString();
            quiz.AuthorId = userid;
            var correctAnswers = new Dictionary<string, string>();
            
            foreach (var (question, questionIndex) in quiz.Questions.WithIndex())
            {
                question.Id = Guid.NewGuid().ToString();
                question.Order = questionIndex;
                var correctAnswerIndex = quizDto.Questions[questionIndex].CorrectAnswerIndex;
                
                foreach (var (answer, answerIndex) in question.Answers.WithIndex())
                {
                    answer.Id = Guid.NewGuid().ToString();
                    answer.Order = answerIndex;
                    
                    if (answerIndex == correctAnswerIndex)
                    {
                        correctAnswers[question.Id] = answer.Id;
                    }
                }
            }

            _quizContext.Quizzes.Add(quiz);
            await _quizContext.SaveChangesAsync();
            
            foreach(var question in quiz.Questions)
            {
                question.CorrectAnswerId = correctAnswers[question.Id];
            }

            await _quizContext.SaveChangesAsync();
            
            return Ok();
        }
    }
}
