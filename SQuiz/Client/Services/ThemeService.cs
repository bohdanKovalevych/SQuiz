using Majorsoft.Blazor.Extensions.BrowserStorage;
using SQuiz.Client.Interfaces;

namespace SQuiz.Client.Services
{
    public class ThemeService : IThemeService
    {
        private readonly ISessionStorageService _session;

        public ThemeService(ISessionStorageService session)
        {
            _session = session;
        }

        public bool IsDarkMode { get; set; }

        public event Func<bool, Task> OnDarkModeChanged;
        public async Task InitializeAsync()
        {
            var exists = await _session.ContainKeyAsync("isDarkMode");
            IsDarkMode = exists 
                && await _session.GetItemAsync<bool>("isDarkMode");
        }

        public async Task ChangeDarkMode(bool isDarkMode)
        {
            if (OnDarkModeChanged != null)
            {
                await OnDarkModeChanged.Invoke(isDarkMode);
            }
            await _session.SetItemAsync("isDarkMode", isDarkMode);

            IsDarkMode = isDarkMode;
        }
    }
}
