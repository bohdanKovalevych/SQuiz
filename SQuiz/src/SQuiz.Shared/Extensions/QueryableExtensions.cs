using SQuiz.Shared.Models;
using System.Linq.Expressions;

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

        public static IQueryable<Moderator> WithNameOrEmail(this IQueryable<Moderator> moderators, string searchQuery)
        {
            Expression<Func<Moderator, bool>> func = string.IsNullOrEmpty(searchQuery)
                ? _ => true
                : x => x.Name.Contains(searchQuery) || x.Email.Contains(searchQuery);

            return moderators.Where(func);
        }
    }
}
