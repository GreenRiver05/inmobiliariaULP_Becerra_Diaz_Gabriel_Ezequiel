using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaBD.Models
{
    public class Gestion
    {
        [Key]
        [Display(Name = "Registro N°")]
        public int Id { get; set; }


        // Usuario que realizó la acción (Admin, operador, etc.)
        [Required]
        public int UsuarioId { get; set; }


        // ID de la entidad afectada (Contrato, Multa, Pago, etc.)
        [Required]
        public int EntidadId { get; set; }


        // Tipo de entidad: "Contrato", "Multa", "Pago", etc.
        [Required]
        public string EntidadTipo { get; set; }


        // Acción realizada: "Alta", "Modificación", "Rescisión", "Renovación", "Pago registrado", etc.
        [Required]
        public string Accion { get; set; }


        // Fecha y hora exacta de la acción
        [Required]
        public DateTime Fecha { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }
    }
}