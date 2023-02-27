using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Client.Interfaces
{
    public interface ICurrentRealtimePlayerService
    {
        PlayerDto? CurrentPlayer { get; }
        RealtimeGameOptionDto? CurrentGame { get; }
        Task InitCurrentPlayerAsync();
        Task SetCurrentPlayerAsync(PlayerDto? currentPlayer);
    }
}
