using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Interfaces;

namespace SQuiz.Shared.Services
{
    public class PlayRealtimeGameService : PlayGameService, IPlayRealtimeGameService
    {
        public PlayRealtimeGameService()
        {

        }

        public void InvokeAllPlayersAnswered(List<ReceivedPointsDto> playerPoints, string correctAnswerId)
        {
            OnAllPlayersAnswered?.Invoke(playerPoints, correctAnswerId);
        }

        public void InvokeError(string? message)
        {
            OnError?.Invoke(message);
        }

        public void InvokeGetQuestion(GameQuestionDto question)
        {
            OnGetQuestion?.Invoke(question);
        }

        public async void InvokePlayerJoined(PlayerDto player)
        {
            if (OnPlayerJoined != null)
            {
                await OnPlayerJoined.Invoke(player);
            }
        }

        public async void InvokePlayerLeft(PlayerDto player)
        {
            if (OnPlayerLeft != null)
            {
                await OnPlayerLeft.Invoke(player);
            }
        }

        public void InvokeStartQuiz()
        {
            OnStartQuiz?.Invoke();
        }

        public void InvokeOtherReceivedPoints(ReceivedPointsDto receivedPoints)
        {
            OnOtherReceivedPoints?.Invoke(receivedPoints);
        }

        public event Action<List<ReceivedPointsDto>, string>? OnAllPlayersAnswered;
        public event Action<string?>? OnError;
        public event Action? OnStartQuiz;
        public event Func<PlayerDto, Task>? OnPlayerJoined;
        public event Func<PlayerDto, Task>? OnPlayerLeft;
        public event Action<GameQuestionDto>? OnGetQuestion;
        public event Action<ReceivedPointsDto>? OnOtherReceivedPoints;
    }
}
