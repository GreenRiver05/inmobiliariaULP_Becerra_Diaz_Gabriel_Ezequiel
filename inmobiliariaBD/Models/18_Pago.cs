using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaBD.Models
{
    public class Pago
    {
        [Key]
        [Display(Name = "Registro N°")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Codigo Contrato")]
        public int ContratoId { get; set; }

        [Required]
        [Display(Name = "Número de Pago")]
        public int NumeroPago { get; set; }

        [Required]
        public string Monto { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Detalle { get; set; }

        [Required]
        //parcial, total
        public string Tipo { get; set; }

        [Required]
        //pago, anulado, pendiente
        public string Estado { get; set; }

        //[ForeignKey("ContratoId")]
        [ForeignKey(nameof(ContratoId))]

        public Contrato Contrato { get; set; }
    }

}