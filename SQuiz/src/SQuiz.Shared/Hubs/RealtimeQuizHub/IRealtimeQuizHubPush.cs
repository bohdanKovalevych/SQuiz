using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Shared.Hubs.RealtimeQuizHub
{
    public interface IRealtimeQuizHubPush
    {
        Task OnGetQuestion(GameQuestionDto question);
        Task OnPlayerAnswered(ReceivedPointsDto playerPoints);
        Task OnAllPlayersAnswered(List<ReceivedPointsDto> playerPoints, string correctAnswerId);
        Task OnEndQuiz(List<PlayerDto> playerPoints);
        Task OnError(string message);
        Task OnStartQuiz();
        Task OnPlayerJoined(PlayerDto joinGame);
        Task OnPlayerLeft(PlayerDto player);
    }
}
