using Microsoft.EntityFrameworkCore;
using SQuiz.Infrastructure.Interfaces;
using SQuiz.Shared.Models.Interfaces;

namespace SQuiz.Infrastructure.Services
{
    public class ModelService : IModelService
    {
        public void AddContentItemShortIdSequences(ModelBuilder modelBuilder)
        {
            var contentItemTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic)
                .SelectMany(x => x.ExportedTypes)
                .Where(x => typeof(IResourceItem).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface);

            foreach (var t in contentItemTypes)
            {
                modelBuilder.HasSequence<int>($"{modelBuilder.Entity(t).Metadata.GetTableName()}_shortId_seq")
                    .StartsAt(1)
                    .IncrementsBy(1);
            }
        }
    }
}
