using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class AddOrderForAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswerId",
                table: "PlayerAnswers",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "PlayerAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAnswers_CorrectAnswerId",
                table: "PlayerAnswers",
                column: "CorrectAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAnswers_Answers_CorrectAnswerId",
                table: "PlayerAnswers",
                column: "CorrectAnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerAnswers_Answers_CorrectAnswerId",
                table: "PlayerAnswers");

            migrationBuilder.DropIndex(
                name: "IX_PlayerAnswers_CorrectAnswerId",
                table: "PlayerAnswers");

            migrationBuilder.DropColumn(
                name: "CorrectAnswerId",
                table: "PlayerAnswers");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "PlayerAnswers");
        }
    }
}
