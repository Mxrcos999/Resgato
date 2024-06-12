using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class cat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_AspNetUsers_ApplicationUserId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "CatsQuantity",
                table: "Settings",
                newName: "GameId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BudgetGame",
                table: "Settings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SettingId",
                table: "Game",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SettingCat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CatsQuantity = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    SettingsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingCat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingCat_Settings_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "Settings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_GameId",
                table: "Settings",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SettingCat_SettingsId",
                table: "SettingCat",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_AspNetUsers_ApplicationUserId",
                table: "Settings",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Game_GameId",
                table: "Settings",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_AspNetUsers_ApplicationUserId",
                table: "Settings");

            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Game_GameId",
                table: "Settings");

            migrationBuilder.DropTable(
                name: "SettingCat");

            migrationBuilder.DropIndex(
                name: "IX_Settings_GameId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "BudgetGame",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SettingId",
                table: "Game");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Settings",
                newName: "CatsQuantity");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Settings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_AspNetUsers_ApplicationUserId",
                table: "Settings",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
