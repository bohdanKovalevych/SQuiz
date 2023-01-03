using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Data.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnType("char(36)");

            builder.Property(x => x.ShortId)
              .HasDefaultValueSql($"NEXT VALUE FOR {builder.Metadata.GetTableName()}_shortId_seq");

            builder.Property(x => x.IsPublic)
                .HasDefaultValue(false);

            builder.HasIndex(x => x.ShortId);

            builder.Property(x => x.AuthorId)
                .HasColumnType("nvarchar(200)");
        }
    }
}
