using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class AddRegularAndRealtimeQuizGameId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizGames_Quizzes_QuizId1",
                table: "QuizGames");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizGames_Quizzes_RegularQuizGame_QuizId1",
                table: "QuizGames");

            migrationBuilder.RenameColumn(
                name: "RegularQuizGame_QuizId1",
                table: "QuizGames",
                newName: "RegularQuizGame_QuizId");

            migrationBuilder.RenameColumn(
                name: "QuizId1",
                table: "QuizGames",
                newName: "RealtimeQuizGame_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizGames_RegularQuizGame_QuizId1",
                table: "QuizGames",
                newName: "IX_QuizGames_RegularQuizGame_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizGames_QuizId1",
                table: "QuizGames",
                newName: "IX_QuizGames_RealtimeQuizGame_QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGames_Quizzes_RealtimeQuizGame_QuizId",
                table: "QuizGames",
                column: "RealtimeQuizGame_QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGames_Quizzes_RegularQuizGame_QuizId",
                table: "QuizGames",
                column: "RegularQuizGame_QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizGames_Quizzes_RealtimeQuizGame_QuizId",
                table: "QuizGames");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizGames_Quizzes_RegularQuizGame_QuizId",
                table: "QuizGames");

            migrationBuilder.RenameColumn(
                name: "RegularQuizGame_QuizId",
                table: "QuizGames",
                newName: "RegularQuizGame_QuizId1");

            migrationBuilder.RenameColumn(
                name: "RealtimeQuizGame_QuizId",
                table: "QuizGames",
                newName: "QuizId1");

            migrationBuilder.RenameIndex(
                name: "IX_QuizGames_RegularQuizGame_QuizId",
                table: "QuizGames",
                newName: "IX_QuizGames_RegularQuizGame_QuizId1");

            migrationBuilder.RenameIndex(
                name: "IX_QuizGames_RealtimeQuizGame_QuizId",
                table: "QuizGames",
                newName: "IX_QuizGames_QuizId1");

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
    }
}
