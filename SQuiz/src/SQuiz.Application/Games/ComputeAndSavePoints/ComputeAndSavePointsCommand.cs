using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;

namespace SQuiz.Application.Games.ComputeAndSavePoints
{
    public class ComputeAndSavePointsCommand : IRequest
    {
        public ComputeAndSavePointsCommand(string playerId)
        {
            PlayerId = playerId;
        }

        public string PlayerId { get; set; }
    }

    public class ComputeAndSavePointsCommandHandler : IRequestHandler<ComputeAndSavePointsCommand>
    {
        private readonly ISQuizContext _context;

        public ComputeAndSavePointsCommandHandler(ISQuizContext context)
        {
            _context = context;
        }

        public async ValueTask<Unit> Handle(ComputeAndSavePointsCommand request, CancellationToken cancellationToken)
        {
            var player = await _context.Players
                .Include(x => x.PlayerAnswers)
                .FirstOrDefaultAsync(x => x.Id == request.PlayerId);

            if (player == null)
            {
                return Unit.Value;
            }

            player.Points = player.PlayerAnswers.Sum(x => x.Points);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
