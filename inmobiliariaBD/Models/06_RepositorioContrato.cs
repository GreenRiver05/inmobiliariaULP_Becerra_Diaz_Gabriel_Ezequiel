
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
                string sql = @"INSERT INTO Contrato (Inquilino_Id, Inmueble_Id, Monto, Desde, Hasta, Estado)
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
                             SET Inquilino_Id=@inquilinoId,
                                 Inmueble_Id=@inmuebleId,
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

        public int Baja(Contrato c)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM Contrato
                             WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", c.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();

                }
                return res;
            }
        }
       
        public IList<Contrato> ObtenerTodos()
        {
            IList<Contrato> res = new List<Contrato>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"
            SELECT c.id, c.inquilino_Id AS InquilinoID, c.inmueble_Id AS InmuebleId, c.monto, c.desde, c.hasta, c.estado,
                   pe.nombre, pe.apellido, pe.telefono, pe.dni, i.direccion
            FROM contrato c
            JOIN inquilino inq ON inq.id = c.inquilino_Id
            JOIN persona pe ON pe.dni = inq.dni
            JOIN inmueble i ON i.id = c.inmueble_Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato c = new Contrato
                        {
                            Id = reader.GetInt32(nameof(c.Id)),
                            InquilinoId = reader.GetInt32(nameof(c.InquilinoId)),
                            InmuebleId = reader.GetInt32(nameof(c.InmuebleId)),
                            Monto = reader.GetString(nameof(c.Monto)),
                            Desde = reader.GetDateTime(nameof(c.Desde)),
                            Hasta = reader.GetDateTime(nameof(c.Hasta)),
                            Estado = reader.GetString(nameof(c.Estado)),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(nameof(c.InquilinoId)),
                                Persona = new Persona
                                {
                                    Nombre = reader.GetString(nameof(c.Inquilino.Persona.Nombre)),
                                    Apellido = reader.GetString(nameof(c.Inquilino.Persona.Apellido)),
                                    Telefono = reader.GetInt64(nameof(c.Inquilino.Persona.Telefono)),
                                    Dni = reader.GetInt32(nameof(c.Inquilino.Persona.Dni))
                                }
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(nameof(c.InmuebleId)),
                                Direccion = reader.GetString(nameof(c.Inmueble.Direccion))
                            }
                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Contrato ObtenerPorId(int id)
        {
            Contrato? c = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Inquilino_id AS InquilinoId, Inmueble_Id AS InmuebleId, Monto, Desde, Hasta, Estado
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
                                Monto = reader.GetString(nameof(c.Monto)),
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