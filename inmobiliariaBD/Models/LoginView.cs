using System.ComponentModel.DataAnnotations;

namespace inmobiliariaBD.Models
{
    public class LoginView
    {

        // Campo obligatorio que representa el email del usuario
        // Se valida que tenga formato de correo electrónico
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un email válido")]
        public string Usuario { get; set; } = "";

        // Campo obligatorio que representa la contraseña
        // Se renderiza como campo tipo password en la vista
        [Required(ErrorMessage = "La clave es obligatoria")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = "";
    }
}