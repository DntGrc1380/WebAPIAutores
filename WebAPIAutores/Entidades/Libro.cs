using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [LetraCapital]
        public string Titulo { get; set; }
        //public int AutorId { get; set;}
        ////Propiedad de navegación Libro -> Autor
        //public Autor Autor { get; set;}
    }
}
