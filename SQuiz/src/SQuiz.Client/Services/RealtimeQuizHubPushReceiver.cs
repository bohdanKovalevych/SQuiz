using SQuiz.Client.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Interfaces;

namespace SQuiz.Client.Services
{
    internal class RealtimeQuizHubPushReceiver : IRealtimeQuizHubPushReceiver
    {
        private readonly IPlayRealtimeGameService _playGameService;
        private readonly ICurrentRealtimePlayerService _currentRealtimePlayer;

        public RealtimeQuizHubPushReceiver(IPlayRealtimeGameService playeGameService, 
            ICurrentRealtimePlayerService currentRealtimePlayer)
        {
            _playGameService = playeGameService;
            _currentRealtimePlayer = currentRealtimePlayer;
        }

        public Task OnAllPlayersAnswered(List<ReceivedPointsDto> playerPoints, string correctAnswerId)
        {
            _playGameService.InvokeOnAllPlayersAnswered(playerPoints, correctAnswerId);
            return Task.CompletedTask;
        }

        public Task OnClosed(Exception? exception)
        {
            _playGameService.InvokeOnError(exception?.Message);
            return Task.CompletedTask;
        }

        public Task OnEndQuiz(List<PlayerDto> playerPoints)
        {
            _playGameService.InvokeOnQuizEnded();
            return Task.CompletedTask;
        }

        public Task OnError(string message)
        {
            _playGameService.InvokeOnError(message);
            return Task.CompletedTask;
        }

        public Task OnGetQuestion(GameQuestionDto question)
        {
            _playGameService.InitQuestion(question);
            _playGameService.InvokeOnGetQuestion(question);
            return Task.CompletedTask;
        }

        public Task OnPlayerAnswered(ReceivedPointsDto playerPoints)
        {
            if (_currentRealtimePlayer.CurrentPlayer?.Id == playerPoints.Player?.Id)
            {
                _playGameService.InvokeOnReceivedPoints(playerPoints);
            }
            else
            {
                _playGameService.InvokeOnOtherReceivedPoints(playerPoints);
            }

            return Task.CompletedTask;
        }

        public async Task OnPlayerJoined(PlayerDto player)
        {
            if (player.Name == _currentRealtimePlayer.CurrentPlayer?.Name)
            {
                await _currentRealtimePlayer.SetCurrentPlayerAsync(player);
            }
            
            _playGameService.InvokeOnPlayerJoined(player);
        }

        public Task OnPlayerLeft(PlayerDto player)
        {
            _playGameService.InvokeOnPlayerLeft(player);
            return Task.CompletedTask;
        }

        public Task OnReconnected(string? connectionId)
        {
            return Task.CompletedTask;
        }

        public Task OnReconnecting(Exception? exception)
        {
            return Task.CompletedTask;
        }

        public Task OnStartQuiz()
        {
            _playGameService.InvokeOnStartQuiz();
            return Task.CompletedTask;
        }
    }
}
