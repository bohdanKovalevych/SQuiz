namespace SQuiz.Client.Interfaces
{
    public interface IInitGameService
    {
        event Func<int, Task> GameCodeChosen;
        event Func<string, Task> PlayerNameChosen;
        event Func<string, Task> JoinedWithExistingId;

        Task ChooseGameCode(int code);
        Task ChoosePlayerName(string name);
        Task JoinWithExistingId(string id);
        Task CopyLink(int gameShortId);
        string GetLink(int gameShortId);
        string GetQrCode(int gameShortId);
    }
}
