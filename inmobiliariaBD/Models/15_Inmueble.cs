using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaBD.Models
{
    public class Inmueble
    {
        [Key]
        [Display(Name = "N°")]
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Codigo Propietario")]
        public int PropietarioId { get; set; }

        [Required]
        [Display(Name = "Tipo Inmueble")]
        //Local, Deposito, Casa, Departamento
        public int TipoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Localidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal Longitud { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal Latitud { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        //Comercial, Residencial
        public string Uso { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        //Cantidad de ambientes
        public int Ambientes { get; set; }

        public string? Observacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        //Disponible, Alquilado, No Disponible
        public string Estado { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal Precio { get; set; }


        //[ForeignKey("PropietarioId")]
        [ForeignKey(nameof(PropietarioId))]
        [Display(Name = "Propietario")]
        public Propietario? Propietario { get; set; }

        //[ForeignKey("TipoId")]
        [ForeignKey(nameof(TipoId))]
        [Display(Name = "Tipo de Inmueble")]
        public TipoInmueble? TipoInmueble { get; set; }

        // public List<Contrato>? Contratos { get; set; }
    }

}