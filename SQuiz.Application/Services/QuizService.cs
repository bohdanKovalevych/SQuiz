using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Extensions;
using SQuiz.Shared.Models;
using SQuiz.Shared.Models.Interfaces;

namespace SQuiz.Application.Services
{
    public class QuizService : IQuizService
    {
        public void AssignIdsToQuestionsAndAnswers(Quiz quiz, Action<IEntity> update, Action<IEntity> add)
        {
            foreach (var question in quiz.Questions)
            {
                question.QuizId = quiz.Id;

                if (question.Id == null)
                {
                    question.Id = Guid.NewGuid().ToString();
                    add(question);
                }
                else
                {
                    update(question);
                }

                foreach (var answer in question.Answers)
                {
                    answer.QuestionId = question.Id;

                    if (answer.Id == null)
                    {
                        answer.Id = Guid.NewGuid().ToString();
                        add(answer);
                    }
                    else
                    {
                        update(answer);
                    }
                }
            }
        }

        public void SetCorrectAnswersFromModel(Quiz quiz, List<QuestionDto> modelQuestions)
        {
            foreach (var (question, questionIndex) in quiz.Questions.WithIndex())
            {
                var questionModel = modelQuestions[questionIndex];
                int answerIndex = questionModel.CorrectAnswerIndex;

                question.CorrectAnswerId = question.Answers
                    .Where(x => x.Order == answerIndex)
                    .FirstOrDefault()?.Id;
            }
        }

        public void SetCorrectAnswersFromEntity(Quiz quiz, List<QuestionDto> modelQuestions)
        {
            foreach (var (question, questionIndex) in quiz.Questions.WithIndex())
            {
                var questionModel = modelQuestions[questionIndex];
                int? index = question.Answers
                    .FirstOrDefault(x => x.Id == question.CorrectAnswerId)
                    ?.Order;

                if (index.HasValue)
                {
                    questionModel.CorrectAnswerIndex = index.Value;
                }
            }
        }

        public void ReorderQuestions(Quiz quiz)
        {
            quiz.Questions = quiz.Questions.OrderBy(x => x.Order).ToList();
        }

        public void ReorderAnswers(Quiz quiz)
        {
            foreach (var question in quiz.Questions)
            {
                question.Answers = question.Answers.OrderBy(x => x.Order).ToList();
            }

        }

        public IEnumerable<Question> GetQuestionsToRemove(Quiz quiz, EditQuizDto model)
        {
            var existingQuestions = model.Questions
                .Where(x => x.Id != null)
                .Select(x => x.Id)
                .ToHashSet();

            var result = quiz.Questions
                .Where(x => !existingQuestions.Contains(x.Id));

            foreach (var question in result)
            {
                question.CorrectAnswerId = null;
            }

            return result;
        }

        public IEnumerable<Answer> GetAnswersToRemove(Quiz quiz, EditQuizDto model)
        {
            var exisitingAnswers = model.Questions
                .Where(x => x.Id != null)
                .SelectMany(x => x.Answers)
                .Where(x => x.Id != null)
                .Select(x => x.Id)
                .ToHashSet();

            return quiz.Questions
                .SelectMany(x => x.Answers)
                .Where(x => !exisitingAnswers.Contains(x.Id));
        }
    }
}
