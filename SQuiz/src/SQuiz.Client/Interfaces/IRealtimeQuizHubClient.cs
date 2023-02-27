using SQuiz.Shared.Hubs.RealtimeQuizHub;

namespace SQuiz.Client.Interfaces
{
    internal interface IRealtimeQuizHubClient
    {
        public IRealtimeQuizHubInvoke QuizHub { get; }

        public IRealtimeQuizHubPushReceiver QuizHubReceiver { get; }

        Task StartAsync();
        Task StopAsync();
    }
}