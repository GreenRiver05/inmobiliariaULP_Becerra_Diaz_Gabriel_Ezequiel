namespace inmobiliariaBD.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> BuscarPorPropietario(int propietarioId);
        IList<Inmueble> BuscarPorDireccion(string direccion);
        IList<Inmueble> BuscarPorLocalidad(string localidad);
        IList<Inmueble> BuscarPorTipo(int tipoId);
        IList<Inmueble> BuscarPorPrecio(decimal precioMin, decimal precioMax);
        IList<Inmueble> BuscarPorEstado(string estado);
    }
}