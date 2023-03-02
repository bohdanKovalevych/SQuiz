using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Shared.Interfaces
{
    public interface IPlayGameService
    {
        double CurrentMaxTime { get; set; }
        void InvokeOnTimeChanged(double timeInSeconds);
        Task InvokeOnTimeEnd(bool allAnswered);
        void InvokeOnStartPreparing();
        void InvokeOnPrepared();
        void InvokeOnReceivedPoints(ReceivedPointsDto points);
        void InvokeOnQuizEnded();
        Task InvokeOnAnswered(SendAnswerDto answer);
        
        event Action<ReceivedPointsDto>? OnReceivedPoints;
        event Action? OnStartPreparing;
        event Action? OnPrepared;
        event Func<bool, Task>? OnTimeEnd;
        event Func<Task>? OnQuizEnded;
        event Func<SendAnswerDto, Task>? OnAnswered;
        event Action<double>? OnTimeChanged;
    }
}
