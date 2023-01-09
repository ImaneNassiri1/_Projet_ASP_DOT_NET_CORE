using Microsoft.EntityFrameworkCore.Migrations;

namespace Projet_Exam_ASP.NetCore.Migrations
{
    public partial class M5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offres_Propriétaire_PropriétaireId",
                table: "Offres");

            migrationBuilder.DropTable(
                name: "Propriétaire");

            migrationBuilder.DropColumn(
                name: "Etat",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PropriétaireId",
                table: "Offres",
                newName: "BoutiqueId");

            migrationBuilder.RenameIndex(
                name: "IX_Offres_PropriétaireId",
                table: "Offres",
                newName: "IX_Offres_BoutiqueId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "AspNetUsers",
                newName: "EnListeNoire");

            migrationBuilder.AddColumn<bool>(
                name: "Valide",
                table: "Offres",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BoutiqueId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Boutiques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Ville = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    site = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boutiques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boutiques_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BoutiqueId",
                table: "AspNetUsers",
                column: "BoutiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Boutiques_ImageId",
                table: "Boutiques",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Boutiques_BoutiqueId",
                table: "AspNetUsers",
                column: "BoutiqueId",
                principalTable: "Boutiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offres_Boutiques_BoutiqueId",
                table: "Offres",
                column: "BoutiqueId",
                principalTable: "Boutiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Boutiques_BoutiqueId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offres_Boutiques_BoutiqueId",
                table: "Offres");

            migrationBuilder.DropTable(
                name: "Boutiques");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BoutiqueId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Valide",
                table: "Offres");

            migrationBuilder.DropColumn(
                name: "BoutiqueId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "BoutiqueId",
                table: "Offres",
                newName: "PropriétaireId");

            migrationBuilder.RenameIndex(
                name: "IX_Offres_BoutiqueId",
                table: "Offres",
                newName: "IX_Offres_PropriétaireId");

            migrationBuilder.RenameColumn(
                name: "EnListeNoire",
                table: "AspNetUsers",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Etat",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Propriétaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    E_mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Etat = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    Mot_de_passe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ville = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propriétaire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Propriétaire_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Propriétaire_ImageId",
                table: "Propriétaire",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offres_Propriétaire_PropriétaireId",
                table: "Offres",
                column: "PropriétaireId",
                principalTable: "Propriétaire",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
