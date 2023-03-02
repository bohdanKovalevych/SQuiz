using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Data.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnType("char(36)");

            builder.Property(x => x.ShortId)
              .HasDefaultValueSql($"NEXT VALUE FOR {builder.Metadata.GetTableName()}_shortId_seq");

            builder.HasIndex(x => x.ShortId);
            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.UserId)
                .HasColumnType("nvarchar(200)");

            builder.HasOne(x => x.QuizGame)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.QuizGameId);

            builder.HasOne(x => x.RealtimeQuizGame)
                .WithMany()
                .HasForeignKey(x => x.RealtimeQuizGameId)
                .IsRequired(false);

            builder.HasOne(x => x.RegularQuizGame)
                .WithMany()
                .HasForeignKey(x => x.RegularQuizGameId)
                .IsRequired(false);
        }
    }
}
