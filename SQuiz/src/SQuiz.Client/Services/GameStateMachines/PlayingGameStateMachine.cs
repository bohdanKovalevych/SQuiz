using SQuiz.Client.Interfaces;
using Stateless;

namespace SQuiz.Client.Services.GameStateMachines
{
    public class PlayingGameStateMachine
    {
    
        private readonly StateMachine<STATE, TRIGGER> _machine;
        private readonly ICurrentRealtimePlayerService _currentRealtimePlayer;
        public PlayingGameStateMachine(ICurrentRealtimePlayerService currentRealtimePlayer)
        {
            _currentRealtimePlayer = currentRealtimePlayer;
            _machine = new StateMachine<STATE, TRIGGER>(STATE.Iddle);
            
            _machine.Configure(STATE.Iddle)
                .OnActivateAsync(InitializeAsync)
                .Permit(TRIGGER.PreviewQuiz, STATE.PreviewingQuiz)
                .Permit(TRIGGER.PlayQuiz, STATE.PlayingQuiz)
                ;

        }

        private async Task InitializeAsync()
        {
            await _currentRealtimePlayer.InitCurrentPlayerAsync();

            //_currentRealtimePlayer.CurrentGame
            _machine.Fire(TRIGGER.PreviewQuiz);
        }


        public enum STATE
        {
            Iddle,
            PreviewingQuiz,
            PlayingQuiz,
            Answered
        }

        public enum TRIGGER
        {
            PreviewQuiz,
            PlayQuiz,
            Answer
        }
    }
}
