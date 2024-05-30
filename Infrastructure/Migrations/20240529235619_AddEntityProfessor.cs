using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddEntityProfessor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Round",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Professor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProfessorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_Professor_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Professor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Round_GameId",
                table: "Round",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GameId",
                table: "AspNetUsers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfessorId",
                table: "AspNetUsers",
                column: "ProfessorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_ProfessorId",
                table: "Game",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Game_GameId",
                table: "AspNetUsers",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Professor_ProfessorId",
                table: "AspNetUsers",
                column: "ProfessorId",
                principalTable: "Professor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Round_Game_GameId",
                table: "Round",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Game_GameId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Professor_ProfessorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Round_Game_GameId",
                table: "Round");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Professor");

            migrationBuilder.DropIndex(
                name: "IX_Round_GameId",
                table: "Round");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GameId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfessorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Round");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "AspNetUsers");
        }
    }
}
