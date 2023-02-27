namespace SQuiz.Shared.Hubs.ManageRealtimeQuizHub
{
    public interface IManageRealtimeQuizHubPush
    {
        Task OnError(string errorMessage);
    }
}
