using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using SQuiz.Client.Interfaces;
using SQuiz.Shared.Hubs.ManageRealtimeQuizHub;
using TypedSignalR.Client;

namespace SQuiz.Client.Services
{
    internal class ManageRealtimeQuizHubClient : IManageRealtimeQuizHubClient, IAsyncDisposable
    {
        private readonly HubConnection _connection;
        private readonly IDisposable _subscription;

        public ManageRealtimeQuizHubClient(IManageRealtimeQuizHubPushReceiver receiver, NavigationManager nav, IAccessTokenProvider tokenProvider)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(nav.ToAbsoluteUri("/manageQuizHub"), httpOptions =>
                {
                    httpOptions.AccessTokenProvider = async () =>
                    {
                        var result = await tokenProvider.RequestAccessToken();
                        if (result != null && result.TryGetToken(out var token))
                        {
                            return token.Value;
                        }

                        return null;
                    };
                })
                .Build();
            
            QuizHub = _connection.CreateHubProxy<IManageRealtimeQuizHubInvoke>();
            _subscription = _connection.Register(receiver);
            ManageQuizHubReceiver = receiver;
        }
        public Task StartAsync()
        {
            return _connection.StartAsync();
        }
        public Task StopAsync()
        {
            return _connection.StopAsync();
        }
        public IManageRealtimeQuizHubInvoke QuizHub { get; }

        public IManageRealtimeQuizHubPushReceiver ManageQuizHubReceiver { get; }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }

            _subscription?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
