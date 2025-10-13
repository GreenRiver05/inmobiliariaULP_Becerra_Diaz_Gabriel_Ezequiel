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
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO multa (contrato_id, fechaAviso, fechaTerminacion, monto)
                       VALUES (@contratoId, @fechaAviso, @fechaTerminacion, @monto);
                       SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@contratoId", p.ContratoId);
                    command.Parameters.AddWithValue("@fechaAviso", p.FechaAviso);
                    command.Parameters.AddWithValue("@fechaTerminacion", p.FechaTerminacion);
                    command.Parameters.AddWithValue("@monto", p.Monto);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(Multa p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM multa WHERE id = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", p.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Multa p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE multa SET contrato_id = @contratoId, fechaAviso = @fechaAviso,
                       fechaTerminacion = @fechaTerminacion, monto = @monto WHERE id = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@contratoId", p.ContratoId);
                    command.Parameters.AddWithValue("@fechaAviso", p.FechaAviso);
                    command.Parameters.AddWithValue("@fechaTerminacion", p.FechaTerminacion);
                    command.Parameters.AddWithValue("@monto", p.Monto);
                    command.Parameters.AddWithValue("@id", p.Id);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarEstado(Multa p)
        {
            throw new NotImplementedException();
        }

        public int ObtenerCantidad(string? busqueda,
                                    bool? estado,
                                    DateTime? desde,
                                    DateTime? hasta,
                                    string? estadoPago)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                    filtros.Add("(pe.nombre LIKE @busqueda OR pe.apellido LIKE @busqueda OR c.id LIKE @busqueda)");
                if (desde.HasValue)
                    filtros.Add("m.fechaAviso >= @desde");
                if (hasta.HasValue)
                    filtros.Add("m.fechaTerminacion <= @hasta");
                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = $@"
                             SELECT COUNT(*)
                             FROM multa m
                             JOIN contrato c ON c.id = m.contrato_id
                             JOIN inquilino inq ON inq.id = c.inquilino_id
                             JOIN persona pe ON pe.dni = inq.dni
                             JOIN inmueble i ON i.id = c.inmueble_id
                             {where}";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(busqueda))
                        command.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");
                    if (desde.HasValue)
                        command.Parameters.AddWithValue("@desde", desde.Value);
                    if (hasta.HasValue)
                        command.Parameters.AddWithValue("@hasta", hasta.Value);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
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

        public IList<Multa> ObtenerPaginados(int pagina,
                                             int cantidadPorPagina,
                                             string? busqueda,
                                             bool? estado,
                                             DateTime? desde,
                                             DateTime? hasta,
                                             string? estadoPago)
        {
            var lista = new List<Multa>();
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                    filtros.Add("(pe.nombre LIKE @busqueda OR pe.apellido LIKE @busqueda OR c.id LIKE @busqueda)");
                if (desde.HasValue)
                    filtros.Add("m.fechaAviso >= @desde");
                if (hasta.HasValue)
                    filtros.Add("m.fechaTerminacion <= @hasta");

                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = $@"
                             SELECT m.id, m.contrato_id AS contratoId, m.fechaAviso, m.fechaTerminacion, m.monto,
                                pe.nombre, pe.apellido, i.direccion
                             FROM multa m
                             JOIN contrato c ON c.id = m.contrato_id
                             JOIN inquilino inq ON inq.id = c.inquilino_id
                             JOIN persona pe ON pe.dni = inq.dni
                             JOIN inmueble i ON i.id = c.inmueble_id
                             {where}
                             ORDER BY m.fechaAviso DESC
                             LIMIT @cantidad OFFSET @offset";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@cantidad", cantidadPorPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * cantidadPorPagina);
                    if (!string.IsNullOrEmpty(busqueda))
                        command.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");
                    if (desde.HasValue)
                        command.Parameters.AddWithValue("@desde", desde.Value);
                    if (hasta.HasValue)
                        command.Parameters.AddWithValue("@hasta", hasta.Value);

                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var m = new Multa
                        {
                            Id = reader.GetInt32(nameof(Multa.Id)),
                            ContratoId = reader.GetInt32(nameof(Multa.ContratoId)),
                            FechaAviso = reader.GetDateTime(nameof(Multa.FechaAviso)),
                            FechaTerminacion = reader.GetDateTime(nameof(Multa.FechaTerminacion)),
                            Monto = reader.GetString(nameof(Multa.Monto)),
                            Contrato = new Contrato
                            {
                                Inquilino = new Inquilino
                                {
                                    Persona = new Persona
                                    {
                                        Nombre = reader.GetString("nombre"),
                                        Apellido = reader.GetString("apellido")
                                    }
                                },
                                Inmueble = new Inmueble
                                {
                                    Direccion = reader.GetString("direccion")
                                }
                            }
                        };
                        lista.Add(m);
                    }
                    connection.Close();
                }
            }
            return lista;
        }

        public Multa ObtenerPorId(int id)
        {
            Multa? m = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id, contrato_id AS contratoId, fechaAviso, fechaTerminacion, monto
                       FROM multa WHERE id = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        m = new Multa
                        {
                            Id = reader.GetInt32(nameof(Multa.Id)),
                            ContratoId = reader.GetInt32(nameof(Multa.ContratoId)),
                            FechaAviso = reader.GetDateTime(nameof(Multa.FechaAviso)),
                            FechaTerminacion = reader.GetDateTime(nameof(Multa.FechaTerminacion)),
                            Monto = reader.GetString(nameof(Multa.Monto))
                        };
                    }
                    connection.Close();
                }
            }
            return m!;
        }


        public IList<Multa> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}