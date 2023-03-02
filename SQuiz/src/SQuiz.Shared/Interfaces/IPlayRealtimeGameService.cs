using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Shared.Interfaces
{
    public interface IPlayRealtimeGameService : IPlayGameService
    {
        void InvokeOnAllPlayersAnswered(List<ReceivedPointsDto> playerPoints, string correctAnswerId);
        void InvokeOnError(string? message);
        void InvokeOnGetQuestion(GameQuestionDto question);
        void InvokeOnPlayerJoined(PlayerDto player);
        void InvokeOnPlayerLeft(PlayerDto player);
        void InvokeOnStartQuiz();
        void InvokeOnOtherReceivedPoints(ReceivedPointsDto receivedPoints);

        public event Action<ReceivedPointsDto>? OnOtherReceivedPoints;
        public event Action<List<ReceivedPointsDto>, string>? OnAllPlayersAnswered;
        public event Action<string?>? OnError;
        public event Action? OnStartQuiz;
        public event Func<PlayerDto, Task>? OnPlayerJoined;
        public event Func<PlayerDto, Task>? OnPlayerLeft;
        public event Action<GameQuestionDto>? OnGetQuestion;
    }
}
