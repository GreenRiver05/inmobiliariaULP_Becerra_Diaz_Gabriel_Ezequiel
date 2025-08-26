namespace inmobiliariaBD.Models
{
    public interface IRepositorioContacto : IRepositorio<Contacto>
    {

        IList<Persona> BuscarPorDni(int dni);
        
    }
}