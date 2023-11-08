using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibros() {
            return await context.Libros.Include(x => x.Autor).ToListAsync();
        }

        [HttpGet("LibroById/{Id:int}")]
        public async Task<ActionResult<Libro>> GetLibro(int Id) {
            return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == Id);
        }

        [HttpPost]
        public async Task<ActionResult> AddLibro(Libro libro) {
            var autor = await context.Autores.AnyAsync(x => x.Id.Equals(libro.AutorId));
            if (!autor)
            {
                return BadRequest($"No existe el autor Id: {libro.AutorId}"); 
            }
            context.Libros.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
