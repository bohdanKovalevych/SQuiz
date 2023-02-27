namespace SQuiz.Shared.Dtos.Game
{
    public abstract class StartGameDto
    {
        public string? Id { get; set; }
        public string QuizId { get; set; }
        public int ShortId { get; set; }
        public string Name { get; set; }
    }

    public class StartRegularGameDto : StartGameDto
    {
        public DateTimeOffset? DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
    }

    public class StartRealtimeGameDto : StartGameDto
    {
        public bool IsOpen { get; set; }
    }
}
