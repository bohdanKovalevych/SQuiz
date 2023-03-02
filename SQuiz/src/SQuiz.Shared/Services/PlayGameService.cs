using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Interfaces;

namespace SQuiz.Shared.Services
{
    public class PlayGameService : IPlayGameService
    {
        public double CurrentMaxTime { get; set; }
        public void InvokeOnStartPreparing()
        {
            OnStartPreparing?.Invoke();
        }

        public void InvokeOnPrepared()
        {
            OnPrepared?.Invoke();
        }

        public async Task InvokeOnTimeEnd(bool allAnswered)
        {
            if (OnTimeEnd != null)
            {
                await OnTimeEnd.Invoke(allAnswered);
            }
        }

        public void InvokeOnReceivedPoints(ReceivedPointsDto points)
        {
            OnReceivedPoints?.Invoke(points);
        }

        public async void InvokeOnQuizEnded()
        {
            if (OnQuizEnded != null)
            {
                await OnQuizEnded();
            }
        }

        public void InvokeOnTimeChanged(double timeInSeconds)
        {
            OnTimeChanged?.Invoke(timeInSeconds);
        }

        public Task InvokeOnAnswered(SendAnswerDto answer)
        {
            throw new NotImplementedException();
        }

        public event Action? OnStartPreparing;
        public event Action? OnPrepared;
        public event Func<bool, Task>? OnTimeEnd;
        public event Func<SendAnswerDto, Task>? OnAnswered;
        public event Action<ReceivedPointsDto>? OnReceivedPoints;
        public event Action<double>? OnTimeChanged;
        public event Func<Task>? OnQuizEnded;
    }
}
