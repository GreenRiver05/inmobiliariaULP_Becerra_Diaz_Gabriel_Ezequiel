using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaBD.Models
{

    public enum Roles
    {
        Admin = 1,
        User = 2,
    }


    public class Usuario
    {
        [Key]
        [Display(Name = "N°")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "DNI")]
        public int Dni { get; set; }

        [Required, DataType(DataType.Password)]
        public string Contraseña { get; set; }

        [Required]
        public int Rol { get; set; }

        public string Avatar { get; set; } = "";

        // Archivo subido desde el formulario (imagen), no se guarda en la base
        [NotMapped] // EF ignora esta propiedad al mapear la tabla
        public IFormFile? AvatarFile { get; set; }

        // Propiedad para obtener el nombre del rol como cadena
        [NotMapped]
        public string RolNombre => Rol > 0 ? ((Roles)Rol).ToString() : "";

        [Required]
        public bool Estado { get; set; }

        //[ForeignKey("Dni")]
        [ForeignKey(nameof(Dni))]
        [Display(Name = "Usuario")]
        public Persona Persona { get; set; }


        public static IDictionary<int, string> ObtenerRoles()
        {
            // Diccionario ordenado que va a contener los roles
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();

            // Obtiene el tipo del enum enRoles
            Type tipoEnumRol = typeof(Roles);

            // Recorre todos los valores definidos en el enum
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
                // Agrega al diccionario: clave = número del rol, valor = nombre del rol
                roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
            }

            // Devuelve el diccionario completo
            return roles;
        }

    }

}
