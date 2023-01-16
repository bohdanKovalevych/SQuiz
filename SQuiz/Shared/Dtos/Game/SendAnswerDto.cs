namespace SQuiz.Shared.Dtos.Game
{
    public class SendAnswerDto
    {
        public string? AnswerId { get; set; }

        public TimeSpan TimeToSolve { get; set; }
    }
}
