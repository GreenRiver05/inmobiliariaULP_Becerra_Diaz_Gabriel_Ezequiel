
using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliariaBD.Models
{
  public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Contrato c)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Contrato (InquilinoId, InmuebleId, Monto, Desde, Hasta, Estado)
                             VALUES (@inquilinoId, @inmuebleId, @monto, @desde, @hasta, @estado);
                             SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@inquilinoId", c.InquilinoId);
                    command.Parameters.AddWithValue("@inmuebleId", c.InmuebleId);
                    command.Parameters.AddWithValue("@monto", c.Monto);
                    command.Parameters.AddWithValue("@desde", c.Desde);
                    command.Parameters.AddWithValue("@hasta", c.Hasta);
                    command.Parameters.AddWithValue("@estado", c.Estado);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarEstado(Contrato p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Contrato
                             SET Estado=@estado
                             WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@id", p.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        
        public int Modificacion(Contrato p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Contrato
                             SET InquilinoId=@inquilinoId,
                                 InmuebleId=@inmuebleId,
                                 Monto=@monto,
                                 Desde=@desde,
                                 Hasta=@hasta,
                                 Estado=@estado
                             WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@inquilinoId", p.InquilinoId);
                    command.Parameters.AddWithValue("@inmuebleId", p.InmuebleId);
                    command.Parameters.AddWithValue("@monto", p.Monto);
                    command.Parameters.AddWithValue("@desde", p.Desde);
                    command.Parameters.AddWithValue("@hasta", p.Hasta);
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@id", p.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }


        public IList<Contrato> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public Contrato ObtenerPorId(int id)
        {
            Contrato? c = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, InquilinoId, InmuebleId, Monto, Desde, Hasta, Estado
                             FROM Contrato
                             WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            c = new Contrato
                            {
                                Id = reader.GetInt32(nameof(c.Id)),
                                InquilinoId = reader.GetInt32(nameof(c.InquilinoId)),
                                InmuebleId = reader.GetInt32(nameof(c.InmuebleId)),
                                Monto = reader.GetDecimal(nameof(c.Monto)),
                                Desde = reader.GetDateTime(nameof(c.Desde)),
                                Hasta = reader.GetDateTime(nameof(c.Hasta)),
                                Estado = reader.GetString(nameof(c.Estado))
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return c;
        }

        public IList<Contrato> BuscarPorEstado(string estado)
        {
            throw new NotImplementedException();
        }

        public IList<Contrato> BuscarPorInmueble(int inmuebleId)
        {
            throw new NotImplementedException();
        }

        public IList<Contrato> BuscarPorInquilino(int inquilinoId)
        {
            throw new NotImplementedException();
        }

        public IList<Contrato> BuscarVigentes()
        {
            throw new NotImplementedException();
        }

    }
}