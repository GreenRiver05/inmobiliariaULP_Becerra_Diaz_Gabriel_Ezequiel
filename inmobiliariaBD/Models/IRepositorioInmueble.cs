namespace inmobiliariaBD.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> ObtenerPorPropietario(int propietarioId);
        IList<Inmueble> Buscar(string nombre);
        
        IList<TipoInmueble> ObtenerTiposInmueble();
    }
}