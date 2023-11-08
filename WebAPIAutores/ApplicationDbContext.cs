using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;

namespace WebAPIAutores
{
    public class ApplicationDbContext: DbContext
    {
        //A través del dbcontext se pueden pasar distintas configuraciones, tales como el cnnString para conectar a BD
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        //También se pueden configurar las tablas que se generarán através de Entityframework
        //El agregar DbSet<Entidad> permite realziar consultas directas a la entidad
        //La siguiente línea crea una tabla a partir de la clase Autor
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        
    }
}
