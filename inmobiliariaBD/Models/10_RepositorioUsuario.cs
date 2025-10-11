
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
            throw new NotImplementedException();
        }

        public int Baja(Usuario p)
        {
            throw new NotImplementedException();
        }

        public IList<Usuario> BuscarPorNombre(string nombre)
        {
            throw new NotImplementedException();
        }

        public int Modificacion(Usuario p)
        {
            throw new NotImplementedException();
        }

        public int ModificarEstado(Usuario p)
        {
            throw new NotImplementedException();
        }

        public int ObtenerCantidad(string? busqueda, bool? estado)
        {
            throw new NotImplementedException();
        }

        public IList<Usuario> ObtenerPaginados(int pagina, int cantidadPorPagina, string? busqueda, bool? estado)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IList<Usuario> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}