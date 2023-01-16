using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Data.Configurations
{
    public class ModeratorConfiguration : IEntityTypeConfiguration<Moderator>
    {
        public void Configure(EntityTypeBuilder<Moderator> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnType("nvarchar(200)");

            builder.Property(x => x.ShortId)
              .HasDefaultValueSql($"NEXT VALUE FOR {builder.Metadata.GetTableName()}_shortId_seq");

            builder.HasIndex(x => x.ShortId);
        }
    }
}
