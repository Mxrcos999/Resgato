using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class l : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SettingCat_Settings_SettingsId",
                table: "SettingCat");

            migrationBuilder.AlterColumn<int>(
                name: "SettingsId",
                table: "SettingCat",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SettingCat_Settings_SettingsId",
                table: "SettingCat",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SettingCat_Settings_SettingsId",
                table: "SettingCat");

            migrationBuilder.AlterColumn<int>(
                name: "SettingsId",
                table: "SettingCat",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_SettingCat_Settings_SettingsId",
                table: "SettingCat",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id");
        }
    }
}
