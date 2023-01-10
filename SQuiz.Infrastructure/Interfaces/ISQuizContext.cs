using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Interfaces
{
    public interface ISQuizContext
    {
        DbSet<Quiz> Quizzes { get; set; }
        DbSet<Question> Questiones { get; set; }
        DbSet<Answer> Answers { get; set; }
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;
        EntityEntry<TEntity> Add<TEntity>(TEntity entity)
            where TEntity : class;
        EntityEntry<TEntity> Update<TEntity>(TEntity entity)
            where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
