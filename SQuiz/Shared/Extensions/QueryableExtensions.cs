using SQuiz.Shared.Models;

namespace SQuiz.Shared.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<Quiz> WithAuthor(this IQueryable<Quiz> quizzes, string authorId)
        {
            return quizzes.Where(x => x.AuthorId == authorId);
        }

        public static IQueryable<Quiz> WithModerator(this IQueryable<Quiz> quizzes, string moderatorId)
        {
            return quizzes.Where(x => x.QuizModerators.Any(x => x.Id == moderatorId));
        }
    }
}
