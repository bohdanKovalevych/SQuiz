using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Data.Configurations
{
    public class PlayerAnswerConfiguration : IEntityTypeConfiguration<PlayerAnswer>
    {
        public void Configure(EntityTypeBuilder<PlayerAnswer> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnType("char(36)");

            builder.HasOne(x => x.Answer)
                .WithMany(x => x.PlayerAnswers)
                .HasForeignKey(x => x.AnswerId);

            builder.HasOne(x => x.CorrectAnswer)
                .WithMany(x => x.CorrectPlayerAnswers)
                .HasForeignKey(x => x.CorrectAnswerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.AnswerId)
                .IsRequired(false);

            builder.HasOne(x => x.Player)
                .WithMany(x => x.PlayerAnswers)
                .HasForeignKey(x => x.PlayerId);
        }
    }
}
