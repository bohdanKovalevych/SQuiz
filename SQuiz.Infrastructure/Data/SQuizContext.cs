﻿using Microsoft.EntityFrameworkCore;
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

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questiones { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}