using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreMovies.Migrations
{
    /// <inheritdoc />
    public partial class ViewMovieCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW dbo.MoviesWithCounts 
            as
            SELECT Id, Title, 
            (SELECT COUNT(*) FROM dbo.GenreMovie where MoviesId = Movies.Id) as AmountGenres,
            (SELECT COUNT(distinct moviesId) FROM dbo.CinemaHallMovie INNER JOIN CinemaHalls ON CinemaHalls.Id = CinemaHallMovie.CinemaHallsId WHERE CinemaHallMovie.MoviesId = Movies.Id) as AmountCinemas,
            (SELECT COUNT(*) FROM dbo.MoviesActors where MovieId = Movies.Id) as AmountActors
            from dbo.Movies
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW dbo.MoviesWithCounts");
        }
    }
}
