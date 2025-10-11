namespace inmobiliariaBD.Models
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        Usuario ObtenerPorEmail(string email);
        IList<Usuario> BuscarPorNombre(string nombre);
       
    }
}