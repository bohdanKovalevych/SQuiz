using AutoMapper;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Games.JoinGame;
using SQuiz.Application.Games.SendAnswer;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Exceptions;
using SQuiz.Shared.Hubs.RealtimeQuizHub;
using SQuiz.Shared.Models;

namespace SQuiz.Server.Hubs
{
    public partial class RealtimeQuizHub : Hub<IRealtimeQuizHubPush>, IRealtimeQuizHubInvoke
    {
        private readonly IMediator _mediator;
        private readonly ISQuizContext _context;
        private readonly IMapper _mapper;

        public RealtimeQuizHub(IMediator mediator, ISQuizContext context, IMapper mapper)
        {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }

        public async Task JoinQuiz(JoinGameDto joinGameDto)
        {
            var playerId = Guid.NewGuid().ToString();
            var joinGameCommand = new JoinRealtimeGameCommand()
            { 
                GameDto = joinGameDto, 
                ValidateGame = g => g.IsOpen ? null : "it is closed now",
                GetUserId = () => Context.ConnectionId,
                GetPlayerId = () => playerId,
                OnPlayerJoined = async (game, player) =>
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, joinGameDto.ShortId.ToString());
                    await Clients.Groups(game.ShortId.ToString()).OnPlayerJoined(player);
                },
                OnPlayerLeft = (game, player) => Clients.Groups(game.ShortId.ToString()).OnPlayerLeft(player)
            };

            var result = await _mediator.Send(joinGameCommand);
            await result.Match(_ => Task.CompletedTask, HandleError);
        }

        public async Task AnswerQuestion(SendAnswerDto sendAnswer)
        {
            var player = await _context.Players.FirstOrDefaultAsync(x => x.UserId == Context.ConnectionId);
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == sendAnswer.QuestionId);
            
            if (player == null || question == null)
            {
                await HandleError(new NotFoundException());
                return;
            }
            
            var playerId = player.Id;
            var command = new SendAnswerCommand(playerId, sendAnswer);
            var receivedPoints = await _mediator.Send(command);
            await receivedPoints.Match(
                async x =>
                {
                    var players = await _context.Players
                        .Include(x => x.PlayerAnswers)
                        .Where(x => x.QuizGame.Players.Any(x => x.Id == playerId))
                        .ToListAsync();
                    
                    var allAnswered = !players
                        .Where(x => x.IsOnline)
                        .Select(x => x.PlayerAnswers.FirstOrDefault(x => x.Order == question.Order))
                        .Any(x => x == null);

                    CalculatePoints(players);
                    await _context.SaveChangesAsync();
                    
                    if (allAnswered)
                    {
                        var points = GetReceivedPoints(players, x.GameShortId, question.Order);
                        var correctAnswerid = x.CorrectAnswerId;
                        await Clients.Groups(x.GameShortId.ToString())
                            .OnAllPlayersAnswered(points, correctAnswerid);
                    }

                    await Clients.Groups(x.GameShortId.ToString()).OnPlayerAnswered(x); 
                },
                HandleError);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var player = _context.Players
                .Include(x => x.QuizGame)
                .FirstOrDefault(x => x.UserId == Context.ConnectionId);
            
            if (player == null || player.QuizGame == null)
            {
                return;
            }

            player.IsOnline = false;
            await _context.SaveChangesAsync();

            var playerDto = _mapper.Map<PlayerDto>(player);
            await Clients.Group(player.QuizGame.ShortId.ToString())
                .OnPlayerLeft(playerDto);
        }

        public override async Task OnConnectedAsync()
        {
            var player = _context.Players
                .Include(x => x.QuizGame)
                .FirstOrDefault(x => x.UserId == Context.ConnectionId);

            if (player == null || player.QuizGame == null)
            {
                return;
            }
            
            player.IsOnline = true;
            await _context.SaveChangesAsync();

            var playerDto = _mapper.Map<PlayerDto>(player);
            await Groups.AddToGroupAsync(Context.ConnectionId, player.QuizGame.ShortId.ToString());

            await Clients.Group(player.QuizGame.ShortId.ToString())
                .OnPlayerJoined(playerDto);
        }

        private void CalculatePoints(List<Player> players)
        {
            foreach(var player in players)
            {
                player.Points = player.PlayerAnswers.Sum(x => x.Points);
            }
        }

        private List<ReceivedPointsDto> GetReceivedPoints(List<Player> players, int shortId, int order)
        {
            var receivedPoints = new List<ReceivedPointsDto>();
            
            foreach (var player in players)
            {
                var lastPlayerAnswer = player.PlayerAnswers.FirstOrDefault(x => x.Order == order);
                var points = new ReceivedPointsDto()
                {
                    Player = _mapper.Map<PlayerDto>(player),
                    TotalPoints = player.Points,
                    GameShortId = shortId,
                    CorrectAnswerId = lastPlayerAnswer?.CorrectAnswerId,
                    CurrentPoints = lastPlayerAnswer?.Points ?? 0,
                    SelectedAnswerId = lastPlayerAnswer?.AnswerId
                };

                receivedPoints.Add(points);
            }

            return receivedPoints;
        }

        private Task HandleError(Exception error)
        {
            return Clients.Caller.OnError(error.Message);
        }

        public Task JoinToGameEvents(int shortId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, shortId.ToString());
        }
    }
}
