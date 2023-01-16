namespace SQuiz.Shared.Models
{
    public class Player : ResourceItem
    {
        public string QuizGameId { get; set; }
        public QuizGame QuizGame { get; set; }

        public string Name { get; set; }
        public string? UserId { get; set; }

        public int Points { get; set; }

        public ICollection<PlayerAnswer> PlayerAnswers { get; set; } = new List<PlayerAnswer>();
    }
}
