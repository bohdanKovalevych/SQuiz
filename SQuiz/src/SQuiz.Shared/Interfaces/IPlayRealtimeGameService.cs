using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Shared.Interfaces
{
    public interface IPlayRealtimeGameService : IPlayGameService
    {
        void InvokeAllPlayersAnswered(List<ReceivedPointsDto> playerPoints, string correctAnswerId);
        void InvokeError(string? message);
        void InvokeGetQuestion(GameQuestionDto question);
        void InvokePlayerJoined(PlayerDto player);
        void InvokePlayerLeft(PlayerDto player);
        void InvokeStartQuiz();
        void InvokeOtherReceivedPoints(ReceivedPointsDto receivedPoints);

        public event Action<ReceivedPointsDto>? OnOtherReceivedPoints;
        public event Action<List<ReceivedPointsDto>, string>? OnAllPlayersAnswered;
        public event Action<string?>? OnError;
        public event Action? OnStartQuiz;
        public event Func<PlayerDto, Task>? OnPlayerJoined;
        public event Func<PlayerDto, Task>? OnPlayerLeft;
        public event Action<GameQuestionDto>? OnGetQuestion;
    }
}
