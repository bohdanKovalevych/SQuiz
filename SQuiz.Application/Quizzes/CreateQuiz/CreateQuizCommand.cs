using AutoMapper;
using Mediator;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Quiz;

namespace SQuiz.Application.Quizzes.CreateQuiz
{
    public class CreateQuizCommand : IRequest
    {
        public EditQuizDto Model { get; set; }
    }

    public class CreateQuizHandler : IRequestHandler<CreateQuizCommand>
    {
        public CreateQuizHandler(IMapper mapper, IQuizService quizService, ISQuizContext quizContext)
        {

        }

        public ValueTask<Unit> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            return Unit.ValueTask;
        }
    }
}
