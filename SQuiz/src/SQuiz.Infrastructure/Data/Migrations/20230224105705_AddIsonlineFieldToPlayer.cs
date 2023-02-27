using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class AddIsonlineFieldToPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "Players");
        }
    }
}
