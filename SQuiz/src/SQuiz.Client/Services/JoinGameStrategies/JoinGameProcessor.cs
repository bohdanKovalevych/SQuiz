using SQuiz.Client.Interfaces;
using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Client.Services.JoinGameStrategies
{
    public class JoinGameProcessor : IJoinGameProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public JoinGameProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<string?> JoinGame<TGameOption>(JoinGameDto joinGame)
            where TGameOption : GameOptionDto
        {
            var service = _serviceProvider.GetRequiredService<IJoinGameStrategy<TGameOption>>();
            return service.JoinGame(joinGame);
        }
    }
}
