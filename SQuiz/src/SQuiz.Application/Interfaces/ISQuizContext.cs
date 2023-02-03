using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SQuiz.Shared.Models;

namespace SQuiz.Application.Interfaces
{
    public interface ISQuizContext
    {
        DbSet<Quiz> Quizzes { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<Moderator> Moderators { get; set; }
        DbSet<QuizModerator> QuizModerators { get; set; }
        DbSet<QuizGame> QuizGames { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<PlayerAnswer> PlayerAnswers { get; set; }

        void SetState<TEntity>(TEntity entity, EntityState state)
            where TEntity : class;
        EntityEntry<TEntity> Add<TEntity>(TEntity entity)
            where TEntity : class;
        EntityEntry<TEntity> Update<TEntity>(TEntity entity)
            where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
