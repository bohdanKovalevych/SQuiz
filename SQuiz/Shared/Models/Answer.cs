using SQuiz.Shared.Models.Interfaces;

namespace SQuiz.Shared.Models
{
    public class Answer : IEntity
    {
        public string Id { get; set; }

        public string AnswerText { get; set; }

        public int Order { get; set; }

        public string QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
