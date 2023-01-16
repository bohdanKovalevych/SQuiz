using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Client.Interfaces
{
    public interface IPlayGameService
    {
        event Func<Task> OnTimeEnd;
        event Func<SendAnswerDto, Task> OnAnswered;
        event Action<double> OnTimeChanged;
        double CurrentMaxTime { get; }
        void InitQuestionAndStartTimer(GameQuestionDto questionDto);

        Task SendAnswer(string AnswerId);
    }
}
