using AutoMapper;
using LanguageExt.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Exceptions;

namespace SQuiz.Application.Quizzes.GetQuiz
{
    public class GetQuizCommand : IRequest<Result<QuizDetailsDto>>
    {
        public string QuizId { get; set; }
    }

    public class GetQuizCommandHandler : IRequestHandler<GetQuizCommand, Result<QuizDetailsDto>>
    {
        private readonly ISQuizContext _quizContext;
        private readonly IQuizService _quizService;
        private readonly IMapper _mapper;

        public GetQuizCommandHandler(ISQuizContext quizContext, IQuizService quizService, IMapper mapper)
        {
            _quizContext = quizContext;
            _quizService = quizService;
            _mapper = mapper;
        }

        public async ValueTask<Result<QuizDetailsDto>> Handle(GetQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _quizContext.Quizzes
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                .Include(x => x.Questions)
                    .ThenInclude(x => x.CorrectAnswer)
                .OrderByDescending(x => x.DateUpdated)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.QuizId);

            if (quiz == null)
            {
                return new Result<QuizDetailsDto>(new NotFoundException());
            }

            _quizService.ReorderQuestions(quiz);
            _quizService.ReorderAnswers(quiz);

            var quizDto = _mapper.Map<QuizDetailsDto>(quiz);

            if (quizDto == null)
            {
                return new Result<QuizDetailsDto>(new BadRequestException());
            }

            _quizService.SetCorrectAnswersFromEntity(quiz, quizDto.Questions);
            
            return quizDto;
        }
    }
}
