using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionToCategoryfr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "SousCategoriesFinancieres",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "CategoriesFinancieres",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "position",
                table: "SousCategoriesFinancieres");

            migrationBuilder.DropColumn(
                name: "position",
                table: "CategoriesFinancieres");
        }
    }
}
