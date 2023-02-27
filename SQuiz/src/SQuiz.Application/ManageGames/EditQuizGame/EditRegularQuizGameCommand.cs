using AutoMapper;
using LanguageExt.Common;
using Mediator;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Models;

namespace SQuiz.Application.ManageGames.EditQuizGame
{
    public class EditRegularQuizGameCommand
        : EditQuizGameCommand<RegularQuizGame, StartRegularGameDto>, IRequest<Result<int>>
    {

    }

    public class EditRegularQuizGameCommandHandler
        : EditQuizGameCommandHandler<RegularQuizGame, StartRegularGameDto>, IRequestHandler<EditRegularQuizGameCommand, Result<int>>
    {
        public EditRegularQuizGameCommandHandler(ISQuizContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public ValueTask<Result<int>> Handle(EditRegularQuizGameCommand request, CancellationToken cancellationToken)
        {
            return base.Handle(request, cancellationToken);
        }
    }
}
