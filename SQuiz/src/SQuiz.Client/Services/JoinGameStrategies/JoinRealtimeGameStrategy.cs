using Microsoft.AspNetCore.Components;
using SQuiz.Client.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Interfaces;

namespace SQuiz.Client.Services.JoinGameStrategies
{
    internal class JoinRealtimeGameStrategy : IJoinGameStrategy<RealtimeGameOptionDto>, IAsyncDisposable
    {
        private readonly NavigationManager _nav;
        private readonly IPlayRealtimeGameService _playGameService;
        private readonly IRealtimeQuizHubClient _hubClient;
        private readonly ICurrentRealtimePlayerService _currentRealtimePlayerService;
        private TaskCompletionSource<string?> _joinGameTaskSource = new TaskCompletionSource<string?>();


        private string? _lastPlayerName;
        private bool _keepConnectionOpened;

        public JoinRealtimeGameStrategy(
            NavigationManager nav,
            IPlayRealtimeGameService playeGameService,
            IRealtimeQuizHubClient hubClient,
            ICurrentRealtimePlayerService currentRealtimePlayerService)
        {
            _nav = nav;
            _playGameService = playeGameService;
            _hubClient = hubClient;
            _playGameService.OnPlayerJoined += OnPlayerJoined;
            _playGameService.OnError += OnError;
            _currentRealtimePlayerService = currentRealtimePlayerService;
        }

        public async Task<string?> JoinGame(JoinGameDto command)
        {
            await _currentRealtimePlayerService.SetCurrentPlayerAsync(null);
            _lastPlayerName = command.Name;
            await _hubClient.StartAsync();

            await _hubClient.QuizHub.JoinQuiz(command);
            var errorMessage = await _joinGameTaskSource.Task;

            if (errorMessage == null)
            {
                _keepConnectionOpened = true;
                _nav.NavigateTo("playRealtime/showGame");
            }

            return errorMessage;
        }

        private void OnError(string? error)
        {
            _joinGameTaskSource.TrySetResult(error);
        }

        private async Task OnPlayerJoined(PlayerDto player)
        {
            if (player.Name == _lastPlayerName)
            {
                await _currentRealtimePlayerService.SetCurrentPlayerAsync(player);
                _joinGameTaskSource.TrySetResult(null);
            }
        }

        public async ValueTask DisposeAsync()
        {
            _joinGameTaskSource.TrySetCanceled();
            _playGameService.OnPlayerJoined -= OnPlayerJoined;
            _playGameService.OnError -= OnError;

            if (!_keepConnectionOpened)
            {
                await _hubClient.StopAsync();
            }
        }
    }
}
