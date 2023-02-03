namespace SQuiz.Shared.Settings
{
    public class QuizSettings
    {
        public const string Name = nameof(QuizSettings);
        public int NormalAnsweringTimeInSeconds { get; set; }
        public int LongAnsweringTimeInSeconds { get; set; }
        public int MaximumPointsForQuestion { get; set; }
        public int MaximumDoublePointsForQuestion { get; set; }
    }
}
