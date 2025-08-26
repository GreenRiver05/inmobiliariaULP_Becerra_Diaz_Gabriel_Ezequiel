namespace inmobiliariaBD.Models
{
    public interface IRepositorioPersona : IRepositorio<Persona>
    {
        Persona ObtenerPorDni(int dni);
        IList<Persona> BuscarPorNombre(string nombre);
    }
}