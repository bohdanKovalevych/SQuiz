namespace SQuiz.Shared.Dtos.Game
{
    public class StartGameDto
    {
        public string? Id { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string QuizId { get; set; }
        public int ShortId { get; set; }
    }
}
