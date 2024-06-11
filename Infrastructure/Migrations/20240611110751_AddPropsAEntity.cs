using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddPropsAEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalPopulation",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPopulationCastrated",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPopulationFemaleCastrated",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPopulationMaleCastrated",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPopulation",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "TotalPopulationCastrated",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "TotalPopulationFemaleCastrated",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "TotalPopulationMaleCastrated",
                table: "Answers");
        }
    }
}
