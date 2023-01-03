using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasOne(x => x.Quiz)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.QuizId)
                .HasColumnType("char(36)");

            builder.Property(x => x.QuestionText)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.CorrectAnswerId)
                .HasColumnType("char(36)");

            builder.HasOne(x => x.CorrectAnswer)
                .WithOne()
                .HasForeignKey<Question>(x => x.CorrectAnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Id)
                .HasColumnType("char(36)");
        }
    }
}
