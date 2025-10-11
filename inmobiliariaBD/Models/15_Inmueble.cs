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
        //Comercial, Residencial
        public string Uso { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        //Cantidad de ambientes
        public int Ambientes { get; set; }

        public string? Observacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        //Disponible, No Disponible
        public bool Estado { get; set; }

        // Use string en Precio, Latitud y Longitud porque el tema del punto y la coma me estaba volviendo loco.
        // El binding automático no respetaba la coma como separador decimal, y me tiraba errores silenciosos.
        // Así que decidí manejar todo como texto, validar con Regex el formato que quiero (coma decimal, punto opcional para miles, signo negativo si aplica),
        // y guardar tal cual en la BD.

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression(@"^\d{1,3}(\.\d{3})*(,\d{1,2})?$", ErrorMessage = "Respeste formato (ej: 1.234,56).")]
        public string Precio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression(@"^-?\d+(,\d{1,6})?$", ErrorMessage = "Respete formato (ej: 34,123456)")]
        public string Longitud { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression(@"^-?\d+(,\d{1,6})?$", ErrorMessage = "Respete formato (ej: -64,123456)")]
        public string Latitud { get; set; }

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