using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateEntit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoundId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Round",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProfessorId = table.Column<string>(type: "text", nullable: true),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Round_AspNetUsers_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoundId",
                table: "AspNetUsers",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_ProfessorId",
                table: "Round",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Round_RoundId",
                table: "AspNetUsers",
                column: "RoundId",
                principalTable: "Round",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Round_RoundId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Round");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoundId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoundId",
                table: "AspNetUsers");
        }
    }
}
