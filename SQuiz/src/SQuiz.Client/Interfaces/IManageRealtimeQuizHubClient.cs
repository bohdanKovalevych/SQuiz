using SQuiz.Shared.Hubs.ManageRealtimeQuizHub;

namespace SQuiz.Client.Interfaces
{
    internal interface IManageRealtimeQuizHubClient
    {
        public IManageRealtimeQuizHubInvoke QuizHub { get; }

        public IManageRealtimeQuizHubPushReceiver ManageQuizHubReceiver { get; }

        Task StartAsync();
        Task StopAsync();
    }
}