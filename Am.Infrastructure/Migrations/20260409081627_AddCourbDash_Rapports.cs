using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Am.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCourbDash_Rapports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rapport1",
                table: "CourbDashes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rapport2",
                table: "CourbDashes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rapport1",
                table: "CourbDashes");

            migrationBuilder.DropColumn(
                name: "rapport2",
                table: "CourbDashes");
        }
    }
}
