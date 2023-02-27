using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class ChangeIdTypeForPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_PlayerAnswers_Players_PlayerId",
               table: "PlayerAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");
            
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Players",
                type: "nvarchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "PlayerAnswers",
                type: "nvarchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAnswers_Players_PlayerId",
                table: "PlayerAnswers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerAnswers_Players_PlayerId",
                table: "PlayerAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Players",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "PlayerAnswers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");

            migrationBuilder.AddPrimaryKey(
               name: "PK_Players",
               table: "Players",
               column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAnswers_Players_PlayerId",
                table: "PlayerAnswers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
