using System.ComponentModel.DataAnnotations;

namespace inmobiliariaBD.Models
{
    public class Persona
    {

        // las propiedades son como los atributos pero con get y set
        // los atributos se crean por detras de las propiedades
        // las propiedades son publicas y los atributos privados

        [Key]
        [Display(Name = "DNI")]
        public int Dni { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre/s")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [Display(Name = "Apellido/s")]
        public string Apellido { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        public string? Localidad { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public long Telefono { get; set; }


        public bool Estado { get; set; }

        public override string ToString()
        {
            return $"{Nombre} {Apellido} (DNI: {Dni}) - Tel: {Telefono}" +
                   $"{(string.IsNullOrEmpty(Correo) ? "" : $", Email: {Correo}")}" +
                   $"{(string.IsNullOrEmpty(Direccion) ? "" : $", Dirección: {Direccion}")}" +
                   $"{(string.IsNullOrEmpty(Localidad) ? "" : $", Localidad: {Localidad}")}" +
                   $", Estado: {(Estado ? "Activo" : "Inactivo")})";

        }

        public string ToStringSimple()
        {
            return $"{Nombre} {Apellido} ({Dni})";
        }

    }
}
