using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CatsQuantity",
                table: "Settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Settings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_ApplicationUserId",
                table: "Settings",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_AspNetUsers_ApplicationUserId",
                table: "Settings",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_AspNetUsers_ApplicationUserId",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Settings_ApplicationUserId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Settings");

            migrationBuilder.AlterColumn<string>(
                name: "CatsQuantity",
                table: "Settings",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
