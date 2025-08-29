using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaBD.Models
{
    public class Propietario
    {
        [Key]
        [Display(Name = "N° Interno")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "DNI")]
        public int Dni { get; set; }

        
        public bool Estado { get; set; }

        //[ForeignKey("Dni")]
        [ForeignKey(nameof(Dni))]
        [Display(Name = "Persona")]
        public Persona? Persona { get; set; }
    }

}
