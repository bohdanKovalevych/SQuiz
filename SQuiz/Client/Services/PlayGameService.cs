using SQuiz.Client.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Models;

namespace SQuiz.Client.Services
{
    public class PlayGameService : IPlayGameService, IDisposable
    {
        public event Func<Task>? OnTimeEnd;
        public event Func<SendAnswerDto, Task>? OnAnswered;
        public event Action<double>? OnTimeChanged;
        private System.Timers.Timer _timer;
        private double _currentSeconds;
        
        private readonly Dictionary<Question.ANSWERING_TIME, double> _maxSeconds;
        public PlayGameService()
        {
            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += Timer_Elapsed;
            _maxSeconds = new Dictionary<Question.ANSWERING_TIME, double>()
            { 
                [Question.ANSWERING_TIME.Short] = 20.0,
                [Question.ANSWERING_TIME.Long] = 40.0
            };
        }

        public double CurrentMaxTime { get; private set; }

        public void InitQuestionAndStartTimer(GameQuestionDto questionDto)
        {
            CurrentMaxTime = _maxSeconds[questionDto.AnsweringTime];
            _currentSeconds = CurrentMaxTime;
            _timer.Start();
        }

        public async Task SendAnswer(string? answerId)
        {
            _timer.Stop();
            await OnAnswered(new SendAnswerDto()
            {
                AnswerId = answerId,
                TimeToSolve = TimeSpan.FromSeconds(_currentSeconds)
            });
        }

        public void Dispose()
        {
            _timer.Elapsed -= Timer_Elapsed;
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }

        private async void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _currentSeconds -= 0.1;
            
            if (_currentSeconds <= 0)
            {
                _timer.Stop();
                
                if (OnTimeEnd != null)
                {
                    await OnTimeEnd.Invoke();
                }
                
                if (OnAnswered != null)
                {
                    await OnAnswered.Invoke(new SendAnswerDto());
                }

                return;
            }

            OnTimeChanged?.Invoke(_currentSeconds);
        }
    }
}
