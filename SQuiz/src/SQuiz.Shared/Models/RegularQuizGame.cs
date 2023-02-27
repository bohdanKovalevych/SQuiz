namespace SQuiz.Shared.Models
{
    public class RegularQuizGame : QuizGame
    {
        public DateTimeOffset? DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
    }
}
