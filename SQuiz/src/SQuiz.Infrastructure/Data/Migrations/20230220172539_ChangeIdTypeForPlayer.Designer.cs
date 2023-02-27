﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SQuiz.Infrastructure.Data;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    [DbContext(typeof(SQuizContext))]
    [Migration("20230220172539_ChangeIdTypeForPlayer")]
    partial class ChangeIdTypeForPlayer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                        .HasColumnType("nvarchar(200)");

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

                    b.Property<string>("RealtimeQuizGameId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RegularQuizGameId")
                        .HasColumnType("char(36)");

                    b.Property<int>("ShortId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR Players_shortId_seq");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("QuizGameId");

                    b.HasIndex("RealtimeQuizGameId");

                    b.HasIndex("RegularQuizGameId");

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

                    b.Property<string>("CorrectAnswerId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("CorrectAnswerId");

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

                    b.Property<DateTimeOffset?>("DateUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.HasDiscriminator<string>("Discriminator").HasValue("QuizGame");
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

            modelBuilder.Entity("SQuiz.Shared.Models.RealtimeQuizGame", b =>
                {
                    b.HasBaseType("SQuiz.Shared.Models.QuizGame");

                    b.Property<int>("CurrentQuestionIndex")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<string>("ModeratorId")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("QuizId1")
                        .HasColumnType("char(36)");

                    b.HasIndex("ModeratorId");

                    b.HasIndex("QuizId1");

                    b.HasDiscriminator().HasValue("RealtimeQuizGame");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.RegularQuizGame", b =>
                {
                    b.HasBaseType("SQuiz.Shared.Models.QuizGame");

                    b.Property<DateTimeOffset?>("DateEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateStart")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModeratorId")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("RegularQuizGame_ModeratorId");

                    b.Property<string>("QuizId1")
                        .HasColumnType("char(36)")
                        .HasColumnName("RegularQuizGame_QuizId1");

                    b.HasIndex("ModeratorId");

                    b.HasIndex("QuizId1");

                    b.HasDiscriminator().HasValue("RegularQuizGame");
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

                    b.HasOne("SQuiz.Shared.Models.RealtimeQuizGame", "RealtimeQuizGame")
                        .WithMany()
                        .HasForeignKey("RealtimeQuizGameId");

                    b.HasOne("SQuiz.Shared.Models.RegularQuizGame", "RegularQuizGame")
                        .WithMany()
                        .HasForeignKey("RegularQuizGameId");

                    b.Navigation("QuizGame");

                    b.Navigation("RealtimeQuizGame");

                    b.Navigation("RegularQuizGame");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.PlayerAnswer", b =>
                {
                    b.HasOne("SQuiz.Shared.Models.Answer", "Answer")
                        .WithMany("PlayerAnswers")
                        .HasForeignKey("AnswerId");

                    b.HasOne("SQuiz.Shared.Models.Answer", "CorrectAnswer")
                        .WithMany("CorrectPlayerAnswers")
                        .HasForeignKey("CorrectAnswerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("SQuiz.Shared.Models.Player", "Player")
                        .WithMany("PlayerAnswers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("CorrectAnswer");

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

            modelBuilder.Entity("SQuiz.Shared.Models.RealtimeQuizGame", b =>
                {
                    b.HasOne("SQuiz.Shared.Models.Moderator", null)
                        .WithMany("RealtimeQuizGames")
                        .HasForeignKey("ModeratorId");

                    b.HasOne("SQuiz.Shared.Models.Quiz", null)
                        .WithMany("RealtimeQuizGames")
                        .HasForeignKey("QuizId1");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.RegularQuizGame", b =>
                {
                    b.HasOne("SQuiz.Shared.Models.Moderator", null)
                        .WithMany("RegularQuizGames")
                        .HasForeignKey("ModeratorId");

                    b.HasOne("SQuiz.Shared.Models.Quiz", null)
                        .WithMany("RegularQuizGames")
                        .HasForeignKey("QuizId1");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Answer", b =>
                {
                    b.Navigation("CorrectPlayerAnswers");

                    b.Navigation("PlayerAnswers");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.Moderator", b =>
                {
                    b.Navigation("QuizGames");

                    b.Navigation("QuizModerators");

                    b.Navigation("RealtimeQuizGames");

                    b.Navigation("RegularQuizGames");
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

                    b.Navigation("RealtimeQuizGames");

                    b.Navigation("RegularQuizGames");
                });

            modelBuilder.Entity("SQuiz.Shared.Models.QuizGame", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
