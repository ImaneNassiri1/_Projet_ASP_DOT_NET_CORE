using Microsoft.EntityFrameworkCore.Migrations;

namespace Projet_Exam_ASP.NetCore.Migrations
{
    public partial class M4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offres_Propriétaires_PropriétaireId",
                table: "Offres");

            migrationBuilder.DropForeignKey(
                name: "FK_Propriétaires_Images_ImageId",
                table: "Propriétaires");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Propriétaires",
                table: "Propriétaires");

            migrationBuilder.RenameTable(
                name: "Propriétaires",
                newName: "Propriétaire");

            migrationBuilder.RenameIndex(
                name: "IX_Propriétaires_ImageId",
                table: "Propriétaire",
                newName: "IX_Propriétaire_ImageId");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Propriétaire",
                table: "Propriétaire",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offres_Propriétaire_PropriétaireId",
                table: "Offres",
                column: "PropriétaireId",
                principalTable: "Propriétaire",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Propriétaire_Images_ImageId",
                table: "Propriétaire",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offres_Propriétaire_PropriétaireId",
                table: "Offres");

            migrationBuilder.DropForeignKey(
                name: "FK_Propriétaire_Images_ImageId",
                table: "Propriétaire");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Propriétaire",
                table: "Propriétaire");

            migrationBuilder.RenameTable(
                name: "Propriétaire",
                newName: "Propriétaires");

            migrationBuilder.RenameIndex(
                name: "IX_Propriétaire_ImageId",
                table: "Propriétaires",
                newName: "IX_Propriétaires_ImageId");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Propriétaires",
                table: "Propriétaires",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offres_Propriétaires_PropriétaireId",
                table: "Offres",
                column: "PropriétaireId",
                principalTable: "Propriétaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Propriétaires_Images_ImageId",
                table: "Propriétaires",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
