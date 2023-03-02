using SQuiz.Shared.Interfaces;
using Timer = System.Timers.Timer;

namespace SQuiz.Shared.Services
{
    public class Waiter : IWaiter, IDisposable
    {
        public event Action<double>? OnTimeChanged;
        private Timer _timer;
        private TaskCompletionSource<bool> _timerTask;
        private double _time;
        private double _delta;
        private bool _needCallCallback;
        
        public Waiter()
        {
            _timer = new Timer();
            _timerTask = new TaskCompletionSource<bool>();
            _timer.Elapsed += Timer_Elapsed;
        }

        public async Task<bool> Wait(double timeSeconds, double deltaMiliseconds)
        {
            _timer.Stop();
            _needCallCallback = true;
            _timer.Interval = deltaMiliseconds;
            _timer.Start();
            _delta = deltaMiliseconds;
            _time = timeSeconds;
            _timerTask = new TaskCompletionSource<bool>();
            return await _timerTask.Task;
        }

        public double Stop()
        {
            _timer.Stop();
            _timerTask?.TrySetResult(true);
            
            return _time;
        }

        public void Dispose()
        {
            _timer.Stop();            
            _timer.Elapsed -= Timer_Elapsed;
            _timer.Dispose();
            GC.SuppressFinalize(this);
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            double deltaInSeconds = _delta / 1000.0;
            _time -= deltaInSeconds;
            
            if (_needCallCallback) 
            {
                OnTimeChanged?.Invoke(_time);
            }

            if (_time <= 0)
            {
                _timer.Stop();
                _timerTask?.TrySetResult(false);
            }
        }

        public async Task Wait(double time)
        {
            _timer.Stop();
            _needCallCallback = false;
            _timer.Interval = time;
            _timer.Start();
            _delta = time;
            _time = time;

            _timerTask = new TaskCompletionSource<bool>();
            await _timerTask.Task;
        }
    }
}
