using static SQuiz.Shared.Models.Question;

namespace SQuiz.Shared.Dtos.Game
{
    public class GameQuestionDto
    {
        public string Id { get; set; }
        public string QuestionText { get; set; }
        public string QuizId { get; set; }
        public int Order { get; set; }
        public ANSWERING_TIME AnsweringTime { get; set; }
        public POINTS Points { get; set; }

        public List<GameAnswerDto> Answers { get; set; } = new List<GameAnswerDto>();
    }

    public class GameAnswerDto
    {
        public string Id { get; set; }

        public string AnswerText { get; set; }

        public int Order { get; set; }
    }
}
