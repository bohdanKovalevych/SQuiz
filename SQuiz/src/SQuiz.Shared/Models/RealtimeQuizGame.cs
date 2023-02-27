namespace SQuiz.Shared.Models
{
    public class RealtimeQuizGame : QuizGame
    {
        public int CurrentQuestionIndex { get; set; }
        public bool IsOpen { get; set; }
    }
}
