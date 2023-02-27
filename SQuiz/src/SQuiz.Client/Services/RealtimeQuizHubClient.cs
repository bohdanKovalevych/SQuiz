using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SQuiz.Client.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Hubs.RealtimeQuizHub;
using TypedSignalR.Client;

namespace SQuiz.Client.Services
{
    internal class RealtimeQuizHubClient : IRealtimeQuizHubClient, IAsyncDisposable
    {
        private readonly ICurrentRealtimePlayerService _currentRealtimePlayerService;
        private readonly HubConnection _connection;
        private readonly IDisposable _subscription;

        public RealtimeQuizHubClient(IRealtimeQuizHubPushReceiver receiver, NavigationManager nav,
            ICurrentRealtimePlayerService currentRealtimePlayerService)
        {
            _currentRealtimePlayerService = currentRealtimePlayerService;
            _connection = new HubConnectionBuilder()
                .WithUrl(nav.ToAbsoluteUri("/quizHub"))
                .Build();

            QuizHub = _connection.CreateHubProxy<IRealtimeQuizHubInvoke>();
            _subscription = _connection.Register<IRealtimeQuizHubPush>(receiver);
            QuizHubReceiver = receiver;
        }

        public IRealtimeQuizHubInvoke QuizHub { get; }

        public IRealtimeQuizHubPushReceiver QuizHubReceiver { get; }

        public async ValueTask DisposeAsync()
        {
            _subscription?.Dispose();

            if (_connection is not null)
            {
                await _connection.DisposeAsync();
            }

            GC.SuppressFinalize(this);
        }

        public async Task StartAsync()
        {
            await _currentRealtimePlayerService.InitCurrentPlayerAsync();
            
            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
                
                if (_currentRealtimePlayerService.CurrentPlayer is PlayerDto player &&
                    _currentRealtimePlayerService.CurrentGame is RealtimeGameOptionDto game)
                {
                    await Reconnect(player, game);
                }
            }
        }

        async Task Reconnect(PlayerDto player, RealtimeGameOptionDto game)
        {
            await QuizHub.JoinQuiz(new JoinGameDto()
            {
                Name = player.Name,
                ShortId = game.ShortId,
                OldConnectionId = player.UserId
            });
        }

        public async Task StopAsync()
        {
            if (_connection.State != HubConnectionState.Disconnected)
            {
                await _connection.StopAsync();
            }
        }
    }

}
