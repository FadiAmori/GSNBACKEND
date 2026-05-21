using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategorieCr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date_affectation",
                table: "UserSocietes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_creation",
                table: "Societes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_affectation",
                table: "Societes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "sous_categorie_cr_id",
                table: "LignesFinancieres",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_creation",
                table: "Admins",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "CategoriesCR",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    rapport_financier_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesCR", x => x.id);
                    table.ForeignKey(
                        name: "FK_CategoriesCR_RapportsFinanciers_rapport_financier_id",
                        column: x => x.rapport_financier_id,
                        principalTable: "RapportsFinanciers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SousCategoriesCR",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "text", nullable: true),
                    categorie_cr_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SousCategoriesCR", x => x.id);
                    table.ForeignKey(
                        name: "FK_SousCategoriesCR_CategoriesCR_categorie_cr_id",
                        column: x => x.categorie_cr_id,
                        principalTable: "CategoriesCR",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LignesFinancieres_sous_categorie_cr_id",
                table: "LignesFinancieres",
                column: "sous_categorie_cr_id");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesCR_rapport_financier_id",
                table: "CategoriesCR",
                column: "rapport_financier_id");

            migrationBuilder.CreateIndex(
                name: "IX_SousCategoriesCR_categorie_cr_id",
                table: "SousCategoriesCR",
                column: "categorie_cr_id");

            migrationBuilder.AddForeignKey(
                name: "FK_LignesFinancieres_SousCategoriesCR_sous_categorie_cr_id",
                table: "LignesFinancieres",
                column: "sous_categorie_cr_id",
                principalTable: "SousCategoriesCR",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LignesFinancieres_SousCategoriesCR_sous_categorie_cr_id",
                table: "LignesFinancieres");

            migrationBuilder.DropTable(
                name: "SousCategoriesCR");

            migrationBuilder.DropTable(
                name: "CategoriesCR");

            migrationBuilder.DropIndex(
                name: "IX_LignesFinancieres_sous_categorie_cr_id",
                table: "LignesFinancieres");

            migrationBuilder.DropColumn(
                name: "sous_categorie_cr_id",
                table: "LignesFinancieres");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_affectation",
                table: "UserSocietes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_creation",
                table: "Societes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_affectation",
                table: "Societes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_creation",
                table: "Admins",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
