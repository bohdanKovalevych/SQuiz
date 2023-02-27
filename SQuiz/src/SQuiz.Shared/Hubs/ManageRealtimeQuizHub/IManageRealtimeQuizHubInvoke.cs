namespace SQuiz.Shared.Hubs.ManageRealtimeQuizHub
{
    public interface IManageRealtimeQuizHubInvoke
    {
        Task NextQuestion(int shortId);

        Task TimeEnd(int shortId);
    }
}
