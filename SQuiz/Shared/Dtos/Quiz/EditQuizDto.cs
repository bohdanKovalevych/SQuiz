using SQuiz.Shared.Models;

namespace SQuiz.Shared.Dtos.Quiz
{
    public class EditQuizDto
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsPublic { get; set; } = false;

        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }

    public class QuestionDto
    {
        public string QuestionText { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public Question.ANSWERING_TIME AnsweringTime { get; set; }
        public Question.POINTS Points { get; set; }

        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }

    public class AnswerDto
    {
        public string AnswerText { get; set; }
        public int Order { get; set; }
    }
}
