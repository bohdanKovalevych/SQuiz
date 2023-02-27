namespace SQuiz.Shared.Dtos.Game
{
    public abstract class GameOptionDto
    {
        public string Id { get; set; }
        public int ShortId { get; set; }
        public string Name { get; set; }
        public string QuizId { get; set; }
        public int QuestionCount { get; set; }
        public abstract bool IsBlocked { get; }
    }

    public class RegularGameOptionDto : GameOptionDto
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsActive => DateTime.Now <= EndDate
            && DateTime.Now >= StartDate;
        public override bool IsBlocked => !IsActive;
    }

    public class RealtimeGameOptionDto : GameOptionDto
    {
        public int CurrentQuestionIndex { get; set; }
        public bool IsOpen { get; set; }
        public override bool IsBlocked => !IsOpen;
    }
}
