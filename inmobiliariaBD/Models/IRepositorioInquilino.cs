namespace inmobiliariaBD.Models
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
        Inquilino ObtenerPorDni(int dni);
        IList<Inquilino> BuscarPorNombre(string nombre);
        
        
    }
}