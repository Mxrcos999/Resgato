using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class t : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Game_GameId",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Settings_GameId",
                table: "Settings");

            migrationBuilder.CreateIndex(
                name: "IX_Game_SettingId",
                table: "Game",
                column: "SettingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Settings_SettingId",
                table: "Game",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Settings_SettingId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_SettingId",
                table: "Game");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_GameId",
                table: "Settings",
                column: "GameId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Game_GameId",
                table: "Settings",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
