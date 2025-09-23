namespace inmobiliariaBD.Models
{
    public interface IRepositorioMulta : IRepositorio<Multa>
    {
          IList<Multa> ObtenerMultasPorContrato(int id);
    }
}