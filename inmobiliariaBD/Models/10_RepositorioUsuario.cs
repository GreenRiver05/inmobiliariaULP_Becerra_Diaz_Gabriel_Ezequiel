
using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliariaBD.Models
{
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Usuario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO usuario (dni, contraseña, rol, avatar, estado)
                             VALUES (@dni, @clave, @rol, @avatar, @estado);
                             SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@clave", p.Contraseña);
                    command.Parameters.AddWithValue("@rol", p.Rol);
                    command.Parameters.AddWithValue("@avatar", p.Avatar ?? "");
                    command.Parameters.AddWithValue("@estado", p.Estado);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(Usuario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM usuario
                             WHERE id = @id";
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

        public int Modificacion(Usuario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE usuario SET
                             contraseña = @clave,
                             rol = @rol,
                             avatar = @avatar
                             WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", p.Id);
                    command.Parameters.AddWithValue("@clave", p.Contraseña);
                    command.Parameters.AddWithValue("@rol", p.Rol);
                    command.Parameters.AddWithValue("@avatar", p.Avatar ?? "");

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarEstado(Usuario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE usuario
                             SET estado = @estado
                             WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", p.Id);
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Usuario ObtenerPorEmail(string email)
        {
            Usuario? u = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT u.id, u.dni, u.contraseña, u.rol, u.avatar, u.estado AS UsuarioEstado,
                              p.nombre, p.apellido, p.direccion, p.localidad, p.correo, p.telefono, p.estado AS PersonaEstado
                             FROM usuario u
                             JOIN persona p ON p.dni = u.dni
                             WHERE p.correo = @correo";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@correo", email);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        u = new Usuario
                        {
                            Id = reader.GetInt32(nameof(u.Id)),
                            Dni = reader.GetInt32(nameof(u.Dni)),
                            Contraseña = reader.GetString(nameof(u.Contraseña)),
                            Rol = reader.GetInt32(nameof(u.Rol)),
                            Avatar = reader.IsDBNull(nameof(u.Avatar)) ? "" : reader.GetString(nameof(u.Avatar)),
                            Estado = reader.GetBoolean("UsuarioEstado"),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(nameof(u.Persona.Nombre)),
                                Apellido = reader.GetString(nameof(u.Persona.Apellido)),
                                Direccion = reader.IsDBNull(nameof(u.Persona.Direccion)) ? null : reader.GetString(nameof(u.Persona.Direccion)),
                                Localidad = reader.IsDBNull(nameof(u.Persona.Localidad)) ? null : reader.GetString(nameof(u.Persona.Localidad)),
                                Correo = reader.GetString(nameof(u.Persona.Correo)),
                                Telefono = reader.GetInt64(nameof(u.Persona.Telefono)),
                                Estado = reader.GetBoolean("PersonaEstado")
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return u;
        }

        public Usuario ObtenerPorId(int id)
        {
            Usuario? u = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT u.id, u.dni, u.contraseña, u.rol, u.avatar, u.estado AS UsuarioEstado,
                       p.nombre, p.apellido, p.direccion, p.localidad, p.correo, p.telefono, p.estado AS PersonaEstado
                       FROM usuario u
                       JOIN persona p ON p.dni = u.dni
                       WHERE u.id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        u = new Usuario
                        {
                            Id = reader.GetInt32(nameof(u.Id)),
                            Dni = reader.GetInt32(nameof(u.Dni)),
                            Contraseña = reader.GetString(nameof(u.Contraseña)),
                            Rol = reader.GetInt32(nameof(u.Rol)),
                            Avatar = reader.IsDBNull(nameof(u.Avatar)) ? "" : reader.GetString(nameof(u.Avatar)),
                            Estado = reader.GetBoolean("UsuarioEstado"),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(nameof(u.Persona.Nombre)),
                                Apellido = reader.GetString(nameof(u.Persona.Apellido)),
                                Direccion = reader.IsDBNull(nameof(u.Persona.Direccion)) ? null : reader.GetString(nameof(u.Persona.Direccion)),
                                Localidad = reader.IsDBNull(nameof(u.Persona.Localidad)) ? null : reader.GetString(nameof(u.Persona.Localidad)),
                                Correo = reader.GetString(nameof(u.Persona.Correo)),
                                Telefono = reader.GetInt64(nameof(u.Persona.Telefono)),
                                Estado = reader.GetBoolean("PersonaEstado")

                            }
                        };
                    }
                    connection.Close();
                }
            }
            return u!;
        }


        public IList<Usuario> ObtenerTodos()
        {
            throw new NotImplementedException();

        }
        public IList<Usuario> BuscarPorNombre(string nombre)
        {
            throw new NotImplementedException();
        }
        public int ObtenerCantidad(string? busqueda = null, bool? estado = null)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                    filtros.Add("(pe.nombre LIKE @busqueda OR pe.apellido LIKE @busqueda OR pe.correo LIKE @busqueda)");
                if (estado.HasValue)
                    filtros.Add("u.estado = @estado");

                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = $@"
                             SELECT COUNT(*) 
                             FROM usuario u
                             JOIN persona pe ON pe.dni = u.dni
            {where}";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(busqueda))
                        command.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");
                    if (estado.HasValue)
                        command.Parameters.AddWithValue("@estado", estado.Value);

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

        public IList<Usuario> ObtenerPaginados(int pagina, int cantidadPorPagina, string? busqueda = null, bool? estado = null)
        {
            var lista = new List<Usuario>();
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                {
                    filtros.Add("(pe.nombre LIKE @busqueda OR pe.apellido LIKE @busqueda OR pe.correo LIKE @busqueda)");
                }
                if (estado.HasValue)
                {
                    filtros.Add("u.estado = @estado");
                }
                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = @"SELECT u.id, u.dni, u.rol, u.avatar, u.estado AS UsuarioEstado,
                              pe.nombre, pe.apellido, pe.direccion, pe.localidad, pe.correo, pe.telefono, pe.estado AS PersonaEstado
                             FROM usuario u
                             JOIN persona pe ON pe.dni = u.dni
                             " + where + @"
                             ORDER BY pe.apellido
                             LIMIT @cantidad OFFSET @offset";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@cantidad", cantidadPorPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * cantidadPorPagina);
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        command.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");
                    }
                    if (estado.HasValue)
                    {
                        command.Parameters.AddWithValue("@estado", estado.Value);
                    }

                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var u = new Usuario
                        {
                            Id = reader.GetInt32("id"),
                            Dni = reader.GetInt32("dni"),
                            Rol = reader.GetInt32("rol"),
                            Avatar = reader.IsDBNull("avatar") ? "" : reader.GetString("avatar"),
                            Estado = reader.GetBoolean("UsuarioEstado"),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido"),
                                Direccion = reader.IsDBNull("direccion") ? null : reader.GetString("direccion"),
                                Localidad = reader.IsDBNull("localidad") ? null : reader.GetString("localidad"),
                                Correo = reader.GetString("correo"),
                                Telefono = reader.GetInt64("telefono"),
                                Estado = reader.GetBoolean("PersonaEstado")
                            }
                        };
                        lista.Add(u);
                    }
                    connection.Close();
                }
            }
            return lista;
        }

    }
}