using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreMovies.Migrations
{
    /// <inheritdoc />
    public partial class Actors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.EnsureSchema(
                name: "movies");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "GenreTbl",
                newSchema: "movies");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "movies",
                table: "GenreTbl",
                newName: "GenreName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenreTbl",
                schema: "movies",
                table: "GenreTbl",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenreTbl",
                schema: "movies",
                table: "GenreTbl");

            migrationBuilder.RenameTable(
                name: "GenreTbl",
                schema: "movies",
                newName: "Genres");

            migrationBuilder.RenameColumn(
                name: "GenreName",
                table: "Genres",
                newName: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");
        }
    }
}
