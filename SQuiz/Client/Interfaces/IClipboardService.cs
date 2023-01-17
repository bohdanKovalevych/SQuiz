namespace SQuiz.Client.Interfaces
{
    public interface IClipboardService
    {
        Task CopyToClipboard(string text);
    }
}
