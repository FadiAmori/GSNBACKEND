using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CRs_Produits_ProduitId",
                table: "CRs");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesCalculees_RapportsFinanciers_rapport_financier_id",
                table: "LignesCalculees");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesFinancieres_CRs_cr_id",
                table: "LignesFinancieres");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesFinancieres_SousCategoriesCR_sous_categorie_cr_id",
                table: "LignesFinancieres");

            migrationBuilder.DropForeignKey(
                name: "FK_Produits_FamillesProduit_famille_produit_id",
                table: "Produits");

            migrationBuilder.DropForeignKey(
                name: "FK_Produits_TypeClients_type_client_id",
                table: "Produits");

            migrationBuilder.AddForeignKey(
                name: "FK_CRs_Produits_ProduitId",
                table: "CRs",
                column: "ProduitId",
                principalTable: "Produits",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LignesCalculees_RapportsFinanciers_rapport_financier_id",
                table: "LignesCalculees",
                column: "rapport_financier_id",
                principalTable: "RapportsFinanciers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LignesFinancieres_CRs_cr_id",
                table: "LignesFinancieres",
                column: "cr_id",
                principalTable: "CRs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LignesFinancieres_SousCategoriesCR_sous_categorie_cr_id",
                table: "LignesFinancieres",
                column: "sous_categorie_cr_id",
                principalTable: "SousCategoriesCR",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produits_FamillesProduit_famille_produit_id",
                table: "Produits",
                column: "famille_produit_id",
                principalTable: "FamillesProduit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produits_TypeClients_type_client_id",
                table: "Produits",
                column: "type_client_id",
                principalTable: "TypeClients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CRs_Produits_ProduitId",
                table: "CRs");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesCalculees_RapportsFinanciers_rapport_financier_id",
                table: "LignesCalculees");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesFinancieres_CRs_cr_id",
                table: "LignesFinancieres");

            migrationBuilder.DropForeignKey(
                name: "FK_LignesFinancieres_SousCategoriesCR_sous_categorie_cr_id",
                table: "LignesFinancieres");

            migrationBuilder.DropForeignKey(
                name: "FK_Produits_FamillesProduit_famille_produit_id",
                table: "Produits");

            migrationBuilder.DropForeignKey(
                name: "FK_Produits_TypeClients_type_client_id",
                table: "Produits");

            migrationBuilder.AddForeignKey(
                name: "FK_CRs_Produits_ProduitId",
                table: "CRs",
                column: "ProduitId",
                principalTable: "Produits",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_LignesCalculees_RapportsFinanciers_rapport_financier_id",
                table: "LignesCalculees",
                column: "rapport_financier_id",
                principalTable: "RapportsFinanciers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_LignesFinancieres_CRs_cr_id",
                table: "LignesFinancieres",
                column: "cr_id",
                principalTable: "CRs",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_LignesFinancieres_SousCategoriesCR_sous_categorie_cr_id",
                table: "LignesFinancieres",
                column: "sous_categorie_cr_id",
                principalTable: "SousCategoriesCR",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produits_FamillesProduit_famille_produit_id",
                table: "Produits",
                column: "famille_produit_id",
                principalTable: "FamillesProduit",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produits_TypeClients_type_client_id",
                table: "Produits",
                column: "type_client_id",
                principalTable: "TypeClients",
                principalColumn: "id");
        }
    }
}
