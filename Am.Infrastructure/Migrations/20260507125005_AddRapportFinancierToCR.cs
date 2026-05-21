using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRapportFinancierToCR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "rapport_financier_id",
                table: "CRs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CRs_rapport_financier_id",
                table: "CRs",
                column: "rapport_financier_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CRs_RapportsFinanciers_rapport_financier_id",
                table: "CRs",
                column: "rapport_financier_id",
                principalTable: "RapportsFinanciers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CRs_RapportsFinanciers_rapport_financier_id",
                table: "CRs");

            migrationBuilder.DropIndex(
                name: "IX_CRs_rapport_financier_id",
                table: "CRs");

            migrationBuilder.DropColumn(
                name: "rapport_financier_id",
                table: "CRs");
        }
    }
}
