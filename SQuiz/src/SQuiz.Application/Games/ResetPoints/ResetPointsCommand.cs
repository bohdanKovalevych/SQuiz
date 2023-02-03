using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;

namespace SQuiz.Application.Games.ResetPoints
{
    public class ResetPointsCommand : IRequest
    {
        public ResetPointsCommand(string playerId)
        {
            PlayerId = playerId;
        }

        public string PlayerId { get; set; }
    }

    public class ResetPointsCommandHandler : IRequestHandler<ResetPointsCommand>
    {
        private readonly ISQuizContext _context;

        public ResetPointsCommandHandler(ISQuizContext context)
        {
            _context = context;
        }

        public async ValueTask<Unit> Handle(ResetPointsCommand request, CancellationToken cancellationToken)
        {
            var player = await _context.Players
                .Include(x => x.PlayerAnswers)
                .FirstOrDefaultAsync(x => x.Id == request.PlayerId);
            
            if (player == null)
            {
                return Unit.Value;
            }

            player.Points = 0;
            _context.PlayerAnswers.RemoveRange(player.PlayerAnswers);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
