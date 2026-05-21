using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDashEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExcelVariables_Societes_societe_id",
                table: "ExcelVariables");

            migrationBuilder.DropIndex(
                name: "IX_ExcelVariables_societe_id",
                table: "ExcelVariables");

            migrationBuilder.CreateTable(
                name: "CBDashes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    sous_category = table.Column<string>(type: "text", nullable: true),
                    societe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CBDashes", x => x.id);
                    table.ForeignKey(
                        name: "FK_CBDashes_Societes_societe_id",
                        column: x => x.societe_id,
                        principalTable: "Societes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountDashes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_entity = table.Column<string>(type: "text", nullable: true),
                    societe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountDashes", x => x.id);
                    table.ForeignKey(
                        name: "FK_CountDashes_Societes_societe_id",
                        column: x => x.societe_id,
                        principalTable: "Societes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CBDashes_societe_id",
                table: "CBDashes",
                column: "societe_id");

            migrationBuilder.CreateIndex(
                name: "IX_CountDashes_societe_id",
                table: "CountDashes",
                column: "societe_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CBDashes");

            migrationBuilder.DropTable(
                name: "CountDashes");

            migrationBuilder.CreateIndex(
                name: "IX_ExcelVariables_societe_id",
                table: "ExcelVariables",
                column: "societe_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExcelVariables_Societes_societe_id",
                table: "ExcelVariables",
                column: "societe_id",
                principalTable: "Societes",
                principalColumn: "id");
        }
    }
}
