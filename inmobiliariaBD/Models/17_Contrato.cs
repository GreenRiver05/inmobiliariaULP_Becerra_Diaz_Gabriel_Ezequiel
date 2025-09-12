using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace inmobiliariaBD.Models
{
    public class Contrato
    {
        [Key]
        [Display(Name = "N°")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Codigo Inquilino")]
        public int InquilinoId { get; set; }

        [Required]
        [Display(Name = "Codigo Inmueble")]
        public int InmuebleId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression(@"^\d{1,3}(\.\d{3})*(,\d{1,2})?$", ErrorMessage = "Respeste formato (ej: 1.234,56).")]
        public string Monto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime Desde { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime Hasta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        //Vigente, Finalizado, Rescindido
        public string Estado { get; set; }

        //[ForeignKey("InquilinoId")]
        [ForeignKey(nameof(InquilinoId))]
        [Display(Name = "Inquilino")]
        public Inquilino? Inquilino { get; set; }

        //[ForeignKey("InmuebleId")]
        [ForeignKey(nameof(InmuebleId))]
        [Display(Name = "Inmueble")]
        public Inmueble? Inmueble { get; set; }

        // [Display(Name = "Pagos")]
        // public List<Pago>? Pagos { get; set; }

        // [Display(Name = "Multas")]
        // public List<Multa>? Multas { get; set; }
    }
}