using SQuiz.Client.Interfaces;

namespace SQuiz.Client.Services
{
    public class InitGameService : IInitGameService
    {
        public event Func<int, Task> GameCodeChosen;
        public event Func<string, Task> PlayerNameChosen;
        public event Func<string, Task> JoinedWithExistingId;

        public Task ChooseGameCode(int code)
        {
            return GameCodeChosen.Invoke(code);
        }

        public Task ChoosePlayerName(string name)
        {
            return PlayerNameChosen.Invoke(name);
        }

        public Task JoinWithExistingId(string id)
        {
            return JoinedWithExistingId.Invoke(id);
        }
    }
}
