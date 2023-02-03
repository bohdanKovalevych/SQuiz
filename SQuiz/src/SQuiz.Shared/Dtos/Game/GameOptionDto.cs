namespace SQuiz.Shared.Dtos.Game
{
    public class GameOptionDto
    {
        public string Id { get; set; }
        public int ShortId { get; set; }
        public string Name { get; set; }
        public string QuizId { get; set; }
        public int QuestionCount { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public bool IsActive => DateTime.Now <= EndDate 
            && DateTime.Now >= StartDate;
    }
}
