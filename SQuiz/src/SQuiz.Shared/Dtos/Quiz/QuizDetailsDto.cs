namespace SQuiz.Shared.Dtos.Quiz
{
    public class QuizDetailsDto
    {
        public string Id { get; set; }
        public int ShortId { get; set; }
        public string AthorId { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; } = false;
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
        public List<ModeratorDto> Moderators { get; set; } = new List<ModeratorDto>();
    }
}
