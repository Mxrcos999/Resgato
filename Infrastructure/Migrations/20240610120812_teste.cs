using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class teste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Professor_ProfessorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Round_RoundId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Round_AspNetUsers_ProfessorId",
                table: "Round");

            migrationBuilder.DropIndex(
                name: "IX_Round_ProfessorId",
                table: "Round");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfessorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoundId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Round");

            migrationBuilder.DropColumn(
                name: "RoundId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Professor",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Professor_ApplicationUserId",
                table: "Professor",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Professor_AspNetUsers_ApplicationUserId",
                table: "Professor",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professor_AspNetUsers_ApplicationUserId",
                table: "Professor");

            migrationBuilder.DropIndex(
                name: "IX_Professor_ApplicationUserId",
                table: "Professor");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Professor");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorId",
                table: "Round",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoundId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Round_ProfessorId",
                table: "Round",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfessorId",
                table: "AspNetUsers",
                column: "ProfessorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoundId",
                table: "AspNetUsers",
                column: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Professor_ProfessorId",
                table: "AspNetUsers",
                column: "ProfessorId",
                principalTable: "Professor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Round_RoundId",
                table: "AspNetUsers",
                column: "RoundId",
                principalTable: "Round",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Round_AspNetUsers_ProfessorId",
                table: "Round",
                column: "ProfessorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
