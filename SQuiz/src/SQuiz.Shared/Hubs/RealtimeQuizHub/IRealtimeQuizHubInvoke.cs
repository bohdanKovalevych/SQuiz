using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Shared.Hubs.RealtimeQuizHub
{
    public interface IRealtimeQuizHubInvoke
    {
        Task JoinToGameEvents(int shortId);
        Task JoinQuiz(JoinGameDto joinGameDto);
        Task AnswerQuestion(SendAnswerDto sendAnswer);
    }
}
