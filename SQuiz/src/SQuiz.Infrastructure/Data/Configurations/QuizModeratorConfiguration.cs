using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Data.Configurations
{
    public class QuizModeratorConfiguration : IEntityTypeConfiguration<QuizModerator>
    {
        public void Configure(EntityTypeBuilder<QuizModerator> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnType("char(36)");

            builder.Property(x => x.QuizId)
                .HasColumnType("char(36)");

            builder.HasOne(x => x.Quiz)
                .WithMany(x => x.QuizModerators)
                .HasForeignKey(x => x.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Moderator)
                .WithMany(x => x.QuizModerators)
                .HasForeignKey(x => x.ModeratorId);
            
            builder.Property(x => x.ShortId)
              .HasDefaultValueSql($"NEXT VALUE FOR {builder.Metadata.GetTableName()}_shortId_seq");

            builder.HasIndex(x => x.ShortId);

            builder.Property(x => x.ModeratorId)
                .HasColumnType("nvarchar(200)");
        }
    }
}
