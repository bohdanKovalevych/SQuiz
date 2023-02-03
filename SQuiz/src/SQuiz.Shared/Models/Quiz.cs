using SQuiz.Shared.Models.Interfaces;

namespace SQuiz.Shared.Models
{
    public class Quiz : ResourceItem
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string AuthorId { get; set; }
        public bool IsPublic { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizModerator> QuizModerators { get; set; } = new List<QuizModerator>();
        public ICollection<QuizGame> QuizGames { get; set; } = new List<QuizGame>();
    }
}
