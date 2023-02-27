using Microsoft.AspNetCore.Components;
using SQuiz.Shared.Dtos.Game;
using System.Net.Http.Json;

namespace SQuiz.Client.Services.JoinGameStrategies
{
    internal class JoinRegularGameStrategy : IJoinGameStrategy<RegularGameOptionDto>
    {
        private readonly NavigationManager _nav;
        private readonly PublicClient _http;

        public JoinRegularGameStrategy(NavigationManager nav, PublicClient http)
        {
            _nav = nav;
            _http = http;
        }

        public async Task<string?> JoinGame(JoinGameDto command)
        {
            var result = await _http.Client.PostAsJsonAsync("Games/join", command);

            if (!result.IsSuccessStatusCode)
            {
                var errorMessage = await result.Content.ReadAsStringAsync();

                return errorMessage;
            }
            _nav.NavigateTo("play/showGame");

            return null;
        }
    }
}
