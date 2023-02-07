using SQuiz.Shared.Models;
using SQuiz.Shared.Models.Interfaces;

namespace SQuiz.Shared.Dtos.Quiz
{
    public class EditQuizDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; } = false;

        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
        public List<ModeratorDto> Moderators { get; set; } = new List<ModeratorDto>();
    }

    public class QuestionDto : IHasOrder
    {
        public string? Id { get; set; }
        public string QuestionText { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public Question.ANSWERING_TIME AnsweringTime { get; set; }
        public Question.POINTS Points { get; set; }
        public int Order { get; set; }
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }

    public class AnswerDto : IHasOrder
    {
        public string? Id { get; set; }
        public string AnswerText { get; set; }
        public int Order { get; set; }
    }
}
