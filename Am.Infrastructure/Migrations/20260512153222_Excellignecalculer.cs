using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Excellignecalculer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExcelLigneCalculees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    variable = table.Column<string>(type: "text", nullable: true),
                    ligne_calculee_id = table.Column<int>(type: "integer", nullable: false),
                    societe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelLigneCalculees", x => x.id);
                    table.ForeignKey(
                        name: "FK_ExcelLigneCalculees_LignesCalculees_ligne_calculee_id",
                        column: x => x.ligne_calculee_id,
                        principalTable: "LignesCalculees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcelLigneCalculees_Societes_societe_id",
                        column: x => x.societe_id,
                        principalTable: "Societes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExcelLigneCalculees_ligne_calculee_id",
                table: "ExcelLigneCalculees",
                column: "ligne_calculee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExcelLigneCalculees_societe_id",
                table: "ExcelLigneCalculees",
                column: "societe_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcelLigneCalculees");
        }
    }
}
