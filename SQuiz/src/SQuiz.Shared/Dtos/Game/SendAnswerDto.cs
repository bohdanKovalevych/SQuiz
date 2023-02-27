namespace SQuiz.Shared.Dtos.Game
{
    public class SendAnswerDto
    {
        public SendAnswerDto(string questionId)
        {
            QuestionId = questionId;
        }

        public string QuestionId { get; set; }
        public string? AnswerId { get; set; }

        public TimeSpan TimeToSolve { get; set; }
    }
}
