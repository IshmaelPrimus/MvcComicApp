using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcComic.Migrations
{
    /// <inheritdoc />
    public partial class AddIssueNumberToComic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssueNumber",
                table: "Comic",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueNumber",
                table: "Comic");
        }
    }
}
