using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaBD.Models
{
    public class Contacto
    {
        [Key]
        [Display(Name = "N° interno")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "DNI")]
        public int Dni { get; set; }

        public string? Correo { get; set; }

        [Display(Name = "Teléfono")]
        public int? Telefono { get; set; }

        [Display(Name = "Teléfono Secundario")]
        public int? TelefonoSecundario { get; set; }

        //[ForeignKey("Dni")]
        [ForeignKey(nameof(Dni))]
        [Display(Name = "Persona")]
        public Persona Persona { get; set; }
    }
}