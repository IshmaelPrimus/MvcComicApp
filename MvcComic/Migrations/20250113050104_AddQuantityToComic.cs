using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcComic.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityToComic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Comic",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Comic");
        }
    }
}
