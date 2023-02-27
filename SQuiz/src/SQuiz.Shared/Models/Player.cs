namespace SQuiz.Shared.Models
{
    public class Player : ResourceItem
    {
        public string QuizGameId { get; set; }
        public QuizGame QuizGame { get; set; }

        public string? RegularQuizGameId { get; set; }
        public RegularQuizGame? RegularQuizGame { get; set; }

        public string? RealtimeQuizGameId { get; set; }
        public RealtimeQuizGame? RealtimeQuizGame { get; set; }


        public string Name { get; set; }
        public string? UserId { get; set; }

        public int Points { get; set; }

        public bool IsOnline { get; set; }

        public ICollection<PlayerAnswer> PlayerAnswers { get; set; } = new List<PlayerAnswer>();
    }
}
