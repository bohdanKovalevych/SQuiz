namespace SQuiz.Shared.Models
{
    public class QuizModerator : ResourceItem
    {
        public string QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public string ModeratorId { get; set; }
        public Moderator Moderator { get; set; }
    }
}
