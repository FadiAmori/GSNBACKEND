using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRapportResultatToLigneCalculee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "rapport_financier_id",
                table: "LignesCalculees",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "resultat",
                table: "LignesCalculees",
                type: "double precision",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LignesCalculees_rapport_financier_id",
                table: "LignesCalculees",
                column: "rapport_financier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_LignesCalculees_RapportsFinanciers_rapport_financier_id",
                table: "LignesCalculees",
                column: "rapport_financier_id",
                principalTable: "RapportsFinanciers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LignesCalculees_RapportsFinanciers_rapport_financier_id",
                table: "LignesCalculees");

            migrationBuilder.DropIndex(
                name: "IX_LignesCalculees_rapport_financier_id",
                table: "LignesCalculees");

            migrationBuilder.DropColumn(
                name: "rapport_financier_id",
                table: "LignesCalculees");

            migrationBuilder.DropColumn(
                name: "resultat",
                table: "LignesCalculees");
        }
    }
}
