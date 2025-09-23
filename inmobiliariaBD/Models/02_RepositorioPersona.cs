using MySql.Data.MySqlClient;
using System.Data;


namespace inmobiliariaBD.Models
{

    public class RepositorioPersona : RepositorioBase, IRepositorioPersona
    {
        public RepositorioPersona(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Persona p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Persona (Dni, Nombre, Apellido, Direccion, Localidad, Correo, Telefono, Estado)
                             VALUES (@dni, @nombre, @apellido, @direccion, @localidad, @correo, @telefono, true);";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@direccion", p.Direccion == null ? DBNull.Value : p.Direccion);
                    command.Parameters.AddWithValue("@localidad", p.Localidad == null ? DBNull.Value : p.Localidad);
                    command.Parameters.AddWithValue("@correo", p.Correo == null ? DBNull.Value : p.Correo);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarEstado(Persona p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Persona
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

        public int Modificar(Persona p, int dniAnterior)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Persona
                             SET Dni=@dni, Nombre=@nombre, Apellido=@apellido, Direccion=@direccion, Localidad=@localidad, Correo=@correo, Telefono=@telefono
                             WHERE Dni=@dniAnterior";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dniAnterior", dniAnterior);
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@direccion", p.Direccion == null ? DBNull.Value : p.Direccion);
                    command.Parameters.AddWithValue("@localidad", p.Localidad == null ? DBNull.Value : p.Localidad);
                    command.Parameters.AddWithValue("@correo", p.Correo == null ? DBNull.Value : p.Correo);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Persona> ObtenerTodos()
        {
            IList<Persona> res = new List<Persona>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Dni, Nombre, Apellido, Direccion, Localidad, Correo, Telefono, Estado
                             FROM Persona";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Persona p = new Persona
                        {
                            Dni = reader.GetInt32(nameof(p.Dni)),
                            Nombre = reader.GetString(nameof(p.Nombre)),
                            Apellido = reader.GetString(nameof(p.Apellido)),
                            Direccion = reader.IsDBNull(nameof(p.Direccion)) ? null : reader.GetString(nameof(p.Direccion)),
                            Localidad = reader.IsDBNull(nameof(p.Localidad)) ? null : reader.GetString(nameof(p.Localidad)),
                            Correo = reader.IsDBNull(nameof(p.Correo)) ? null : reader.GetString(nameof(p.Correo)),
                            Telefono = reader.GetInt64(nameof(p.Telefono)),
                            Estado = reader.GetBoolean(nameof(p.Estado))

                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Persona ObtenerPorDni(int dni)
        {
            Persona p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Dni, Nombre, Apellido, Direccion, Localidad, Correo, Telefono, Estado
                             FROM Persona 
                             WHERE Dni=@dni";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", dni);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Persona
                        {
                            Dni = reader.GetInt32(nameof(p.Dni)),
                            Nombre = reader.GetString(nameof(p.Nombre)),
                            Apellido = reader.GetString(nameof(p.Apellido)),
                            Direccion = reader.IsDBNull(nameof(p.Direccion)) ? null : reader.GetString(nameof(p.Direccion)),
                            Localidad = reader.IsDBNull(nameof(p.Localidad)) ? null : reader.GetString(nameof(p.Localidad)),
                            Correo = reader.IsDBNull(nameof(p.Correo)) ? null : reader.GetString(nameof(p.Correo)),
                            Telefono = reader.GetInt64(nameof(p.Telefono)),
                            Estado = reader.GetBoolean(nameof(p.Estado))
                        };
                    }

                    connection.Close();
                }
            }
            return p;
        }

        public int Baja(Persona p)
        {
            throw new NotImplementedException();
        }

        public IList<Persona> BuscarPorNombre(string nombre)
        {
            throw new NotImplementedException();
        }

        public Persona ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public int Modificacion(Persona p)
        {
            throw new NotImplementedException();
        }

        public IList<Persona> ObtenerPaginados(int pagina, int cantidadPorPagina)
        {
            throw new NotImplementedException();
        }

        public int ObtenerCantidad()
        {
            throw new NotImplementedException();
        }
    }
}