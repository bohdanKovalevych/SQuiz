using AutoMapper;
using LanguageExt.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Exceptions;
using SQuiz.Shared.Models;

namespace SQuiz.Application.Games.JoinGame
{
    public abstract class JoinGameCommand<TGame>
        where TGame : QuizGame
    {
        public JoinGameDto GameDto { get; set; }

        public Func<TGame, string?> ValidateGame { get; set; }

        public Func<TGame, PlayerDto, Task>? OnPlayerLeft { get; set; }
        public Func<TGame, PlayerDto, Task>? OnPlayerJoined { get; set; }

        public Func<string> GetPlayerId { get; set; }

        public Func<string>? GetUserId { get; set; }
    }

    public abstract class JoinGameCommandHandler<TGame>
        where TGame : QuizGame
    {
        private readonly ISQuizContext _context;
        private readonly IMapper _mapper;

        public JoinGameCommandHandler(ISQuizContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async ValueTask<Result<Unit>> Handle(JoinGameCommand<TGame> request, CancellationToken cancellationToken)
        {
            var gameDto = request.GameDto;
            var gameId = gameDto.ShortId;
            var name = gameDto.Name;
            var game = await _context.Set<TGame>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ShortId == gameId, cancellationToken);
            var playerId = request.GetPlayerId();
            var userId = request.GetUserId != null ? request.GetUserId() : null;

            if (game == null)
            {
                return new Result<Unit>(new NotFoundException());
            }

            if (gameDto.OldConnectionId != null && await _context.Players
                .FirstOrDefaultAsync(x => x.UserId == gameDto.OldConnectionId, cancellationToken) 
                is Player reconnectingPlayer)
            {
                reconnectingPlayer.IsOnline = true;
                reconnectingPlayer.UserId = userId;
                reconnectingPlayer.Name = gameDto.Name;

                await _context.SaveChangesAsync(cancellationToken);
                
                if (request.OnPlayerJoined != null)
                {
                    var playerDto = _mapper.Map<PlayerDto>(reconnectingPlayer);
                    await request.OnPlayerJoined(game, playerDto);
                }

                return Unit.Value;
            }

            if (request.ValidateGame(game) is string validationMessage)
            {
                return new Result<Unit>(new BadRequestException(validationMessage));
            }

            if (await _context.Players.AnyAsync(x => x.Name == name && x.QuizGameId == game.Id, cancellationToken))
            {
                return new Result<Unit>(new BadRequestException($"This name '{name}' is already in use"));
            }

            
            if (await _context.Players.FirstOrDefaultAsync(x => (x.UserId == userId || x.Id == playerId) && x.QuizGameId == game.Id, cancellationToken)
                is Player existingPlayer)
            {
                if (request.OnPlayerLeft != null)
                {                    
                    var playerDto = _mapper.Map<PlayerDto>(existingPlayer);
                    await request.OnPlayerLeft(game, playerDto);
                }

                _context.Players.Remove(existingPlayer);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var player = new Player()
            {
                Id = playerId,
                Name = name,
                QuizGameId = game.Id,
                UserId = userId
            };

            _context.Players.Add(player);

            await _context.SaveChangesAsync(cancellationToken);
            
            if (request.OnPlayerJoined != null)
            {
                player.IsOnline = true;
                _context.Players.Update(player);
                await _context.SaveChangesAsync(cancellationToken);
                
                var playerDto = _mapper.Map<PlayerDto>(player);
                await request.OnPlayerJoined(game, playerDto);
            }

            return Unit.Value;
        }
    }
}
