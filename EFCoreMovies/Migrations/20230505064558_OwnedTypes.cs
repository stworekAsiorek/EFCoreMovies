using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreMovies.Migrations
{
    /// <inheritdoc />
    public partial class OwnedTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Cinemas",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Cinemas",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Cinemas",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_County",
                table: "Actors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Province",
                table: "Actors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Street",
                table: "Actors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress_County",
                table: "Actors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress_Province",
                table: "Actors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress_Street",
                table: "Actors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Cinemas");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Cinemas");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Cinemas");

            migrationBuilder.DropColumn(
                name: "BillingAddress_County",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Province",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Street",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "HomeAddress_County",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "HomeAddress_Province",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "HomeAddress_Street",
                table: "Actors");
        }
    }
}
