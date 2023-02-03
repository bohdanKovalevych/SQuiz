namespace SQuiz.Shared.Dtos.Quiz
{
    public class QuizOptionDto
    {
        public string Id { get; set; }
        public int ShortId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string AuthorId { get; set; }
        public bool IsPublic { get; set; }
        public int QuestionsCount { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
    }
}
