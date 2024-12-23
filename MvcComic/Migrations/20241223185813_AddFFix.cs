using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcComic.Migrations
{
    /// <inheritdoc />
    public partial class AddFFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Issue",
                table: "Comic");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Comic");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Comic");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Comic",
                newName: "Volume");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Volume",
                table: "Comic",
                newName: "Genre");

            migrationBuilder.AddColumn<int>(
                name: "Issue",
                table: "Comic",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Comic",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Comic",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
