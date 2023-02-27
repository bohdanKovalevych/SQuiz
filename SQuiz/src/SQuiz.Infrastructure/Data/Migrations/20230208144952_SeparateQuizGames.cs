using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class SeparateQuizGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentQuestionIndex",
                table: "QuizGames",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "QuizGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "QuizGames",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratorId",
                table: "QuizGames",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizId1",
                table: "QuizGames",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegularQuizGame_ModeratorId",
                table: "QuizGames",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegularQuizGame_QuizId1",
                table: "QuizGames",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealtimeQuizGameId",
                table: "Players",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegularQuizGameId",
                table: "Players",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizGames_ModeratorId",
                table: "QuizGames",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizGames_QuizId1",
                table: "QuizGames",
                column: "QuizId1");

            migrationBuilder.CreateIndex(
                name: "IX_QuizGames_RegularQuizGame_ModeratorId",
                table: "QuizGames",
                column: "RegularQuizGame_ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizGames_RegularQuizGame_QuizId1",
                table: "QuizGames",
                column: "RegularQuizGame_QuizId1");

            migrationBuilder.CreateIndex(
                name: "IX_Players_RealtimeQuizGameId",
                table: "Players",
                column: "RealtimeQuizGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_RegularQuizGameId",
                table: "Players",
                column: "RegularQuizGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_QuizGames_RealtimeQuizGameId",
                table: "Players",
                column: "RealtimeQuizGameId",
                principalTable: "QuizGames",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_QuizGames_RegularQuizGameId",
                table: "Players",
                column: "RegularQuizGameId",
                principalTable: "QuizGames",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGames_Moderators_ModeratorId",
                table: "QuizGames",
                column: "ModeratorId",
                principalTable: "Moderators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGames_Moderators_RegularQuizGame_ModeratorId",
                table: "QuizGames",
                column: "RegularQuizGame_ModeratorId",
                principalTable: "Moderators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGames_Quizzes_QuizId1",
                table: "QuizGames",
                column: "QuizId1",
                principalTable: "Quizzes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGames_Quizzes_RegularQuizGame_QuizId1",
                table: "QuizGames",
                column: "RegularQuizGame_QuizId1",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_QuizGames_RealtimeQuizGameId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_QuizGames_RegularQuizGameId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizGames_Moderators_ModeratorId",
                table: "QuizGames");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizGames_Moderators_RegularQuizGame_ModeratorId",
                table: "QuizGames");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizGames_Quizzes_QuizId1",
                table: "QuizGames");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizGames_Quizzes_RegularQuizGame_QuizId1",
                table: "QuizGames");

            migrationBuilder.DropIndex(
                name: "IX_QuizGames_ModeratorId",
                table: "QuizGames");

            migrationBuilder.DropIndex(
                name: "IX_QuizGames_QuizId1",
                table: "QuizGames");

            migrationBuilder.DropIndex(
                name: "IX_QuizGames_RegularQuizGame_ModeratorId",
                table: "QuizGames");

            migrationBuilder.DropIndex(
                name: "IX_QuizGames_RegularQuizGame_QuizId1",
                table: "QuizGames");

            migrationBuilder.DropIndex(
                name: "IX_Players_RealtimeQuizGameId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_RegularQuizGameId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentQuestionIndex",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "ModeratorId",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "QuizId1",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "RegularQuizGame_ModeratorId",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "RegularQuizGame_QuizId1",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "RealtimeQuizGameId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "RegularQuizGameId",
                table: "Players");
        }
    }
}
