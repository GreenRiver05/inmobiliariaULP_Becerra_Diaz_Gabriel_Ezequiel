using System.Data;
using MySql.Data.MySqlClient;


namespace inmobiliariaBD.Models
{

    public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Propietario (Dni, Estado)
                             VALUES (@dni, true);
                             SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);


                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM Propietario 
                             WHERE id=@id;";
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

        public int ModificarEstado(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Propietario
                             SET Estado=@estado
                             WHERE Dni=@dni";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Propietario
                             SET Estado=@estado, Dni=@dni
                             WHERE Dni=@dni";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@estado", p.Estado);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.id, pe.nombre, pe.apellido, p.dni, pe.direccion, pe.localidad, pe.correo, pe.telefono, p.estado
                             FROM propietario as p
                             JOIN persona pe ON pe.dni = p.dni";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(nameof(p.Id)),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(nameof(p.Persona.Nombre)),
                                Apellido = reader.GetString(nameof(p.Persona.Apellido)),
                                Direccion = reader.IsDBNull(nameof(p.Persona.Direccion)) ? null : reader.GetString(nameof(p.Persona.Direccion)),
                                Localidad = reader.IsDBNull(nameof(p.Persona.Localidad)) ? null : reader.GetString(nameof(p.Persona.Localidad)),
                                Correo = reader.IsDBNull(nameof(p.Persona.Correo)) ? null : reader.GetString(nameof(p.Persona.Correo)),
                                Telefono = reader.GetInt64(nameof(p.Persona.Telefono))
                            },
                            Dni = reader.GetInt32(nameof(p.Dni)),
                            Estado = reader.GetBoolean(nameof(p.Estado))
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Propietario ObtenerPorId(int id)
        {
            Propietario? p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.id, p.dni, p.estado, pe.nombre, pe.apellido, pe.direccion, pe.localidad, pe.correo, pe.telefono
                             FROM propietario p 
                             JOIN persona pe ON pe.dni = p.dni
                             WHERE p.id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            Id = reader.GetInt32(nameof(p.Id)),
                            Dni = reader.GetInt32(nameof(p.Dni)),
                            Estado = reader.GetBoolean(nameof(p.Estado)),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(nameof(p.Persona.Nombre)),
                                Apellido = reader.GetString(nameof(p.Persona.Apellido)),
                                Direccion = reader.IsDBNull(nameof(p.Persona.Direccion)) ? null : reader.GetString(nameof(p.Persona.Direccion)),
                                Localidad = reader.IsDBNull(nameof(p.Persona.Localidad)) ? null : reader.GetString(nameof(p.Persona.Localidad)),
                                Correo = reader.IsDBNull(nameof(p.Persona.Correo)) ? null : reader.GetString(nameof(p.Persona.Correo)),
                                Telefono = reader.GetInt64(nameof(p.Persona.Telefono))
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public Propietario ObtenerPorDni(int dni)
        {
            Propietario? p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.id, p.dni, p.estado, pe.nombre, pe.apellido, pe.direccion, pe.localidad, pe.correo, pe.telefono
                             FROM propietario p 
                             JOIN persona pe ON pe.dni = p.dni
                             WHERE p.dni = @dni";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", dni);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            Id = reader.GetInt32(nameof(p.Id)),
                            Dni = reader.GetInt32(nameof(p.Dni)),
                            Estado = reader.GetBoolean(nameof(p.Estado)),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(nameof(p.Persona.Nombre)),
                                Apellido = reader.GetString(nameof(p.Persona.Apellido)),
                                Direccion = reader.IsDBNull(nameof(p.Persona.Direccion)) ? null : reader.GetString(nameof(p.Persona.Direccion)),
                                Localidad = reader.IsDBNull(nameof(p.Persona.Localidad)) ? null : reader.GetString(nameof(p.Persona.Localidad)),
                                Correo = reader.IsDBNull(nameof(p.Persona.Correo)) ? null : reader.GetString(nameof(p.Persona.Correo)),
                                Telefono = reader.GetInt64(nameof(p.Persona.Telefono))
                            },
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Propietario> BuscarPorNombre(string nombre)
        {
            throw new NotImplementedException();
        }

        public IList<Propietario> ObtenerPaginados(int pagina, int cantidadPorPagina)
        {
            var lista = new List<Propietario>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.id, pe.nombre, pe.apellido, p.dni, pe.direccion, pe.localidad, pe.correo, pe.telefono, p.estado
                             FROM propietario as p
                             JOIN persona pe ON pe.dni = p.dni
                             LIMIT @cantidad OFFSET @offset";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@cantidad", cantidadPorPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * cantidadPorPagina);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(nameof(p.Id)),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(nameof(p.Persona.Nombre)),
                                Apellido = reader.GetString(nameof(p.Persona.Apellido)),
                                Direccion = reader.IsDBNull(nameof(p.Persona.Direccion)) ? null : reader.GetString(nameof(p.Persona.Direccion)),
                                Localidad = reader.IsDBNull(nameof(p.Persona.Localidad)) ? null : reader.GetString(nameof(p.Persona.Localidad)),
                                Correo = reader.IsDBNull(nameof(p.Persona.Correo)) ? null : reader.GetString(nameof(p.Persona.Correo)),
                                Telefono = reader.GetInt64(nameof(p.Persona.Telefono))
                            },
                            Dni = reader.GetInt32(nameof(p.Dni)),
                            Estado = reader.GetBoolean(nameof(p.Estado))
                        };
                        lista.Add(p);
                    }
                }
            }
            return lista;
        }

        public int ObtenerCantidad()
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM Propietario";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = reader.GetInt32(0);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}