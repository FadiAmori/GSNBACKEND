using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExcelVariable_SocieteId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "societe_id",
                table: "ExcelVariables",
                type: "integer",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExcelVariables_Societes_societe_id",
                table: "ExcelVariables");

            migrationBuilder.DropIndex(
                name: "IX_ExcelVariables_societe_id",
                table: "ExcelVariables");

            migrationBuilder.DropColumn(
                name: "societe_id",
                table: "ExcelVariables");
        }
    }
}
