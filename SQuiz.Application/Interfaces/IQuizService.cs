using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Models;
using SQuiz.Shared.Models.Interfaces;

namespace SQuiz.Application.Interfaces
{
    public interface IQuizService
    {
        void AssignIdsToQuestionsAndAnswers(Quiz quiz, Action<IEntity> update, Action<IEntity> add);
        void AssignOrderToQuestions(Quiz quiz);
        void AssignOrderToAnswers(Quiz quiz);
        void SetCorrectAnswersFromModel(Quiz quiz, List<QuestionDto> modelQuestions);
        void SetCorrectAnswersFromEntity(Quiz quiz, List<QuestionDto> modelQuestions);
        void ReorderQuestions(Quiz quiz);
        void ReorderAnswers(Quiz quiz);
        IEnumerable<Question> GetQuestionsToRemove(Quiz quiz, EditQuizDto model);
        IEnumerable<Answer> GetAnswersToRemove(Quiz quiz, EditQuizDto model);
    }
}
