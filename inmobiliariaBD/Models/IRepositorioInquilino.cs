namespace inmobiliariaBD.Models
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
        Persona ObtenerPorDni(int dni);
        
    }
}