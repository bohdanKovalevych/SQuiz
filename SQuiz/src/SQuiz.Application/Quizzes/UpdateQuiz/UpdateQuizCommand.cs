using AutoMapper;
using LanguageExt.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Exceptions;
using SQuiz.Shared.Models;

namespace SQuiz.Application.Quizzes.UpdateQuiz
{
    public class UpdateQuizCommand : IRequest<Result<Unit>>
    {
        public EditQuizDto Model { get; set; }
    }

    public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, Result<Unit>>
    {
        private readonly ISQuizContext _quizContext;
        private readonly IMapper _mapper;
        private readonly IQuizService _quizService;

        public UpdateQuizCommandHandler(ISQuizContext quizContext, IMapper mapper, IQuizService quizService)
        {
            _quizContext = quizContext;
            _mapper = mapper;
            _quizService = quizService;
        }

        public async ValueTask<Result<Unit>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _quizContext.Quizzes
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Model.Id);

            if (quiz == null)
            {
                return new Result<Unit>(new NotFoundException());
            }

            await RemoveQuestionsAndAnswers(quiz, request.Model, cancellationToken);
            await UpdateQuestionsAndAnswers(quiz, request.Model, cancellationToken);

            return Unit.Value;
        }

        private async Task RemoveQuestionsAndAnswers(Quiz quiz, EditQuizDto model, CancellationToken cancellationToken)
        {
            var questionsToRemove = _quizService.GetQuestionsToRemove(quiz, model);
            var answersToRemove = _quizService.GetAnswersToRemove(quiz, model);

            _quizContext.Questions.UpdateRange(questionsToRemove);
            await _quizContext.SaveChangesAsync(cancellationToken);

            _quizContext.Answers.RemoveRange(answersToRemove);
            _quizContext.Questions.RemoveRange(questionsToRemove);
            await _quizContext.SaveChangesAsync(cancellationToken);

            foreach (var q in quiz.Questions)
            {
                _quizContext.SetState(q, EntityState.Detached);
                foreach (var a in q.Answers)
                {
                    _quizContext.SetState(a, EntityState.Detached);
                }
            }
        }

        private async Task UpdateQuestionsAndAnswers(Quiz quiz, EditQuizDto model, CancellationToken cancellationToken)
        {
            _mapper.Map(model, quiz);
            quiz.DateUpdated = DateTime.Now;

            _quizService.AssignIdsToQuestionsAndAnswers(quiz,
                x => _quizContext.SetState(x, EntityState.Modified),
                x => _quizContext.SetState(x, EntityState.Added));

            _quizContext.Quizzes.Update(quiz);

            await _quizContext.SaveChangesAsync(cancellationToken);

            _quizService.SetCorrectAnswersFromModel(quiz, model.Questions);

            await _quizContext.SaveChangesAsync(cancellationToken);
        }
    }
}
