namespace SQuiz.Client
{
    public static class Constants
    {
        public static class SessionStorageKey
        {
            public const string PreviewGame = nameof(PreviewGame);
            public const string PreviewScore = nameof(PreviewScore);
            public const string PreviewQuestion = nameof(PreviewQuestion);
            public const string PreviewAllQuestions = nameof(PreviewAllQuestions);
            public const string QuizStepsState = nameof(QuizStepsState);
            public const string Game = nameof(Game);
            public const string GameStarted = nameof(GameStarted);
            public const string GameState = nameof(GameState);
            public const string IsDarkMode = nameof(IsDarkMode);
        }

        public static class CookiesKey
        {
            public const string PlayerId = nameof(PlayerId);
            public const string QuestionIndex = nameof(QuestionIndex);
        }

        public static class HeadersKey
        {
            public const string ResponseEntityType = nameof(ResponseEntityType);
        }

    }
}
