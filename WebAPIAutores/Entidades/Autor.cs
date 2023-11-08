using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:100, ErrorMessage ="El campo {0} no debe ser mayor a {1}")]
        [LetraCapital]
        public string Nombre { get; set;}
        //[Range(18,120)]
        //[NotMapped]  //Indica que no es necesario que EF mapee la propiedad a una tabla
        //public int Edad { get; set; }
        //[NotMapped]
        //[CreditCard]  //para validar # de tarjeta a 16 dígitos
        //public string CreditCard { get; set; }
        //[NotMapped]
        //[Url]  //Para validar url
        //public string Url { get; set; }
        //[NotMapped]
        //public int Menor { get; set; }
        //[NotMapped]
        //public int Mayor { get; set; }

        //Propiedad de navegación Libros de un autor
        public List<Libro> Libros { get; set; }


        //Validaciones por modelo: para que se ejecuten primero se deben pasar TODAS las validaciones de atributo
        //public IEnumerable<ValidationResult> validate(ValidationContext validationContext)
        //{
        //    if(!string.IsNullOrEmpty(Nombre))
        //    {
        //        var letra = Nombre[0].ToString();

        //        //yield agrega un registro a IEnumerable
        //        if (letra != letra.ToUpper()) { 
        //            yield return new ValidationResult("La primera letra debe ser mayúscula",
        //                new string[] { nameof(Nombre)});
        //        }
        //    }

        //    //if (Menor >= Mayor) { 
        //    //    yield return new ValidationResult("El campo Menor debe ser más grande que el campo Mayor",
        //    //        new string[] { nameof(Mayor)});
        //    //}
        //}
    }
}
