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
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO pago (contrato_id, numero_pago, monto, fecha, detalle, tipo, estado)
                       VALUES (@contratoId, @numeroPago, @monto, @fecha, @detalle, @tipo, @estado);
                       SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@contratoId", p.ContratoId);
                    command.Parameters.AddWithValue("@numeroPago", p.NumeroPago);
                    command.Parameters.AddWithValue("@monto", p.Monto);
                    command.Parameters.AddWithValue("@fecha", p.Fecha);
                    command.Parameters.AddWithValue("@detalle", p.Detalle);
                    command.Parameters.AddWithValue("@tipo", p.Tipo);
                    command.Parameters.AddWithValue("@estado", p.Estado);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(Pago p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM pago WHERE id = @id";

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

        public int Modificacion(Pago p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE pago SET contrato_id = @contratoId, numero_pago = @numeroPago, monto = @monto,
                       fecha = @fecha, detalle = @detalle, tipo = @tipo, estado = @estado WHERE id = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@contratoId", p.ContratoId);
                    command.Parameters.AddWithValue("@numeroPago", p.NumeroPago);
                    command.Parameters.AddWithValue("@monto", p.Monto);
                    command.Parameters.AddWithValue("@fecha", p.Fecha);
                    command.Parameters.AddWithValue("@detalle", p.Detalle);
                    command.Parameters.AddWithValue("@tipo", p.Tipo);
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@id", p.Id);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarEstado(Pago p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "UPDATE pago SET estado = @estado WHERE id = @id";

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

        public int ObtenerCantidad(string? busqueda = null,
                                   bool? estado = null,
                                   DateTime? desde = null,
                                   DateTime? hasta = null,
                                   string? estadoPago = null)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                    filtros.Add("(pe.nombre LIKE @busqueda OR pe.apellido LIKE @busqueda OR p.id LIKE @busqueda)");
                if (!string.IsNullOrEmpty(estadoPago))
                    filtros.Add("p.estado = @estadoPago");
                if (desde.HasValue)
                    filtros.Add("p.fecha >= @desde");
                if (hasta.HasValue)
                    filtros.Add("p.fecha <= @hasta");


                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = $@"
                             SELECT COUNT(*)
                             FROM pago p
                             JOIN contrato c ON c.id = p.contrato_id
                             JOIN inquilino inq ON inq.id = c.inquilino_id
                             JOIN persona pe ON pe.dni = inq.dni
                             JOIN inmueble i ON i.id = c.inmueble_id
                             {where}";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(busqueda))
                        command.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");
                    if (!string.IsNullOrEmpty(estadoPago))
                        command.Parameters.AddWithValue("@estadoPago", estadoPago);
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

        public IList<Pago> ObtenerPaginados(int pagina,
                                            int cantidadPorPagina,
                                            string? busqueda = null,
                                            bool? estado = null,
                                            DateTime? desde = null,
                                            DateTime? hasta = null,
                                            string? estadoPago = null)
        {
            var lista = new List<Pago>();
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                    filtros.Add("(pe.nombre LIKE @busqueda OR pe.apellido LIKE @busqueda OR p.id LIKE @busqueda)");
                if (!string.IsNullOrEmpty(estadoPago))
                    filtros.Add("p.estado = @estadoPago");
                if (desde.HasValue)
                    filtros.Add("p.fecha >= @desde");
                if (hasta.HasValue)
                    filtros.Add("p.fecha <= @hasta");


                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = $@"
                             SELECT p.id, p.contrato_id AS contratoId, p.numero_pago AS numeroPago, p.monto, p.fecha, p.detalle, p.tipo, p.estado,
                               pe.nombre, pe.apellido, i.direccion
                             FROM pago p
                             JOIN contrato c ON c.id = p.contrato_id
                             JOIN inquilino inq ON inq.id = c.inquilino_id
                             JOIN persona pe ON pe.dni = inq.dni
                             JOIN inmueble i ON i.id = c.inmueble_id
                             {where}
                             ORDER BY p.fecha DESC
                             LIMIT @cantidad OFFSET @offset";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@cantidad", cantidadPorPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * cantidadPorPagina);
                    if (!string.IsNullOrEmpty(busqueda))
                        command.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");
                    if (!string.IsNullOrEmpty(estadoPago))
                        command.Parameters.AddWithValue("@estadoPago", estadoPago);

                    if (desde.HasValue)
                        command.Parameters.AddWithValue("@desde", desde.Value);
                    if (hasta.HasValue)
                        command.Parameters.AddWithValue("@hasta", hasta.Value);

                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var p = new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                            Monto = reader.GetString(nameof(Pago.Monto)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Detalle = reader.GetString(nameof(Pago.Detalle)),
                            Tipo = reader.GetString(nameof(Pago.Tipo)),
                            Estado = reader.GetString(nameof(Pago.Estado)),
                            Contrato = new Contrato
                            {
                                Inquilino = new Inquilino
                                {
                                    Persona = new Persona
                                    {
                                        Nombre = reader.GetString(nameof(Persona.Nombre)),
                                        Apellido = reader.GetString(nameof(Persona.Apellido))
                                    }
                                },
                                Inmueble = new Inmueble
                                {
                                    Direccion = reader.GetString("direccion")
                                }
                            }
                        };
                        lista.Add(p);
                    }
                    connection.Close();
                }
            }
            return lista;
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
            Pago? p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id, contrato_id AS contratoId, numero_pago AS numeroPago, monto, fecha, detalle, tipo, estado
                       FROM pago WHERE id = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                            Monto = reader.GetString(nameof(Pago.Monto)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Detalle = reader.GetString(nameof(Pago.Detalle)),
                            Tipo = reader.GetString(nameof(Pago.Tipo)),
                            Estado = reader.GetString(nameof(Pago.Estado))
                        };
                    }
                    connection.Close();
                }
            }
            return p!;
        }

        public IList<Pago> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}