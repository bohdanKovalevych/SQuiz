using SQuiz.Shared.Hubs.RealtimeQuizHub;
using TypedSignalR.Client;

namespace SQuiz.Client.Interfaces
{
    internal interface IRealtimeQuizHubPushReceiver : IRealtimeQuizHubPush, IHubConnectionObserver
    {

    }
}
