using System.ComponentModel.DataAnnotations;

namespace inmobiliariaBD.Models
{
    public class TipoInmueble
    {
        [Key]
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        //Local, Deposito, Casa, Departamento
        public string? Tipo { get; set; }


        [Display(Name = "Tipo de Inmueble")]
        public string? Descripcion { get; set; }

        // public List<Inmueble>? Inmuebles { get; set; }
    }
}