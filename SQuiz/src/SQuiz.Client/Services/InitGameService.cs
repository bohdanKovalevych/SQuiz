using Microsoft.AspNetCore.Components;
using SQuiz.Client.Interfaces;

namespace SQuiz.Client.Services
{
    public class InitGameService : IInitGameService
    {
        private readonly IClipboardService _clipboardService;
        private readonly NavigationManager _nav;

        public InitGameService(IClipboardService clipboardService, NavigationManager nav)
        {
            _clipboardService = clipboardService;
            _nav = nav;
        }

        public event Func<int, Task>? GameCodeChosen;
        public event Func<string, Task>? PlayerNameChosen;
        public event Func<string, Task>? JoinedWithExistingId;

        public async Task ChooseGameCode(int code)
        {
            if (GameCodeChosen != null)
            {
                await GameCodeChosen.Invoke(code);
            }
        }

        public async Task ChoosePlayerName(string name)
        {
            if (PlayerNameChosen != null)
            {
                await PlayerNameChosen.Invoke(name);
            }
        }

        public async Task CopyLink(int gameShortId)
        {
            await _clipboardService.CopyToClipboard(GetLink(gameShortId));
        }

        public string GetLink(int gameShortId)
        {
            return $"{_nav.BaseUri}chooseGame?game={gameShortId}";
        }

        public async Task JoinWithExistingId(string id)
        {
            if (JoinedWithExistingId != null)
            {
                await JoinedWithExistingId.Invoke(id);
            }
        }
    }
}
