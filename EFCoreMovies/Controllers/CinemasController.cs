using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using EFCoreMovies.Entities.Keyless;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Reflection.Metadata;

namespace EFCoreMovies.Controllers
{
    [ApiController]
    [Route("api/cinemas")]
    public class CinemasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CinemasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<CinemaDTO>> Get()
        {
            return await context.Cinemas.AsNoTracking().ProjectTo<CinemaDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("withoutLocation")]
        public async Task<IEnumerable<CinemaWithoutLocation>> GetWithoutLocation()
        {
            //return await context.Set<CinemaWithoutLocation>().ToListAsync();
            return await context.CinemasWithoutLocations.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var cinemaDB = await context.Cinemas
                .Include(c => c.CinemaHalls)
                .Include(c => c.CinemaOffer)
                .Include(c => c.CinemaDetail)
                .FirstOrDefaultAsync(c => c.Id == id);

            //var cinemaDB = await context.Cinemas.FromSqlInterpolated($"SELECT * FROM Cinemas WHERE Id = {id}")
            //        .Include(c => c.CinemaHalls)
            //        .Include(c => c.CinemaOffer)
            //        .Include(c => c.CinemaDetail)
            //    .FirstOrDefaultAsync();

            if (cinemaDB is null)
            {
                return NotFound();
            }

            cinemaDB.Location = null;

            return Ok(cinemaDB);
        }

        [HttpGet("leftjoin")]
        public ActionResult LeftJoinGet()
        {
            var query = from c in context.Set<Cinema>()
                        join co in context.Set<CinemaOffer>()
                            on c.Id equals co.CinemaId into grouping
                        from co in grouping.DefaultIfEmpty()
                        select new { c.Name, co };

            return Ok(query.ToList());
        }

        [HttpGet("closetome")]
        public async Task<ActionResult> Get(double latitude, double longitude)
        {
            var geometryFactor = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var myLocation = geometryFactor.CreatePoint(new Coordinate(longitude, latitude));

            var cinemas = await context.Cinemas.OrderBy(c => c.Location.Distance(myLocation)).Where(c => c.Location.IsWithinDistance(myLocation, 2000)).Select(c => new 
            {
                c.Name,
                Distance = Math.Round(c.Location.Distance(myLocation)),
            
            }).ToListAsync();

            return Ok(cinemas);
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            var geometryFactor = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var cinemaLocation = geometryFactor.CreatePoint(new Coordinate(-69.913539, 18.476256));

            var cinema = new Cinema()
            {
                Name = "My cinema",
                Location = cinemaLocation,
                CinemaOffer = new CinemaOffer()
                {
                    DiscountPercentage = 5,
                    Begin = DateTime.Today,
                    End = DateTime.Today.AddDays(7)
                },
                CinemaHalls = new HashSet<CinemaHall>
                {
                    new CinemaHall()
                    {
                        Cost = 200,
                        Currency = Currency.DominicanPeso,
                        CinemaHallType = CinemaHallType.TwoDimensions
                    },
                    new CinemaHall()
                    {
                        Cost = 250,
                        Currency = Currency.USDollar,
                        CinemaHallType = CinemaHallType.TwoDimensions
                    }
                },
                CinemaDetail = new CinemaDetail()
                {
                    History = "The History...",
                    Missions = "The Missions...",
                }
            };

            context.Cinemas.Add(cinema);

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("withDTO")]
        public async Task<ActionResult> PostWithDTO(CinemaCreationDTO cinemaCreationDTO)
        {
            var cinema = mapper.Map<Cinema>(cinemaCreationDTO);

            context.Add(cinema);

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("cinemaOffer")]
        public async Task<ActionResult> PutCinemaOffer(CinemaOffer cinemaOffer)
        {
            context.Update(cinemaOffer);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put(CinemaCreationDTO cinemaCreationDTO, int id)
        {
            var cinemaDB = await context.Cinemas
                .Include(c => c.CinemaHalls)
                .Include(c => c.CinemaOffer).FirstOrDefaultAsync(c => c.Id == id);

            if(cinemaDB is null)
            {
                return NotFound();
            }

            cinemaDB = mapper.Map(cinemaCreationDTO, cinemaDB);

            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cinema = await context.Cinemas.Include(p => p.CinemaHalls).FirstOrDefaultAsync(c => c.Id == id);

            if(cinema is null)
            {
                return NotFound();
            }

            context.Remove(cinema);

            await context.SaveChangesAsync();

            return Ok();
        }


    }
}

