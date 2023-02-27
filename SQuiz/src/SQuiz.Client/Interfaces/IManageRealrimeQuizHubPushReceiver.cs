using SQuiz.Shared.Hubs.ManageRealtimeQuizHub;
using TypedSignalR.Client;

namespace SQuiz.Client.Interfaces
{
    internal interface IManageRealtimeQuizHubPushReceiver : IManageRealtimeQuizHubPush, IHubConnectionObserver
    {

    }
}
