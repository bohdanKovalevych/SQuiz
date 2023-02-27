using AutoMapper;
using LanguageExt.Common;
using Mediator;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Models;

namespace SQuiz.Application.Games.JoinGame
{
    public class JoinRealtimeGameCommand : JoinGameCommand<RealtimeQuizGame>, IRequest<Result<Unit>> { }

    public class JoinRealtimeGameCommandHandler : JoinGameCommandHandler<RealtimeQuizGame>, IRequestHandler<JoinRealtimeGameCommand, Result<Unit>>
    {
        public JoinRealtimeGameCommandHandler(ISQuizContext context, IMapper mapper) : base(context, mapper) { }

        public ValueTask<Result<Unit>> Handle(JoinRealtimeGameCommand request, CancellationToken cancellationToken)
        {
            return base.Handle(request, cancellationToken);
        }
    }
}
