using AutoMapper;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public GenresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Genre>> Get()
        {
            context.Logs.Add(new Log() { Message = "Executing Get from GenresController"});
            await context.SaveChangesAsync();
            return await context.Genres.AsNoTracking().OrderByDescending(g => EF.Property<DateTime>(g, "CreatedDate")).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            //var genre = await context.Genres.FromSqlInterpolated($"SELECT * FROM GENRES WHERE Id = {id}")
            //    .IgnoreQueryFilters().FirstOrDefaultAsync();

            var genre = await context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
            {
                return NotFound();
            }

            var createdDate = context.Entry(genre).Property<DateTime>("CreatedDate").CurrentValue;

            return Ok(new
            {
                Id = genre.Id,
                Name = genre.Name,
                CreatedDate = createdDate
            });
        }

        [HttpGet("stored_procedure/{id:int}")]
        public async Task<ActionResult<Genre>> GetByStoredProcedure(int id)
        {
            var genres = context.Genres.FromSqlInterpolated($"EXEC Genres_GetById {id}")
                .IgnoreQueryFilters().AsAsyncEnumerable();

            await foreach(var genre in genres)
            {
                return genre;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(GenreCreationDTO genreCreationDTO)
        {

            var genreExists = await context.Genres.AnyAsync(g => g.Name == genreCreationDTO.Name);
            if (genreExists)
            {
                return BadRequest($"The genre with name {genreCreationDTO.Name} already exists.");
            }
            var genre = mapper.Map<Genre>(genreCreationDTO);

            context.Genres.Add(genre);

            //context.Entry(genre).State = EntityState.Added;

            //            await context.Database.ExecuteSqlInterpolatedAsync($@"
            //INSERT INTO GENRES (Name) VALUES ({genre.Name})
            //");

            await context.SaveChangesAsync();

            return Ok();

        }

        [HttpPost("stored_procedure")]
        public async Task<ActionResult> PostByStoredProcedure(GenreCreationDTO genreCreationDTO)
        {

            var genreExists = await context.Genres.AnyAsync(g => g.Name == genreCreationDTO.Name);
            if (genreExists)
            {
                return BadRequest($"The genre with name {genreCreationDTO.Name} already exists.");
            }

            var output = new SqlParameter();
            output.ParameterName = "@id";
            output.SqlDbType = System.Data.SqlDbType.Int;
            output.Direction = System.Data.ParameterDirection.Output;

            await context.Database.ExecuteSqlRawAsync("EXEC Genres_Insert @name = {0}, @id = {1} OUTPUT", genreCreationDTO.Name, output);

            var id = (int)output.Value;

            return Ok(id);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Genre genre)
        {
            context.Update(genre);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("several")]
        public async Task<ActionResult> Post(GenreCreationDTO[] genresDTO)
        {
            var genres = mapper.Map<Genre[]>(genresDTO);

            context.Genres.AddRange(genres);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("add2")]
        public async Task<ActionResult> Add2(int id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(p => p.Id == id);

            if( genre is null)
            {
                return NotFound();
            }

            genre.Name += " 2";

            await context.SaveChangesAsync();

            return Ok();    
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if(genre is null)
            {
                return NotFound();
            }

            context.Remove(genre);

            await context.SaveChangesAsync();

            return Ok();

        }

        [HttpDelete("softdelete/{id:int}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
            {
                return NotFound();
            }

            genre.IsDeleted = true;

            await context.SaveChangesAsync();

            return Ok();

        }


        [HttpPost("restore/{id:int}")]
        public async Task<ActionResult> Restore(int id)
        {
            var genre = await context.Genres.IgnoreQueryFilters().FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
            {
                return NotFound();
            }

            genre.IsDeleted = false;

            await context.SaveChangesAsync();

            return Ok();

        }
    }
}
