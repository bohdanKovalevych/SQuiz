namespace SQuiz.Shared.Models
{
    public class Moderator : ResourceItem
    {
        public string? Name { get; set; }
        public string? Email { get; set; }

        public ICollection<QuizModerator> QuizModerators { get; set; } = new List<QuizModerator>();
        public ICollection<QuizGame> QuizGames { get; set; } = new List<QuizGame>();
        public ICollection<RegularQuizGame> RegularQuizGames { get; set; } = new List<RegularQuizGame>();
        public ICollection<RealtimeQuizGame> RealtimeQuizGames { get; set; } = new List<RealtimeQuizGame>();
    }
}
