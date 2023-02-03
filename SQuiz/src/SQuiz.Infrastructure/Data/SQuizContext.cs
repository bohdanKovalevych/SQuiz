using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Infrastructure.Interfaces;
using SQuiz.Shared.Models;

namespace SQuiz.Infrastructure.Data
{
    public class SQuizContext : DbContext, ISQuizContext
    {
        private readonly IModelService _modelService;

        public SQuizContext(IModelService modelService, DbContextOptions<SQuizContext> options) : base(options)
        {
            _modelService = modelService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            _modelService.AddContentItemShortIdSequences(modelBuilder);
        }

        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Moderator> Moderators { get; set; }
        public virtual DbSet<QuizModerator> QuizModerators { get; set; }
        public virtual DbSet<QuizGame> QuizGames { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlayerAnswer> PlayerAnswers { get; set; }

        public virtual void SetState<TEntity>(TEntity entity, EntityState state)
            where TEntity : class
        {
            Entry(entity).State = state;
        }
    }
}