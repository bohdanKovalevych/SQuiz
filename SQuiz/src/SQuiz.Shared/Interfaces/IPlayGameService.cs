using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Shared.Interfaces
{
    public interface IPlayGameService
    {
        double CurrentMaxTime { get; }
        void InitQuestion(GameQuestionDto questionDto);
        void StartTimer();
        void DelayToPrepareForQuestion();
        void ReceivePoints(ReceivedPointsDto points);
        void EndQuiz();
        Task SendAnswer(string AnswerId);
        
        event Action<ReceivedPointsDto>? OnReceivedPoints;
        event Action? OnStartPreparing;
        event Action? OnPrepared;
        event Func<Task>? OnTimeEnd;
        event Func<Task>? OnQuizEnded;
        event Func<SendAnswerDto, Task>? OnAnswered;
        event Action<double>? OnTimeChanged;
    }
}
