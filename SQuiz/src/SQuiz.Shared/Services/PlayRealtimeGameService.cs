using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Interfaces;

namespace SQuiz.Shared.Services
{
    public class PlayRealtimeGameService : PlayGameService, IPlayRealtimeGameService
    {
        public void InvokeOnAllPlayersAnswered(List<ReceivedPointsDto> playerPoints, string correctAnswerId)
        {
            OnAllPlayersAnswered?.Invoke(playerPoints, correctAnswerId);
        }

        public void InvokeOnError(string? message)
        {
            OnError?.Invoke(message);
        }

        public void InvokeOnGetQuestion(GameQuestionDto question)
        {
            OnGetQuestion?.Invoke(question);
        }

        public async void InvokeOnPlayerJoined(PlayerDto player)
        {
            if (OnPlayerJoined != null)
            {
                await OnPlayerJoined.Invoke(player);
            }
        }

        public async void InvokeOnPlayerLeft(PlayerDto player)
        {
            if (OnPlayerLeft != null)
            {
                await OnPlayerLeft.Invoke(player);
            }
        }

        public void InvokeOnStartQuiz()
        {
            OnStartQuiz?.Invoke();
        }

        public void InvokeOnOtherReceivedPoints(ReceivedPointsDto receivedPoints)
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
