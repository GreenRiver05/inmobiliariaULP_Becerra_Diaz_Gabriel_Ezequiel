namespace inmobiliariaBD.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        Propietario ObtenerPorDni(int dni);
        IList<Propietario> BuscarPorNombre(string nombre);

    
        
    }
}