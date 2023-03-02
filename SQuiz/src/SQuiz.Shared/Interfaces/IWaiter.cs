namespace SQuiz.Shared.Interfaces
{
    public interface IWaiter : IDisposable
    {
        Task Wait(double timeSeconds);

        /// <summary>
        /// Wait specified time and rise OnTimeChanged event every deltaMiliseconds
        /// </summary>
        /// <param name="timeSeconds">Time to wait in seconds</param>
        /// <param name="deltaMiliseconds">Time interval in ms to raise Event OnTimeChanged</param>
        /// <returns>true if it was stopped before and false otherwise</returns>

        Task<bool> Wait(double timeSeconds, double deltaMiliseconds);
        double Stop();

        event Action<double> OnTimeChanged;
    }
}
