using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Data.Configurations
{
    public class RealtimeQuizGameConfiguration : IEntityTypeConfiguration<RealtimeQuizGame>
    {
        public void Configure(EntityTypeBuilder<RealtimeQuizGame> builder)
        {
            builder.HasOne("Quiz")
                .WithMany("RealtimeQuizGames")
                .HasForeignKey("RealtimeQuizGame_QuizId");
        }
    }

    public class RegularQuizGameConfiguration : IEntityTypeConfiguration<RegularQuizGame>
    {
        public void Configure(EntityTypeBuilder<RegularQuizGame> builder)
        {
            builder.HasOne("Quiz")
                .WithMany("RegularQuizGames")
                .HasForeignKey("RegularQuizGame_QuizId");
        }
    }
}
