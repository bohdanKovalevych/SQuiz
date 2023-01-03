using Microsoft.EntityFrameworkCore;

namespace SQuiz.Infrastructure.Interfaces
{
    public interface IModelService
    {
        void AddContentItemShortIdSequences(ModelBuilder modelBuilder);
    }
}
