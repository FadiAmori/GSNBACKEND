using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: true),
                    nom = table.Column<string>(type: "text", nullable: true),
                    prenom = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    adresse = table.Column<string>(type: "text", nullable: true),
                    identifiant = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FamillesProduit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamillesProduit", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Societes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    adresse = table.Column<string>(type: "text", nullable: true),
                    ville = table.Column<string>(type: "text", nullable: true),
                    pays = table.Column<string>(type: "text", nullable: true),
                    telephone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    date_affectation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Societes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TypeClients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeClients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ClesDeRepartitions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mois = table.Column<string>(type: "text", nullable: true),
                    saisonalite_ca = table.Column<double>(type: "double precision", nullable: false),
                    saisonalite_poids = table.Column<double>(type: "double precision", nullable: false),
                    cles_cout_fixes = table.Column<double>(type: "double precision", nullable: false),
                    societe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClesDeRepartitions", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClesDeRepartitions_Societes_societe_id",
                        column: x => x.societe_id,
                        principalTable: "Societes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RapportsFinanciers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    annee = table.Column<int>(type: "integer", nullable: false),
                    societe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportsFinanciers", x => x.id);
                    table.ForeignKey(
                        name: "FK_RapportsFinanciers_Societes_societe_id",
                        column: x => x.societe_id,
                        principalTable: "Societes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSocietes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    prenom = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    telephone = table.Column<string>(type: "text", nullable: true),
                    adresse = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    date_affectation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    societe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSocietes", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserSocietes_Societes_societe_id",
                        column: x => x.societe_id,
                        principalTable: "Societes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produits",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    poids_prevu = table.Column<double>(type: "double precision", nullable: false),
                    taux_poids = table.Column<double>(type: "double precision", nullable: false),
                    tps_unitaire = table.Column<double>(type: "double precision", nullable: false),
                    temps_global = table.Column<double>(type: "double precision", nullable: false),
                    cout_mod_par_heure = table.Column<double>(type: "double precision", nullable: false),
                    type_client_id = table.Column<int>(type: "integer", nullable: true),
                    famille_produit_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produits", x => x.id);
                    table.ForeignKey(
                        name: "FK_Produits_FamillesProduit_famille_produit_id",
                        column: x => x.famille_produit_id,
                        principalTable: "FamillesProduit",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Produits_TypeClients_type_client_id",
                        column: x => x.type_client_id,
                        principalTable: "TypeClients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CategoriesFinancieres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    rapport_financier_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesFinancieres", x => x.id);
                    table.ForeignKey(
                        name: "FK_CategoriesFinancieres_RapportsFinanciers_rapport_financier_~",
                        column: x => x.rapport_financier_id,
                        principalTable: "RapportsFinanciers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CRs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    montant_consomme = table.Column<double>(type: "double precision", nullable: false),
                    annee = table.Column<int>(type: "integer", nullable: false),
                    ProduitId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRs", x => x.id);
                    table.ForeignKey(
                        name: "FK_CRs_Produits_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "Produits",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SousCategoriesFinancieres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    categorie_financiere_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SousCategoriesFinancieres", x => x.id);
                    table.ForeignKey(
                        name: "FK_SousCategoriesFinancieres_CategoriesFinancieres_categorie_f~",
                        column: x => x.categorie_financiere_id,
                        principalTable: "CategoriesFinancieres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LignesFinancieres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    unite = table.Column<string>(type: "text", nullable: true),
                    montant = table.Column<double>(type: "double precision", nullable: false),
                    mois = table.Column<int>(type: "integer", nullable: false),
                    annee = table.Column<int>(type: "integer", nullable: false),
                    sous_categorie_financiere_id = table.Column<int>(type: "integer", nullable: false),
                    cr_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LignesFinancieres", x => x.id);
                    table.ForeignKey(
                        name: "FK_LignesFinancieres_CRs_cr_id",
                        column: x => x.cr_id,
                        principalTable: "CRs",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LignesFinancieres_SousCategoriesFinancieres_sous_categorie_~",
                        column: x => x.sous_categorie_financiere_id,
                        principalTable: "SousCategoriesFinancieres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesFinancieres_rapport_financier_id",
                table: "CategoriesFinancieres",
                column: "rapport_financier_id");

            migrationBuilder.CreateIndex(
                name: "IX_ClesDeRepartitions_societe_id",
                table: "ClesDeRepartitions",
                column: "societe_id");

            migrationBuilder.CreateIndex(
                name: "IX_CRs_ProduitId",
                table: "CRs",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_LignesFinancieres_cr_id",
                table: "LignesFinancieres",
                column: "cr_id");

            migrationBuilder.CreateIndex(
                name: "IX_LignesFinancieres_sous_categorie_financiere_id",
                table: "LignesFinancieres",
                column: "sous_categorie_financiere_id");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_famille_produit_id",
                table: "Produits",
                column: "famille_produit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_type_client_id",
                table: "Produits",
                column: "type_client_id");

            migrationBuilder.CreateIndex(
                name: "IX_RapportsFinanciers_societe_id",
                table: "RapportsFinanciers",
                column: "societe_id");

            migrationBuilder.CreateIndex(
                name: "IX_SousCategoriesFinancieres_categorie_financiere_id",
                table: "SousCategoriesFinancieres",
                column: "categorie_financiere_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserSocietes_societe_id",
                table: "UserSocietes",
                column: "societe_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ClesDeRepartitions");

            migrationBuilder.DropTable(
                name: "LignesFinancieres");

            migrationBuilder.DropTable(
                name: "UserSocietes");

            migrationBuilder.DropTable(
                name: "CRs");

            migrationBuilder.DropTable(
                name: "SousCategoriesFinancieres");

            migrationBuilder.DropTable(
                name: "Produits");

            migrationBuilder.DropTable(
                name: "CategoriesFinancieres");

            migrationBuilder.DropTable(
                name: "FamillesProduit");

            migrationBuilder.DropTable(
                name: "TypeClients");

            migrationBuilder.DropTable(
                name: "RapportsFinanciers");

            migrationBuilder.DropTable(
                name: "Societes");
        }
    }
}
