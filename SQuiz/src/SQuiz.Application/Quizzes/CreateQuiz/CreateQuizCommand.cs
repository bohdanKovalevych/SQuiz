using AutoMapper;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Models;

namespace SQuiz.Application.Quizzes.CreateQuiz
{
    public class CreateQuizCommand : IRequest
    {
        public EditQuizDto Model { get; set; }

        public string UserId { get; set; }
    }

    public class CreateQuizHandler : IRequestHandler<CreateQuizCommand>
    {
        private readonly IMapper _mapper;
        private readonly IQuizService _quizService;
        private readonly ISQuizContext _quizContext;

        public CreateQuizHandler(IMapper mapper, IQuizService quizService, ISQuizContext quizContext)
        {
            _mapper = mapper;
            _quizService = quizService;
            _quizContext = quizContext;
        }

        public async ValueTask<Unit> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            string userid = request.UserId;
            var quiz = _mapper.Map<Quiz>(request.Model);

            quiz.Id = Guid.NewGuid().ToString();
            quiz.AuthorId = userid;

            _quizService.AssignIdsToQuestionsAndAnswers(quiz,
                x => _quizContext.SetState(x, EntityState.Modified),
                x => _quizContext.SetState(x, EntityState.Added));

            _quizContext.Quizzes.Add(quiz);

            await _quizContext.SaveChangesAsync();

            _quizService.SetCorrectAnswersFromModel(quiz, request.Model.Questions);

            await _quizContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
