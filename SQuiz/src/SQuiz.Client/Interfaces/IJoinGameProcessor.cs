using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Client.Interfaces
{
    public interface IJoinGameProcessor
    {
        Task<string?> JoinGame<TGameOption>(JoinGameDto joinGame)
            where TGameOption : GameOptionDto;
    }
}
