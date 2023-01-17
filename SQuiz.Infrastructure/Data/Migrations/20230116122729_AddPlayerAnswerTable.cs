using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQuiz.Infrastructure.Data.Migrations
{
    public partial class AddPlayerAnswerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerAnswers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    PlayerId = table.Column<string>(type: "char(36)", nullable: false),
                    AnswerId = table.Column<string>(type: "char(36)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerAnswers_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerAnswers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAnswers_AnswerId",
                table: "PlayerAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAnswers_PlayerId",
                table: "PlayerAnswers",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerAnswers");
        }
    }
}
