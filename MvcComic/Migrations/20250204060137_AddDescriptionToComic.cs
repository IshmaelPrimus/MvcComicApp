using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcComic.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToComic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Comic",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Comic");
        }
    }
}
