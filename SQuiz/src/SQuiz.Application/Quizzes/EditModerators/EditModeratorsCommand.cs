using LanguageExt.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Exceptions;
using SQuiz.Shared.Models;

namespace SQuiz.Application.Quizzes.EditModerators
{
    public class EditModeratorsCommand : IRequest<Result<Unit>>
    {
        public EditModeratorsCommand(EditQuizDto model)
        {
            Model = model;
        }

        public EditQuizDto Model { get; set; }
    }

    public class EditModeratorsCommandHandler : IRequestHandler<EditModeratorsCommand, Result<Unit>>
    {
        private readonly ISQuizContext _quizContext;

        public EditModeratorsCommandHandler(ISQuizContext quizContext)
        {
            _quizContext = quizContext;
        }

        public async ValueTask<Result<Unit>> Handle(EditModeratorsCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var quizId = model.Id;
            var quiz = await _quizContext.Quizzes
                .Include(x => x.QuizModerators)
                    .ThenInclude(x => x.Moderator)
                .FirstOrDefaultAsync(x => x.Id == quizId, cancellationToken);

            if (quiz == null)
            {
                return new Result<Unit>(new NotFoundException());
            }

            RemoveModerators(quiz, model);
            AddModerators(quiz, model);
            await _quizContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private void RemoveModerators(Quiz quiz, EditQuizDto model)
        {
            var ids = model.Moderators.Select(x => x.Id)
                .ToHashSet();

            foreach (var quizModerator in quiz.QuizModerators)
            {
                if (!ids.Contains(quizModerator.ModeratorId))
                {
                    _quizContext.QuizModerators.Remove(quizModerator);
                }
            }
        }

        private void AddModerators(Quiz quiz, EditQuizDto model)
        {
            var quizIds = quiz.QuizModerators
                .Select(x => x.ModeratorId)
                .ToHashSet();

            foreach (var moderator in model.Moderators)
            {
                if (!quizIds.Contains(moderator.Id))
                {
                    _quizContext.QuizModerators.Add(new QuizModerator()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ModeratorId = moderator.Id,
                        QuizId = quiz.Id
                    });
                }
            }
        }
    }
}
