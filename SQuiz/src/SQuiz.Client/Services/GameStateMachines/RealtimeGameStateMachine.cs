using SQuiz.Shared.Dtos.Game;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQuiz.Client.Services.GameStateMachines
{
    public class RealtimeGameStateMachine
    {
        public enum TRIGGER
        {
            StartQuiz,
            StartPreparingToQuestion,
            GetQuestion,
            AnswerQuestion,
            ReceivePoints,
            ReceiveOtherPoints,
            ReceiveAllPoints,
            EndTimeForQuestion,
            EndQuiz,
            ShowError,
            PlayerJoined,
            PlayerLeft
        }
        
        public enum STATE
        {
            PreviewingQuiz,
            PreparingToQuestion,
            Playing,
            Answered,
            ReceivedPoints,
            ShowingFinalScores
        }

        public STATE State = STATE.PreviewingQuiz;
        
        protected StateMachine<STATE, TRIGGER> StateMachine;

        public RealtimeGameStateMachine()
        {
            StateMachine = new StateMachine<STATE, TRIGGER>(State);
            _allPlayersAnsweredParams = StateMachine.SetTriggerParameters<List<ReceivedPointsDto>, string>(TRIGGER.ReceiveAllPoints);
            _errorParams = StateMachine.SetTriggerParameters<string?>(TRIGGER.ShowError);
            _getQuestionParams = StateMachine.SetTriggerParameters<GameQuestionDto>(TRIGGER.GetQuestion);
            _playerJoinedParams = StateMachine.SetTriggerParameters<PlayerDto>(TRIGGER.PlayerJoined);
            _playerLeftParams = StateMachine.SetTriggerParameters<PlayerDto>(TRIGGER.PlayerLeft);
            _otherReceivedPointsParams = StateMachine.SetTriggerParameters<ReceivedPointsDto>(TRIGGER.ReceiveOtherPoints);

            _receivePointsParams = StateMachine.SetTriggerParameters<ReceivedPointsDto>(TRIGGER.ReceivePoints);
            _sendAnswerParams = StateMachine.SetTriggerParameters<SendAnswerDto>(TRIGGER.AnswerQuestion);

            StateMachine.Configure(STATE.PreviewingQuiz)
                .Permit(TRIGGER.StartQuiz, STATE.Playing);

            StateMachine.Configure(STATE.Playing)
                .InitialTransition(STATE.PreparingToQuestion)
                .Permit(TRIGGER.AnswerQuestion, STATE.Answered);

            StateMachine.Configure(STATE.Answered)
                .Permit(TRIGGER.AnswerQuestion, STATE.Answered);
        }

        StateMachine<STATE, TRIGGER>.TriggerWithParameters<List<ReceivedPointsDto>, string> _allPlayersAnsweredParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<string?> _errorParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<GameQuestionDto> _getQuestionParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<PlayerDto> _playerJoinedParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<PlayerDto> _playerLeftParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<ReceivedPointsDto> _otherReceivedPointsParams;
        
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<ReceivedPointsDto> _receivePointsParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<SendAnswerDto> _sendAnswerParams;
    }
}
