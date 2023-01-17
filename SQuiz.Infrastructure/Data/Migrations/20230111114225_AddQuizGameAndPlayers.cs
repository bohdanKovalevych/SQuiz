using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class AddQuizGameAndPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questiones_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questiones_Answers_CorrectAnswerId",
                table: "Questiones");

            migrationBuilder.DropForeignKey(
                name: "FK_Questiones_Quizzes_QuizId",
                table: "Questiones");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizModerator_Quizzes_QuizId",
                table: "QuizModerator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizModerator",
                table: "QuizModerator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questiones",
                table: "Questiones");

            migrationBuilder.DropSequence(
                name: "QuizModerator_shortId_seq");

            migrationBuilder.RenameTable(
                name: "QuizModerator",
                newName: "QuizModerators");

            migrationBuilder.RenameTable(
                name: "Questiones",
                newName: "Questions");

            migrationBuilder.RenameIndex(
                name: "IX_QuizModerator_QuizId",
                table: "QuizModerators",
                newName: "IX_QuizModerators_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Questiones_QuizId",
                table: "Questions",
                newName: "IX_Questions_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Questiones_CorrectAnswerId",
                table: "Questions",
                newName: "IX_Questions_CorrectAnswerId");

            migrationBuilder.CreateSequence<int>(
                name: "Moderators_shortId_seq");

            migrationBuilder.CreateSequence<int>(
                name: "Players_shortId_seq");

            migrationBuilder.CreateSequence<int>(
                name: "QuizGames_shortId_seq");

            migrationBuilder.CreateSequence<int>(
                name: "QuizModerators_shortId_seq");

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "QuizModerators",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR QuizModerators_shortId_seq",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ModeratorId",
                table: "QuizModerators",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizModerators",
                table: "QuizModerators",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Moderators",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR Moderators_shortId_seq"),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moderators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizGames",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    DateStart = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    StartedById = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    QuizId = table.Column<string>(type: "char(36)", nullable: false),
                    ShortId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR QuizGames_shortId_seq"),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizGames_Moderators_StartedById",
                        column: x => x.StartedById,
                        principalTable: "Moderators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizGames_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    QuizGameId = table.Column<string>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false),
                    ShortId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR Players_shortId_seq"),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_QuizGames_QuizGameId",
                        column: x => x.QuizGameId,
                        principalTable: "QuizGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizModerators_ModeratorId",
                table: "QuizModerators",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizModerators_ShortId",
                table: "QuizModerators",
                column: "ShortId");

            migrationBuilder.CreateIndex(
                name: "IX_Moderators_ShortId",
                table: "Moderators",
                column: "ShortId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_QuizGameId",
                table: "Players",
                column: "QuizGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ShortId",
                table: "Players",
                column: "ShortId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizGames_QuizId",
                table: "QuizGames",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizGames_ShortId",
                table: "QuizGames",
                column: "ShortId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizGames_StartedById",
                table: "QuizGames",
                column: "StartedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Answers_CorrectAnswerId",
                table: "Questions",
                column: "CorrectAnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizModerators_Moderators_ModeratorId",
                table: "QuizModerators",
                column: "ModeratorId",
                principalTable: "Moderators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizModerators_Quizzes_QuizId",
                table: "QuizModerators",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Answers_CorrectAnswerId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizModerators_Moderators_ModeratorId",
                table: "QuizModerators");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizModerators_Quizzes_QuizId",
                table: "QuizModerators");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "QuizGames");

            migrationBuilder.DropTable(
                name: "Moderators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizModerators",
                table: "QuizModerators");

            migrationBuilder.DropIndex(
                name: "IX_QuizModerators_ModeratorId",
                table: "QuizModerators");

            migrationBuilder.DropIndex(
                name: "IX_QuizModerators_ShortId",
                table: "QuizModerators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropSequence(
                name: "Moderators_shortId_seq");

            migrationBuilder.DropSequence(
                name: "Players_shortId_seq");

            migrationBuilder.DropSequence(
                name: "QuizGames_shortId_seq");

            migrationBuilder.DropSequence(
                name: "QuizModerators_shortId_seq");

            migrationBuilder.DropColumn(
                name: "ModeratorId",
                table: "QuizModerators");

            migrationBuilder.RenameTable(
                name: "QuizModerators",
                newName: "QuizModerator");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Questiones");

            migrationBuilder.RenameIndex(
                name: "IX_QuizModerators_QuizId",
                table: "QuizModerator",
                newName: "IX_QuizModerator_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuizId",
                table: "Questiones",
                newName: "IX_Questiones_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_CorrectAnswerId",
                table: "Questiones",
                newName: "IX_Questiones_CorrectAnswerId");

            migrationBuilder.CreateSequence<int>(
                name: "QuizModerator_shortId_seq");

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "QuizModerator",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR QuizModerators_shortId_seq");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizModerator",
                table: "QuizModerator",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questiones",
                table: "Questiones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questiones_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questiones_Answers_CorrectAnswerId",
                table: "Questiones",
                column: "CorrectAnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questiones_Quizzes_QuizId",
                table: "Questiones",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizModerator_Quizzes_QuizId",
                table: "QuizModerator",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
