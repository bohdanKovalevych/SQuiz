namespace SQuiz.Client.Interfaces
{
    public interface IThemeService
    {
        bool IsDarkMode { get; set; }
        event Func<bool, Task> OnDarkModeChanged;
        Task ChangeDarkMode(bool isDarkMode);
        Task InitializeAsync();
    }
}
