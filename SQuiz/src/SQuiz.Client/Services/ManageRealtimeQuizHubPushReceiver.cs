using SQuiz.Client.Interfaces;
using SQuiz.Shared.Interfaces;

namespace SQuiz.Client.Services
{
    public class ManageRealtimeQuizHubPushReceiver : IManageRealtimeQuizHubPushReceiver
    {
        private readonly IPlayRealtimeGameService _playeGameService;

        public ManageRealtimeQuizHubPushReceiver(IPlayRealtimeGameService playeGameService)
        {
            _playeGameService = playeGameService;
        }

        public Task OnClosed(Exception? exception)
        {
            _playeGameService.InvokeError(exception?.Message);
            return Task.CompletedTask;
        }

        public Task OnError(string errorMessage)
        {
            _playeGameService.InvokeError(errorMessage);
            return Task.CompletedTask;
        }

        public Task OnReconnected(string? connectionId)
        {
            return Task.CompletedTask;
        }

        public Task OnReconnecting(Exception? exception)
        {
            _playeGameService.InvokeError(exception?.Message);
            return Task.CompletedTask;
        }
    }
}
