using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaBD.Models
{
    public class Inquilino
    {
        [Key]
        [Display(Name = "N°")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "DNI")]
        public int Dni { get; set; }

        [Required]
        public bool Estado { get; set; }

        //[ForeignKey("Dni")]
        [ForeignKey(nameof(Dni))]
        [Display(Name = "Persona")]
        public Persona Persona { get; set; }
    }

}
