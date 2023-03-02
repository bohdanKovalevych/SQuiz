using SQuiz.Client.Services.GameStateMachines;
using SQuiz.Shared.Dtos.Game;
using Stateless;
using static SQuiz.Client.Services.GameStateMachines.RealtimeGameStateMachine;

namespace SQuiz.Client.Interfaces
{
    internal interface IRealtimeGameStateMachine : IRealtimeQuizHubPushReceiver
    {
        STATE State { get; }
        GameQuestionDto? CurrentQuestion { get; }
        double CurrentMaxTime { get; }
        List<PlayerDto> Players { get; }
        string? CurrentCorrectAnswerId { get; }
        List<ReceivedPointsDto>? CurrentAllReceivedPoints { get; }
        ReceivedPointsDto? CurrentReceivedPoints { get; }
        int PlayersAnswered { get; }

        Task SaveState();
        Task ClearState();
        Task RestoreState();
        void PreviewQuiz(RealtimeGameOptionDto game);
        Task AnswerQuestion(string? answerId);
        Task EndTimeForQuestion();
        Task StartQuestion();
    }
}
