using System.ComponentModel.DataAnnotations;

namespace inmobiliariaBD.Models
{
    public class Persona
    {

        // las propiedades son como los atributos pero con get y set
        // los atributos se crean por detras de las propiedades
        // las propiedades son publicas y los atributos privados

        [Key]
        [Display(Name = "DNI")]
        public int Dni { get; set; }

        [Required]
        public required string Nombre { get; set; }

        [Required]
        public required string Apellido { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        public string? Localidad { get; set; }

        public string? Correo { get; set; }

        [Display(Name = "Teléfono")]
        [Required]
        public int Telefono { get; set; }

       
        public bool Estado { get; set; }


    }

}
