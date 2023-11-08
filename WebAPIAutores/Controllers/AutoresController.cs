using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPIAutores.Entidades;
using WebAPIAutores.Filtros;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Autorize]  //Si se utiliza a éste nivel, se protegen TODOS los endpoints del controlador
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        //inyectar dependencias en el constructor
        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        //[ResponseCache(Duration = 10)] //Utiliza el caché configurado, el número representa el tiempo en segundos que se usará la misma data
        //[Authorize]  //Sirve para proteger los endpoints, se configura previamente en Program.cs o Startup.cs
        public async Task<ActionResult<List<Autor>>> GetAutores()
        {
            throw new System.NotImplementedException();
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpPost]
        //public async Task<ActionResult<Autor>> AddAutor(Autor autor)
        public async Task<ActionResult<Autor>> AddAutor([FromBody] Autor autor)
        {
            //Validación de nombre existente
            var existeAutor = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);

            if (existeAutor) {
                return BadRequest($"Ya existe un autor con el nombre {autor.Nombre}");
            }
            //Validación de nombre existente

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> EditAutor(int Id, Autor autor)
        {
            if (Id != autor.Id)
            {
                return BadRequest("Los id de autor NO coinciden");
            }
            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        //Borrado lógico
        [HttpPatch("Delete/{Id:int}")]
        public async Task<ActionResult> DelAutor(int Id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(a => a.Id == Id);
            if (autor != null)
            {
                autor.Nombre = "---";
                context.Update(autor);
                await context.SaveChangesAsync();
            }
            else
            {
                return NotFound("No se encontró el registro");
            }

            return Ok();
        }

        //Borrado físico
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteAutor(int Id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(a => a.Id == Id);
            if (autor != null)
            {
                context.Remove(autor);
                await context.SaveChangesAsync();
            }
            else
            {
                return NotFound("No se encontró el registro");
            }

            return Ok();
        }
    }
}
