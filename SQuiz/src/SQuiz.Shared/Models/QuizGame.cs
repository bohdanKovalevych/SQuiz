namespace SQuiz.Shared.Models
{
    public abstract class QuizGame : ResourceItem
    {
        public string StartedById { get; set; }
        public Moderator StartedBy { get; set; }
        
        public string Name { get; set; }

        public string QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
