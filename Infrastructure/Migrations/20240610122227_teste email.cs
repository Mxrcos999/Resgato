using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class testeemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Professor_ProfessorId",
                table: "Game");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessorId",
                table: "Game",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmail",
                table: "Game",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Professor_ProfessorId",
                table: "Game",
                column: "ProfessorId",
                principalTable: "Professor",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Professor_ProfessorId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "ProfessorEmail",
                table: "Game");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessorId",
                table: "Game",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Professor_ProfessorId",
                table: "Game",
                column: "ProfessorId",
                principalTable: "Professor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
