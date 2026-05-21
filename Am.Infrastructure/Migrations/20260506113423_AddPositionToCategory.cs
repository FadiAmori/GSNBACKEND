using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "SousCategoriesCR",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "LignesFinancieres",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "CategoriesFinancieres",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "SousCategoriesFinancieres",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "CategoriesCR",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "position",
                table: "SousCategoriesCR");

            migrationBuilder.DropColumn(
                name: "position",
                table: "LignesFinancieres");

            migrationBuilder.DropColumn(
                name: "position",
                table: "CategoriesFinancieres");

            migrationBuilder.DropColumn(
                name: "position",
                table: "SousCategoriesFinancieres");

            migrationBuilder.DropColumn(
                name: "position",
                table: "CategoriesCR");
        }
    }
}
