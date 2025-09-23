namespace inmobiliariaBD.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        IList<Pago> ObtenerPagosPorContrato(int id);
    }
}