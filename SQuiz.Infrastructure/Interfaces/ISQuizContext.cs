using Microsoft.EntityFrameworkCore;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Interfaces
{
    public interface ISQuizContext
    {
        DbSet<Quiz> Quizzes { get; set; }
        DbSet<Question> Questiones { get; set; }
        DbSet<Answer> Answers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
