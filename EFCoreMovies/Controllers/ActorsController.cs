using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using EFCoreMovies.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActorsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<ActorDTO>> Get()
        {
            return await context.Actors.AsNoTracking().OrderBy(g => g.Name).ProjectTo<ActorDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ActorCreationDTO actorCreationDTO)
        {
            var actor = mapper.Map<Actor>(actorCreationDTO);
            context.Add(actor);

            await context.SaveChangesAsync();

            return Ok(actor);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ActorCreationDTO actorCreationDTO, [FromRoute]int id)
        {
            var actorDB = await context.Actors.FirstOrDefaultAsync(p => p.Id == id);

            if(actorDB is null)
            {
                return NotFound();
            }

            actorDB = mapper.Map(actorCreationDTO, actorDB);

            await context.SaveChangesAsync();   

            return Ok(actorDB);
        }

        [HttpPut("disconnected/{id:int}")]
        public async Task<ActionResult> PutDisconnected(ActorCreationDTO actorCreationDTO, [FromRoute] int id)
        {
            var existActor = await context.Actors.AnyAsync(p => p.Id == id);

            if (!existActor)
            {
                return NotFound();
            }

            var actor = mapper.Map<Actor>(actorCreationDTO);

            actor.Id = id;

            //context.Entry(actor).Property(p => p.Name).IsModified = true;

            context.Update(actor);

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}

