namespace inmobiliariaBD.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        IList<Contrato> ObtenerPorInquilino(int inquilinoId);
        IList<Contrato> ObtenerPorInmueble(int inmuebleId);
        IList<Contrato> BuscarPorEstado(string estado);
        IList<Contrato> BuscarVigentes();
    }
        
}