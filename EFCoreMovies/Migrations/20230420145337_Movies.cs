﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreMovies.Migrations
{
    /// <inheritdoc />
    public partial class Movies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InCinemas = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    PosterURL = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");

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
        }
    }
}
