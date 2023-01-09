using Microsoft.EntityFrameworkCore.Migrations;

namespace Projet_Exam_ASP.NetCore.Migrations
{
    public partial class M9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reduction",
                table: "Boutiques");

            migrationBuilder.AddColumn<int>(
                name: "reduction",
                table: "Offres",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reduction",
                table: "Offres");

            migrationBuilder.AddColumn<int>(
                name: "reduction",
                table: "Boutiques",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
