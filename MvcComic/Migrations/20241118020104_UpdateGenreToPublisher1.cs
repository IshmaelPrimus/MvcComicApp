using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcComic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGenreToPublisher1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Comic",
                newName: "Publisher");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Publisher",
                table: "Comic",
                newName: "Genre");
        }
    }
}
