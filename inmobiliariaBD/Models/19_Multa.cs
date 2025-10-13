using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaBD.Models
{
    public class Multa
    {
        [Key]
        [Display(Name = "Registro N°")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Codigo Contrato")]
        public int ContratoId { get; set; }

        [Required]
        public DateTime FechaAviso { get; set; }

        [Required]
        public DateTime FechaTerminacion { get; set; }

        [Required]
        public string Monto { get; set; }

        //[ForeignKey("ContratoId")]
        [ForeignKey(nameof(ContratoId))]
        public Contrato? Contrato { get; set; }
    }

}