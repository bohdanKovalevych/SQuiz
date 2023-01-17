using SQuiz.Shared.Models.Interfaces;

namespace SQuiz.Shared.Models
{
    public class PlayerAnswer : IEntity
    {
        public string Id { get; set; }

        public string PlayerId { get; set; }
        public Player Player { get; set; }

        public string? AnswerId { get; set; }
        public Answer Answer { get; set; }

        public int Points { get; set; }
    }
}
