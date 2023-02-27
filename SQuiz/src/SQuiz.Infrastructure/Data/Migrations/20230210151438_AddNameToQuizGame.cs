using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class AddNameToQuizGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "QuizGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "QuizGames");
        }
    }
}
