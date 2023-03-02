using Majorsoft.Blazor.Extensions.BrowserStorage;
using SQuiz.Client.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Interfaces;
using SQuiz.Shared.Models;
using Stateless;

namespace SQuiz.Client.Services.GameStateMachines
{
    internal class RealtimeGameStateMachine : IRealtimeGameStateMachine
    {
        protected StateMachine<STATE, TRIGGER> StateMachine;
        private readonly ISessionStorageService _sessionStorage;
        private readonly IPlayRealtimeGameService _playGameService;
        private readonly ICurrentRealtimePlayerService _currentPlayerService;
        private readonly IWaiter _prepareToQuestionWaiter;
        private readonly IWaiter _gameTimeWaiter;

        private readonly Dictionary<Question.ANSWERING_TIME, double> _maxSeconds;
        private const double _delayInSecondsToPrepare = 4;
        private const double _gameDeltaTimeInMiliseconds = 100;

        public RealtimeGameStateMachine(
            ISessionStorageService sessionStorage,
            IPlayRealtimeGameService playGameService,
            ICurrentRealtimePlayerService currentPlayer,
            IWaiter prepareToQuestionWaiter,
            IWaiter gameTimeWaiter)
        {
            StateMachine = new StateMachine<STATE, TRIGGER>(STATE.Default);
            _sessionStorage = sessionStorage;
            _playGameService = playGameService;
            _currentPlayerService = currentPlayer;
            _prepareToQuestionWaiter = prepareToQuestionWaiter;
            _gameTimeWaiter = gameTimeWaiter;
            _maxSeconds = new Dictionary<Question.ANSWERING_TIME, double>()
            {
                [Question.ANSWERING_TIME.Short] = 20.0,
                [Question.ANSWERING_TIME.Long] = 40.0
            };
            _getFinalScoresParams = StateMachine.SetTriggerParameters<List<PlayerDto>>(TRIGGER.EndQuiz);
            _allPlayersAnsweredParams = StateMachine.SetTriggerParameters<List<ReceivedPointsDto>, string>(TRIGGER.ReceiveAllPoints);
            _errorParams = StateMachine.SetTriggerParameters<string?>(TRIGGER.ShowError);
            _getQuestionParams = StateMachine.SetTriggerParameters<GameQuestionDto>(TRIGGER.GetQuestion);
            _playerJoinedParams = StateMachine.SetTriggerParameters<PlayerDto>(TRIGGER.PlayerJoined);
            _playerLeftParams = StateMachine.SetTriggerParameters<PlayerDto>(TRIGGER.PlayerLeft);
            _otherReceivedPointsParams = StateMachine.SetTriggerParameters<ReceivedPointsDto>(TRIGGER.ReceiveOtherPoints);

            _receivePointsParams = StateMachine.SetTriggerParameters<ReceivedPointsDto>(TRIGGER.ReceivePoints);
            _sendAnswerParams = StateMachine.SetTriggerParameters<string?>(TRIGGER.AnswerQuestion);

            ConfigureStates();
        }

        public STATE State => StateMachine.State;

        public GameQuestionDto? CurrentQuestion { get; private set; }

        public double CurrentMaxTime { get; private set; }

        public List<PlayerDto> Players { get; private set; }

        public string? CurrentCorrectAnswerId { get; private set; }

        public List<ReceivedPointsDto>? CurrentAllReceivedPoints { get; private set; }
        public ReceivedPointsDto? CurrentReceivedPoints { get; private set; }

        public int PlayersAnswered { get; private set; }

        public RealtimeGameOptionDto? Game { get; private set; }

        private void ConfigureStates()
        {
            StateMachine.Configure(STATE.Default)
                .Permit(TRIGGER.Quit, STATE.Default)
                .Permit(TRIGGER.PreviewQuiz, STATE.PreviewingQuiz)
                .OnEntryFromAsync(TRIGGER.Quit, OnQuitHandler)
                .InternalTransition(_playerLeftParams, (player, t) => _playGameService.InvokeOnPlayerLeft(player))
                .InternalTransition(_playerJoinedParams, (player, t) => _playGameService.InvokeOnPlayerJoined(player))
                .InternalTransition(_errorParams, (error, t) => _playGameService.InvokeOnError(error));

            StateMachine.Configure(STATE.PreviewingQuiz)
                .SubstateOf(STATE.Default)
                .Permit(TRIGGER.StartQuiz, STATE.WaitingForQuestion);

            StateMachine.Configure(STATE.WaitingForQuestion)
                .SubstateOf(STATE.Default)
                .Permit(TRIGGER.GetQuestion, STATE.PreparingToQuestion)
                .Permit(TRIGGER.EndQuiz, STATE.ShowingFinalScores);

            StateMachine.Configure(STATE.PreparingToQuestion)
                .SubstateOf(STATE.Default)
                .OnExitAsync(PrepareToQuestion)
                .OnEntryFrom(_getQuestionParams, OnEntryFromGettingQuestion)
                .Permit(TRIGGER.EndPreparingToQuestion, STATE.Playing);

            StateMachine.Configure(STATE.Playing)
                .SubstateOf(STATE.Default)
                .OnEntry(OnEntryPlayingState)
                .InternalTransition(_otherReceivedPointsParams, OnEntryFromOtherAnswered)
                .OnExit(OnExitPlayingState)
                .Permit(TRIGGER.AnswerQuestion, STATE.Answered)
                .Permit(TRIGGER.EndTimeForQuestion, STATE.Answered)
                .Permit(TRIGGER.ReceiveAllPoints, STATE.ReceivedPoints);

            StateMachine.Configure(STATE.Answered)
                .SubstateOf(STATE.Default)
                .OnEntryFrom(_sendAnswerParams, OnEntryFromAnswerQuestion)
                .InternalTransition(_otherReceivedPointsParams, OnEntryFromOtherAnswered)
                .InternalTransition(_receivePointsParams, OnReceivedPointsHandler)
                .Permit(TRIGGER.ReceiveAllPoints, STATE.ReceivedPoints);

            StateMachine.Configure(STATE.ReceivedPoints)
                .SubstateOf(STATE.Default)
                .OnEntryFrom(_allPlayersAnsweredParams, OnAllAnsweredHandler)
                .Permit(TRIGGER.StartQuestion, STATE.WaitingForQuestion);

            StateMachine.Configure(STATE.ShowingFinalScores)
                .SubstateOf(STATE.Default)
                .OnEntryFrom(_getFinalScoresParams, OnEndQuizHandler)
                .Permit(TRIGGER.Quit, STATE.Default);
        }

        #region Handlers
        private async Task PrepareToQuestion()
        {
            _playGameService.InvokeOnStartPreparing();
            await _prepareToQuestionWaiter.Wait(_delayInSecondsToPrepare);
            _playGameService.InvokeOnPrepared();
        }

        private void OnEntryFromGettingQuestion(GameQuestionDto question, StateMachine<STATE, TRIGGER>.Transition t)
        {
            _playGameService.InvokeOnGetQuestion(question);
            CurrentQuestion = question;
            CurrentMaxTime = _maxSeconds[question.AnsweringTime];
            PlayersAnswered = 0;
        }

        private async void OnEntryPlayingState()
        {
            _gameTimeWaiter.OnTimeChanged += _playGameService.InvokeOnTimeChanged;
            var wasStopped = await _gameTimeWaiter.Wait(CurrentMaxTime, _gameDeltaTimeInMiliseconds);

            if (!wasStopped && CurrentQuestion != null)
            {
                await _playGameService.InvokeOnAnswered(new SendAnswerDto(CurrentQuestion.Id));
            }

            await _playGameService.InvokeOnTimeEnd(wasStopped);
        }

        private void OnExitPlayingState()
        {
            _gameTimeWaiter.OnTimeChanged -= _playGameService.InvokeOnTimeChanged;
        }

        private void OnEntryFromOtherAnswered(ReceivedPointsDto otherPoints, StateMachine<STATE, TRIGGER>.Transition t)
        {
            ++PlayersAnswered;
            _playGameService.InvokeOnOtherReceivedPoints(otherPoints);
        }

        private void OnEntryFromAnswerQuestion(string? answerId, StateMachine<STATE, TRIGGER>.Transition t)
        {
            var time = _gameTimeWaiter.Stop();

            if (CurrentQuestion != null)
            {
                var answer = new SendAnswerDto(CurrentQuestion.Id)
                {
                    AnswerId = answerId,
                    TimeToSolve = TimeSpan.FromSeconds(time)
                };

                _playGameService.InvokeOnAnswered(answer);
            }
        }

        private void OnAllAnsweredHandler(List<ReceivedPointsDto> points, string correctAnswerId, StateMachine<STATE, TRIGGER>.Transition t)
        {
            CurrentCorrectAnswerId = correctAnswerId;
            CurrentAllReceivedPoints = points;

            _playGameService.InvokeOnAllPlayersAnswered(points, correctAnswerId);
        }

        private void OnReceivedPointsHandler(ReceivedPointsDto points, StateMachine<STATE, TRIGGER>.Transition t)
        {
            CurrentReceivedPoints = points;
            _playGameService.InvokeOnReceivedPoints(points);
        }

        private void OnEndQuizHandler(List<PlayerDto> scores, StateMachine<STATE, TRIGGER>.Transition t)
        {
            _playGameService.InvokeOnQuizEnded();
            Players = scores;
        }

        private async Task OnQuitHandler()
        {
            await ClearState();
        }

        #endregion Handlers


        #region Pushes

        public Task OnGetQuestion(GameQuestionDto question)
        {
            return StateMachine.FireAsync(_getQuestionParams, question);
        }

        public async Task OnPlayerAnswered(ReceivedPointsDto playerPoints)
        {
            if (_currentPlayerService.CurrentPlayer?.Id == playerPoints?.Player?.Id)
            {
                await StateMachine.FireAsync(_receivePointsParams, playerPoints);
            }
            else
            {
                await StateMachine.FireAsync(_otherReceivedPointsParams, playerPoints);
            }
        }

        public Task OnAllPlayersAnswered(List<ReceivedPointsDto> playerPoints, string correctAnswerId)
        {
            return StateMachine.FireAsync(_allPlayersAnsweredParams, playerPoints, correctAnswerId);
        }

        public Task OnEndQuiz(List<PlayerDto> playerPoints)
        {
            return StateMachine.FireAsync(_getFinalScoresParams, playerPoints);
        }

        public Task OnError(string message)
        {
            return StateMachine.FireAsync(_errorParams, message);
        }

        public Task OnStartQuiz()
        {
            _playGameService.InvokeOnStartQuiz();
            return StateMachine.FireAsync(TRIGGER.StartQuiz);
        }

        public Task OnPlayerJoined(PlayerDto joinGame)
        {
            return StateMachine.FireAsync(_playerJoinedParams, joinGame);
        }

        public Task OnPlayerLeft(PlayerDto player)
        {
            return StateMachine.FireAsync(_playerLeftParams, player);
        }

        public Task OnClosed(Exception? exception)
        {
            return StateMachine.FireAsync(_errorParams, exception?.Message);
        }

        public Task OnReconnected(string? connectionId)
        {
            return Task.CompletedTask;
        }

        public Task OnReconnecting(Exception? exception)
        {
            return StateMachine.FireAsync(_errorParams, exception?.Message);
        }

        #endregion Pushes

        public async Task SaveState()
        {
            await _sessionStorage.SetItemAsync(Constants.SessionStorageKey.GameState, this);
        }

        public async Task ClearState()
        {
            await _sessionStorage.RemoveItemAsync(Constants.SessionStorageKey.GameState);
        }

        public async Task RestoreState()
        {
            if (await _sessionStorage.GetItemAsync<RealtimeGameStateMachine>(Constants.SessionStorageKey.GameState)
                is RealtimeGameStateMachine instance)
            {
                CurrentAllReceivedPoints = instance.CurrentAllReceivedPoints;
                CurrentCorrectAnswerId = instance.CurrentCorrectAnswerId;
                CurrentMaxTime = instance.CurrentMaxTime;
                CurrentQuestion = instance.CurrentQuestion;
                CurrentReceivedPoints = instance.CurrentReceivedPoints;
                Players = instance.Players;
                PlayersAnswered = instance.PlayersAnswered;
                StateMachine = instance.StateMachine;
                Game = instance.Game;
            }
        }

        public void PreviewQuiz(RealtimeGameOptionDto game)
        {
            Game = game;
            StateMachine.Fire(TRIGGER.PreviewQuiz);
        }

        public Task AnswerQuestion(string? answerId)
        {
            return StateMachine.FireAsync(_sendAnswerParams, answerId);
        }

        public Task EndTimeForQuestion()
        {
            _playGameService.InvokeOnTimeEnd(false);
            return StateMachine.FireAsync(TRIGGER.EndTimeForQuestion);
        }

        public Task StartQuestion()
        {
            return StateMachine.FireAsync(TRIGGER.StartQuestion);
        }

        #region StateTriggerParams

        StateMachine<STATE, TRIGGER>.TriggerWithParameters<List<ReceivedPointsDto>, string> _allPlayersAnsweredParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<List<PlayerDto>> _getFinalScoresParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<string?> _errorParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<GameQuestionDto> _getQuestionParams;

        StateMachine<STATE, TRIGGER>.TriggerWithParameters<PlayerDto> _playerJoinedParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<PlayerDto> _playerLeftParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<ReceivedPointsDto> _otherReceivedPointsParams;

        StateMachine<STATE, TRIGGER>.TriggerWithParameters<ReceivedPointsDto> _receivePointsParams;
        StateMachine<STATE, TRIGGER>.TriggerWithParameters<string?> _sendAnswerParams;

        #endregion StateTriggerParams

        public enum TRIGGER
        {
            PreviewQuiz,
            StartQuiz,
            StartQuestion,
            GetQuestion,
            StartPreparingToQuestion,
            EndPreparingToQuestion,
            StartTimer,
            AnswerQuestion,
            ReceivePoints,
            ReceiveOtherPoints,
            ReceiveAllPoints,
            EndTimeForQuestion,
            EndQuiz,
            ShowError,
            PlayerJoined,
            PlayerLeft,
            Quit
        }

        public enum STATE
        {
            Default,
            PreviewingQuiz,
            WaitingForQuestion,
            PreparingToQuestion,
            Playing,
            Answered,
            ReceivedPoints,
            ShowingFinalScores
        }
    }
}
