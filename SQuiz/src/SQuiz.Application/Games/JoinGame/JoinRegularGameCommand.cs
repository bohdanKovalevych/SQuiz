using AutoMapper;
using LanguageExt.Common;
using Mediator;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Models;

namespace SQuiz.Application.Games.JoinGame
{
    public class JoinRegularGameCommand : JoinGameCommand<RegularQuizGame>, IRequest<Result<Unit>> { }

    public class JoinGameCommandHandler : JoinGameCommandHandler<RegularQuizGame>, IRequestHandler<JoinRegularGameCommand, Result<Unit>>
    {
        public JoinGameCommandHandler(ISQuizContext context, IMapper mapper) : base(context, mapper) { }

        public ValueTask<Result<Unit>> Handle(JoinRegularGameCommand request, CancellationToken cancellationToken)
        {
            return base.Handle(request, cancellationToken);
        }
    }
}
