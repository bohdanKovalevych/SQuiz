using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "QuizModerator_shortId_seq");

            migrationBuilder.CreateSequence<int>(
                name: "Quizzes_shortId_seq");

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<string>(type: "char(36)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ShortId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR Quizzes_shortId_seq"),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizModerator",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    QuizId = table.Column<string>(type: "char(36)", nullable: false),
                    ShortId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizModerator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizModerator_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<string>(type: "char(36)", nullable: false),
                    QuizId = table.Column<string>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questiones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CorrectAnswerId = table.Column<string>(type: "char(36)", nullable: false),
                    QuizId = table.Column<string>(type: "char(36)", nullable: false),
                    AnsweringTime = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questiones_Answers_CorrectAnswerId",
                        column: x => x.CorrectAnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questiones_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuizId",
                table: "Answers",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Questiones_CorrectAnswerId",
                table: "Questiones",
                column: "CorrectAnswerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questiones_QuizId",
                table: "Questiones",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizModerator_QuizId",
                table: "QuizModerator",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ShortId",
                table: "Quizzes",
                column: "ShortId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questiones_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questiones_QuestionId",
                table: "Answers");

            migrationBuilder.DropTable(
                name: "QuizModerator");

            migrationBuilder.DropTable(
                name: "Questiones");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropSequence(
                name: "QuizModerator_shortId_seq");

            migrationBuilder.DropSequence(
                name: "Quizzes_shortId_seq");
        }
    }
}
