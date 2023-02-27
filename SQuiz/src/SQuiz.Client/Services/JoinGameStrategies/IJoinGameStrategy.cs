using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Client.Services.JoinGameStrategies
{
    internal interface IJoinGameStrategy<T>
        where T : GameOptionDto
    {
        Task<string?> JoinGame(JoinGameDto command);
    }
}
