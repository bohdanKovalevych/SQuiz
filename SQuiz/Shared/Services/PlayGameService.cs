using SQuiz.Shared.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Models;

namespace SQuiz.Shared.Services
{
    public class PlayGameService : IPlayGameService, IDisposable
    {
        private readonly Dictionary<Question.ANSWERING_TIME, double> _maxSeconds;
        private System.Timers.Timer _gameTimer;
        private System.Timers.Timer _prepareTimer;
        private double _currentSeconds;
        private double _delayToPrepare = 4000;
        private double _gameTimeUint = 100;

        public PlayGameService()
        {
            _gameTimer = new System.Timers.Timer(_gameTimeUint);
            _prepareTimer = new System.Timers.Timer(_delayToPrepare);
            _prepareTimer.Elapsed += PrepareTimer_Elapsed;
            _gameTimer.Elapsed += GameTimer_Elapsed;
            _maxSeconds = new Dictionary<Question.ANSWERING_TIME, double>()
            { 
                [Question.ANSWERING_TIME.Short] = 20.0,
                [Question.ANSWERING_TIME.Long] = 40.0
            };
        }

        public double CurrentMaxTime { get; private set; }

        public void InitQuestion(GameQuestionDto questionDto)
        {
            CurrentMaxTime = _maxSeconds[questionDto.AnsweringTime];
            _currentSeconds = CurrentMaxTime;
        }

        public void DelayToPrepareForQuestion()
        {
            OnStartPreparing?.Invoke();
            _prepareTimer.Start();
        }

        public void StartTimer()
        {
            _gameTimer.Start();
        }

        public async Task SendAnswer(string? answerId)
        {
            _gameTimer.Stop();
            if (OnAnswered != null)
            {
                await OnAnswered(new SendAnswerDto()
                {
                    AnswerId = answerId,
                    TimeToSolve = TimeSpan.FromSeconds(_currentSeconds)
                });
            }
        }

        public void ReceivePoints(ReceivedPointsDto points)
        {
            OnReceivedPoints?.Invoke(points);
        }

        public async void EndQuiz()
        {
            if (OnQuizEnded != null)
            {
                await OnQuizEnded();
            }
            _prepareTimer?.Stop();
            _gameTimer?.Stop();
        }

        public void Dispose()
        {
            _prepareTimer?.Dispose();
            _gameTimer?.Dispose();
        }

        private async void GameTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _currentSeconds -= 0.1;
            
            if (_currentSeconds <= 0)
            {
                _gameTimer.Stop();
                
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

        private void PrepareTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _prepareTimer.Stop();
            OnPrepared?.Invoke();
        }

        public event Action? OnStartPreparing;
        public event Action? OnPrepared;
        public event Func<Task>? OnTimeEnd;
        public event Func<SendAnswerDto, Task>? OnAnswered;
        public event Action<ReceivedPointsDto>? OnReceivedPoints;
        public event Action<double>? OnTimeChanged;
        public event Func<Task>? OnQuizEnded;
    }
}
