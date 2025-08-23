using System.ComponentModel.DataAnnotations;

namespace inmobiliariaBD.Models
{
    public class Persona
    {
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

        [Required]
        public bool Estado { get; set; }

        public byte[]? Avatar { get; set; }
    }

}
