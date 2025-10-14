namespace inmobiliariaBD.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        IList<Contrato> ObtenerPorInquilino(int inquilinoId);
        IList<Contrato> ObtenerPorInmueble(int inmuebleId);
        IList<Contrato> BuscarPorEstado(string estado);

        bool ExisteSuperposicion(int inmuebleId, DateTime desde, DateTime hasta, int contratoId = 0);
        IList<Contrato> BuscarVigentes();
    }
        
}