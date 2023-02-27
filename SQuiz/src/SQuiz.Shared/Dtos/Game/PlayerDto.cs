namespace SQuiz.Shared.Dtos.Game
{
    public class PlayerDto
    {
        public string Id { get; set; }
        public int ShortId { get; set; }
        public string QuizGameId { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }
        public int Points { get; set; }
        public bool IsOnline { get; set; }
    }
}
