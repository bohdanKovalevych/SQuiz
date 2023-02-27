using Majorsoft.Blazor.Extensions.BrowserStorage;
using SQuiz.Client.Interfaces;
using SQuiz.Shared.Dtos.Game;
using System.Net.Http.Json;

namespace SQuiz.Client.Services
{
    public class CurrentRealtimePlayerService : ICurrentRealtimePlayerService
    { 
        private readonly ISessionStorageService _sessionStorageService;
        private readonly PublicClient _http;

        public CurrentRealtimePlayerService(ISessionStorageService sessionStorage, PublicClient client)
        {
            _sessionStorageService = sessionStorage;
            _http = client;
        }

        public PlayerDto? CurrentPlayer { get; set; }
        public RealtimeGameOptionDto? CurrentGame { get; set; }

        public async Task InitCurrentPlayerAsync()
        {
            var playerId = await _sessionStorageService.GetItemAsStringAsync(Constants.CookiesKey.PlayerId);

            if (playerId != null && await _http.Client.GetAsync($"Games/players/{playerId}")
                is HttpResponseMessage message && message.IsSuccessStatusCode 
                && await message.Content.ReadFromJsonAsync<PlayerDto>() is PlayerDto player)
            {
                CurrentPlayer = player;
            }

            CurrentGame = await _sessionStorageService.GetItemAsync<RealtimeGameOptionDto>(Constants.SessionStorageKey.Game);
        }

        public async Task SetCurrentPlayerAsync(PlayerDto? currentPlayer)
        {
            CurrentPlayer = currentPlayer;
            await _sessionStorageService.SetItemAsync(Constants.CookiesKey.PlayerId, currentPlayer?.Id);
        }
    }
}
