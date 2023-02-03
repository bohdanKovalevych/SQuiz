using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class RemoveAnswersFromQuiz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Quizzes_QuizId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Questiones_CorrectAnswerId",
                table: "Questiones");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuizId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Answers");

            migrationBuilder.AlterColumn<string>(
                name: "CorrectAnswerId",
                table: "Questiones",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)");

            migrationBuilder.CreateIndex(
                name: "IX_Questiones_CorrectAnswerId",
                table: "Questiones",
                column: "CorrectAnswerId",
                unique: true,
                filter: "[CorrectAnswerId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Questiones_CorrectAnswerId",
                table: "Questiones");

            migrationBuilder.AlterColumn<string>(
                name: "CorrectAnswerId",
                table: "Questiones",
                type: "char(36)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizId",
                table: "Answers",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questiones_CorrectAnswerId",
                table: "Questiones",
                column: "CorrectAnswerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuizId",
                table: "Answers",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Quizzes_QuizId",
                table: "Answers",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }
    }
}
