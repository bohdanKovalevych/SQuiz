namespace SQuiz.Shared.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string QuestionText { get; set; }
        public string? CorrectAnswerId { get; set; }
        public Answer CorrectAnswer { get; set; }
        public string QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public int Order { get; set; }
        public ANSWERING_TIME AnsweringTime { get; set; }
        public POINTS Points { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public enum ANSWERING_TIME
        {
            Short,
            Long
        }

        public enum POINTS
        {
            Normal,
            Double
        }
    }
}
