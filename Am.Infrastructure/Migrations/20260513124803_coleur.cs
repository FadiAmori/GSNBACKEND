using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coleur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "couleur",
                table: "SousCategoriesFinancieres",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "couleur",
                table: "LignesFinancieres",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "couleur",
                table: "LignesCalculees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "couleur",
                table: "CategoriesFinancieres",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "couleur",
                table: "SousCategoriesFinancieres");

            migrationBuilder.DropColumn(
                name: "couleur",
                table: "LignesFinancieres");

            migrationBuilder.DropColumn(
                name: "couleur",
                table: "LignesCalculees");

            migrationBuilder.DropColumn(
                name: "couleur",
                table: "CategoriesFinancieres");
        }
    }
}
