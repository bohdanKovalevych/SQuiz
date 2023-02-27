using AutoMapper;
using LanguageExt.Common;
using Mediator;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Models;

namespace SQuiz.Application.ManageGames.EditQuizGame
{
    public class EditRealtimeQuizGameCommand
         : EditQuizGameCommand<RealtimeQuizGame, StartRealtimeGameDto>, IRequest<Result<int>>
    {

    }

    public class EditRealtimeQuizGameCommandHandler
        : EditQuizGameCommandHandler<RealtimeQuizGame, StartRealtimeGameDto>, IRequestHandler<EditRealtimeQuizGameCommand, Result<int>>
    {
        public EditRealtimeQuizGameCommandHandler(ISQuizContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public ValueTask<Result<int>> Handle(EditRealtimeQuizGameCommand request, CancellationToken cancellationToken)
        {
            return base.Handle(request, cancellationToken);
        }
    }
}
