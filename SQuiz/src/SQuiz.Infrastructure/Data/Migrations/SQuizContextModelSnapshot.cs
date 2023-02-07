﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SQuiz.Infrastructure.Data;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    [DbContext(typeof(SQuizContext))]
    partial class SQuizContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.HasSequence<int>("Moderators_shortId_seq");

            modelBuilder.HasSequence<int>("Players_shortId_seq");

            modelBuilder.HasSequence<int>("QuizGames_shortId_seq");

            modelBuilder.HasSequence<int>("QuizModerators_shortId_seq");

            modelBuilder.HasSequence<int>("Quizzes_shortId_seq");

            modelBuilder.Entity("SQuiz.Shared.Models.Answer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("QuestionId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Moderator", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset?>("DateCreated")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShortId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR Moderators_shortId_seq");

                    b.HasKey("Id");

                    b.HasIndex("ShortId");

                    b.ToTable("Moderators");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset?>("DateCreated")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("QuizGameId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.Property<int>("ShortId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR Players_shortId_seq");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("QuizGameId");

                    b.HasIndex("ShortId");

                    b.HasIndex("UserId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.PlayerAnswer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("AnswerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerAnswers");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Question", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(36)");

                    b.Property<int>("AnsweringTime")
                        .HasColumnType("int");

                    b.Property<string>("CorrectAnswerId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("QuizId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CorrectAnswerId")
                        .IsUnique()
                        .HasFilter("[CorrectAnswerId] IS NOT NULL");

                    b.HasIndex("QuizId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Quiz", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset?>("DateCreated")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPublic")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShortId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR Quizzes_shortId_seq");

                    b.HasKey("Id");

                    b.HasIndex("ShortId");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.QuizGame", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset?>("DateCreated")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateStart")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("QuizId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.Property<int>("ShortId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR QuizGames_shortId_seq");

                    b.Property<string>("StartedById")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.HasIndex("ShortId");

                    b.HasIndex("StartedById");

                    b.ToTable("QuizGames");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.QuizModerator", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset?>("DateCreated")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModeratorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("QuizId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.Property<int>("ShortId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR QuizModerators_shortId_seq");

                    b.HasKey("Id");

                    b.HasIndex("ModeratorId");

                    b.HasIndex("QuizId");

                    b.HasIndex("ShortId");

                    b.ToTable("QuizModerators");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Answer", b =>
                {
                    b.HasOne("SQuiz.Shared.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Player", b =>
                {
                    b.HasOne("SQuiz.Shared.Models.QuizGame", "QuizGame")
                        .WithMany("Players")
                        .HasForeignKey("QuizGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuizGame");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.PlayerAnswer", b =>
                {
                    b.HasOne("SQuiz.Shared.Models.Answer", "Answer")
                        .WithMany("PlayerAnswers")
                        .HasForeignKey("AnswerId");

                    b.HasOne("SQuiz.Shared.Models.Player", "Player")
                        .WithMany("PlayerAnswers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Question", b =>
                {
                    b.HasOne("SQuiz.Shared.Models.Answer", "CorrectAnswer")
                        .WithOne()
                        .HasForeignKey("SQuiz.Shared.Models.Question", "CorrectAnswerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SQuiz.Shared.Models.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CorrectAnswer");

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.QuizGame", b =>
                {
                    b.HasOne("SQuiz.Shared.Models.Quiz", "Quiz")
                        .WithMany("QuizGames")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SQuiz.Shared.Models.Moderator", "StartedBy")
                        .WithMany("QuizGames")
                        .HasForeignKey("StartedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");

                    b.Navigation("StartedBy");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.QuizModerator", b =>
                {
                    b.HasOne("SQuiz.Shared.Models.Moderator", "Moderator")
                        .WithMany("QuizModerators")
                        .HasForeignKey("ModeratorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SQuiz.Shared.Models.Quiz", "Quiz")
                        .WithMany("QuizModerators")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Moderator");

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Answer", b =>
                {
                    b.Navigation("PlayerAnswers");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Moderator", b =>
                {
                    b.Navigation("QuizGames");

                    b.Navigation("QuizModerators");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Player", b =>
                {
                    b.Navigation("PlayerAnswers");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Quiz", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("QuizGames");

                    b.Navigation("QuizModerators");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.QuizGame", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}