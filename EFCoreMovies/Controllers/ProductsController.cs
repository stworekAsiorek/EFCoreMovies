using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController
    {
        public readonly ApplicationDbContext context;

        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return await context.Products.ToListAsync();
        }

        [HttpGet("merch")]
        public async Task<ActionResult<IEnumerable<Product>>> GetMerch()
        {
            return await context.Set<Merchandising>().ToListAsync();
        }

        [HttpGet("rentables")]
        public async Task<ActionResult<IEnumerable<RentableMovie>>> GetRentable()
        {
            return await context.Set<RentableMovie>().ToListAsync();
        }
    }
}
