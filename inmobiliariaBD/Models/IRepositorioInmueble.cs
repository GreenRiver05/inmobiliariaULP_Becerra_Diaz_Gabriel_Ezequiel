namespace inmobiliariaBD.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> ObtenerPorPropietario(int propietarioId);
        IList<Inmueble> Buscar(string nombre);
        
        IList<Inmueble> BuscarDisponiblesEntreFechas(DateTime desde, DateTime hasta);
        IList<TipoInmueble> ObtenerTiposInmueble();
    }
}