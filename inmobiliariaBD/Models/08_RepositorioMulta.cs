using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliariaBD.Models
{
    public class RepositorioMulta : RepositorioBase, IRepositorioMulta
    {
        public RepositorioMulta(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Multa p)
        {
            throw new NotImplementedException();
        }

        public int Baja(Multa p)
        {
            throw new NotImplementedException();
        }

        public int Modificacion(Multa p)
        {
            throw new NotImplementedException();
        }

        public int ModificarEstado(Multa p)
        {
            throw new NotImplementedException();
        }

        public int ObtenerCantidad()
        {
            throw new NotImplementedException();
        }

        public IList<Multa> ObtenerMultasPorContrato(int contratoId)
        {
            IList<Multa> res = new List<Multa>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @" SELECT m.id, m.contrato_id AS contratoId, m.fechaAviso, m.fechaTerminacion, m.monto
                                FROM multa m
                                WHERE m.contrato_id = @contratoId";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@contratoId", contratoId);

                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Multa m = new Multa
                        {
                            Id = reader.GetInt32(nameof(m.Id)),
                            ContratoId = reader.GetInt32(nameof(m.ContratoId)),
                            FechaAviso = reader.GetDateTime(nameof(m.FechaAviso)),
                            FechaTerminacion = reader.GetDateTime(nameof(m.FechaTerminacion)),
                            Monto = reader.GetString(nameof(m.Monto))
                        };
                        res.Add(m);
                    }
                    connection.Close();
                }
            }

            return res;
        }

        public IList<Multa> ObtenerPaginados(int pagina, int cantidadPorPagina)
        {
            throw new NotImplementedException();
        }

        public Multa ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Multa> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}