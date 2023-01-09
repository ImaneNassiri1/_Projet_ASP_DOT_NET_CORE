using Microsoft.EntityFrameworkCore.Migrations;

namespace Projet_Exam_ASP.NetCore.Migrations
{
    public partial class M6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offres_Boutiques_BoutiqueId",
                table: "Offres");

            migrationBuilder.DropIndex(
                name: "IX_Offres_BoutiqueId",
                table: "Offres");

            migrationBuilder.DropColumn(
                name: "BoutiqueId",
                table: "Offres");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoutiqueId",
                table: "Offres",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Offres_BoutiqueId",
                table: "Offres",
                column: "BoutiqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offres_Boutiques_BoutiqueId",
                table: "Offres",
                column: "BoutiqueId",
                principalTable: "Boutiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
