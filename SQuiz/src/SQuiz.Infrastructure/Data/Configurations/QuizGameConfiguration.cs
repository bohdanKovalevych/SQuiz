using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Data.Configurations
{
    public class QuizGameConfiguration : IEntityTypeConfiguration<QuizGame>
    {
        public void Configure(EntityTypeBuilder<QuizGame> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnType("char(36)");

            builder.Property(x => x.ShortId)
              .HasDefaultValueSql($"NEXT VALUE FOR {builder.Metadata.GetTableName()}_shortId_seq");

            builder.HasIndex(x => x.ShortId);
            builder.HasIndex(x => x.StartedById);

            builder.Property(x => x.StartedById)
                .HasColumnType("nvarchar(200)");

            builder.HasOne(x => x.StartedBy)
                .WithMany(x => x.QuizGames)
                .HasForeignKey(x => x.StartedById);
            
            builder.HasOne(x => x.Quiz)
                .WithMany(x => x.QuizGames)
                .HasForeignKey(x => x.QuizId);
        }
    }
}
