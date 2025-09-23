using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliariaBD.Models
{
    public class RepositorioPago : RepositorioBase, IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Pago p)
        {
            throw new NotImplementedException();
        }

        public int Baja(Pago p)
        {
            throw new NotImplementedException();
        }

        public int Modificacion(Pago p)
        {
            throw new NotImplementedException();
        }

        public int ModificarEstado(Pago p)
        {
            throw new NotImplementedException();
        }

        public int ObtenerCantidad()
        {
            throw new NotImplementedException();
        }

        public IList<Pago> ObtenerPaginados(int pagina, int cantidadPorPagina)
        {
            throw new NotImplementedException();
        }

        public IList<Pago> ObtenerPagosPorContrato(int contratoId)
        {
            IList<Pago> res = new List<Pago>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.id, p.contrato_id AS contratoID, p.numero_pago AS numeroPago, p.monto, p.fecha, p.detalle, p.tipo, p.estado
                             FROM pago p
                             WHERE p.contrato_Id = @contratoId";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@contratoId", contratoId);

                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            Id = reader.GetInt32(nameof(p.Id)),
                            ContratoId = reader.GetInt32(nameof(p.ContratoId)),
                            NumeroPago = reader.GetInt32(nameof(p.NumeroPago)),
                            Monto = reader.GetString(nameof(p.Monto)),
                            Fecha = reader.GetDateTime(nameof(p.Fecha)),
                            Detalle = reader.GetString(nameof(p.Detalle)),
                            Tipo = reader.GetString(nameof(p.Tipo)),
                            Estado = reader.GetString(nameof(p.Estado))
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }

            return res;
        }


        public Pago ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Pago> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}