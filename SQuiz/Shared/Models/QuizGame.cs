namespace SQuiz.Shared.Models
{
    public class QuizGame : ResourceItem
    {
        public DateTimeOffset? DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        
        public string StartedById { get; set; }
        public Moderator StartedBy { get; set; }
        
        public string QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
