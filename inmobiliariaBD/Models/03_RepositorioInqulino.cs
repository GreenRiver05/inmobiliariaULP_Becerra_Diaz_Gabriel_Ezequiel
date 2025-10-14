using MySql.Data.MySqlClient;
using System.Data;


namespace inmobiliariaBD.Models
{
    public class RepositorioInquilino : RepositorioBase, IRepositorioInquilino
    {
        public RepositorioInquilino(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Inquilino i)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inquilino(Dni, Estado)
                             VALUES(@dni, true);
                             SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", i.Dni);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(Inquilino i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM Inquilino 
                             WHERE id=@id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", i.Id);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarEstado(Inquilino i)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inquilino
                             SET Estado=@estado
                             WHERE Dni=@dni";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@estado", i.Estado);
                    command.Parameters.AddWithValue("@dni", i.Dni);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inquilino i)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inquilino
                             SET Estado=@estado, Dni=@dni
                             WHERE Dni=@dni";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", i.Dni);
                    command.Parameters.AddWithValue("@estado", i.Estado);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inquilino> ObtenerTodos()
        {
            IList<Inquilino> res = new List<Inquilino>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id, pe.nombre, pe.apellido, i.dni, pe.direccion, pe.localidad, pe.correo, pe.telefono, i.estado
                             FROM inquilino as i
                             JOIN persona pe ON pe.dni = i.dni";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    {
                        while (reader.Read())
                        {
                            Inquilino i = new Inquilino
                            {
                                Id = reader.GetInt32(nameof(i.Id)),
                                Dni = reader.GetInt32(nameof(i.Dni)),
                                Estado = reader.GetBoolean(nameof(i.Estado)),
                                Persona = new Persona
                                {
                                    Nombre = reader.GetString(nameof(i.Persona.Nombre)),
                                    Apellido = reader.GetString(nameof(i.Persona.Apellido)),
                                    Direccion = reader.IsDBNull(nameof(i.Persona.Direccion)) ? null : reader.GetString(nameof(i.Persona.Direccion)),
                                    Localidad = reader.IsDBNull(nameof(i.Persona.Localidad)) ? null : reader.GetString(nameof(i.Persona.Localidad)),
                                    Correo = reader.IsDBNull(nameof(i.Persona.Correo)) ? null : reader.GetString(nameof(i.Persona.Correo)),
                                    Telefono = reader.GetInt64(nameof(i.Persona.Telefono)),
                                    Dni = reader.GetInt32(nameof(i.Dni))
                                }
                            };
                            res.Add(i);
                        }
                    }
                    connection.Close();
                }
            }
            return res;


        }

        public Inquilino ObtenerPorId(int id)
        {
            Inquilino i = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id, i.dni, i.estado, pe.nombre, pe.apellido, pe.direccion, pe.localidad, pe.correo, pe.telefono
                             FROM inquilino i
                             JOIN persona pe ON pe.dni = i.dni
                             WHERE i.id = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    {
                        if (reader.Read())
                        {
                            i = new Inquilino
                            {
                                Id = reader.GetInt32(nameof(i.Id)),
                                Dni = reader.GetInt32(nameof(i.Dni)),
                                Estado = reader.GetBoolean(nameof(i.Estado)),
                                Persona = new Persona
                                {
                                    Nombre = reader.GetString(nameof(i.Persona.Nombre)),
                                    Apellido = reader.GetString(nameof(i.Persona.Apellido)),
                                    Direccion = reader.IsDBNull(nameof(i.Persona.Direccion)) ? null : reader.GetString(nameof(i.Persona.Direccion)),
                                    Localidad = reader.IsDBNull(nameof(i.Persona.Localidad)) ? null : reader.GetString(nameof(i.Persona.Localidad)),
                                    Correo = reader.IsDBNull(nameof(i.Persona.Correo)) ? null : reader.GetString(nameof(i.Persona.Correo)),
                                    Telefono = reader.GetInt64(nameof(i.Persona.Telefono))
                                }
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return i;
        }

        Inquilino IRepositorioInquilino.ObtenerPorDni(int dni)
        {
            Inquilino i = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id, i.dni, i.estado, pe.nombre, pe.apellido, pe.direccion, pe.localidad, pe.correo, pe.telefono
                             FROM inquilino i
                             JOIN persona pe ON pe.dni = i.dni
                             WHERE i.dni = @dni";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", dni);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    {
                        if (reader.Read())
                        {
                            i = new Inquilino
                            {
                                Id = reader.GetInt32(nameof(i.Id)),
                                Dni = reader.GetInt32(nameof(i.Dni)),
                                Estado = reader.GetBoolean(nameof(i.Estado)),
                                Persona = new Persona
                                {
                                    Nombre = reader.GetString(nameof(i.Persona.Nombre)),
                                    Apellido = reader.GetString(nameof(i.Persona.Apellido)),
                                    Direccion = reader.IsDBNull(nameof(i.Persona.Direccion)) ? null : reader.GetString(nameof(i.Persona.Direccion)),
                                    Localidad = reader.IsDBNull(nameof(i.Persona.Localidad)) ? null : reader.GetString(nameof(i.Persona.Localidad)),
                                    Correo = reader.IsDBNull(nameof(i.Persona.Correo)) ? null : reader.GetString(nameof(i.Persona.Correo)),
                                    Telefono = reader.GetInt64(nameof(i.Persona.Telefono))
                                }
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public IList<Inquilino> Buscar(string nombre)
        {
            var res = new List<Inquilino>();
            nombre = "%" + nombre + "%";
            Inquilino? i = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id, i.dni, i.estado, pe.nombre, pe.apellido, pe.direccion, pe.localidad, pe.correo, pe.telefono
                        FROM inquilino i
                        JOIN persona pe ON pe.dni = i.dni
                        WHERE pe.nombre LIKE @nombre OR pe.apellido LIKE @nombre OR pe.dni LIKE @nombre
                        ORDER BY pe.apellido ASC
                        LIMIT 10";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", nombre);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        i = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(i.Id)),
                            Dni = reader.GetInt32(nameof(i.Dni)),
                            Estado = reader.GetBoolean(nameof(i.Estado)),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(nameof(i.Persona.Nombre)),
                                Apellido = reader.GetString(nameof(i.Persona.Apellido)),
                                Direccion = reader.IsDBNull(nameof(i.Persona.Direccion)) ? null : reader.GetString(nameof(i.Persona.Direccion)),
                                Localidad = reader.IsDBNull(nameof(i.Persona.Localidad)) ? null : reader.GetString(nameof(i.Persona.Localidad)),
                                Correo = reader.IsDBNull(nameof(i.Persona.Correo)) ? null : reader.GetString(nameof(i.Persona.Correo)),
                                Telefono = reader.GetInt64(nameof(i.Persona.Telefono))
                            }
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inquilino> ObtenerPaginados(int pagina, int cantidadPorPagina, string? busqueda = null, bool? estado = null, DateTime? desde = null, DateTime? hasta = null, string? estadoPago = null)
        {
            var lista = new List<Inquilino>();
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                    filtros.Add("(pe.nombre LIKE @busqueda OR pe.apellido LIKE @busqueda)");
                if (estado.HasValue)
                    filtros.Add("i.estado = @estado");

                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = $@"
                             SELECT i.id, i.dni, i.estado, pe.nombre, pe.apellido, pe.direccion, pe.localidad, pe.correo, pe.telefono
                             FROM inquilino i
                             JOIN persona pe ON pe.dni = i.dni
                             {where}
                             ORDER BY pe.apellido
                             LIMIT @cantidad OFFSET @offset";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@cantidad", cantidadPorPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * cantidadPorPagina);
                    if (!string.IsNullOrEmpty(busqueda))
                        command.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");
                    if (estado.HasValue)
                        command.Parameters.AddWithValue("@estado", estado.Value);

                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Dni = reader.GetInt32(nameof(Inquilino.Dni)),
                            Estado = reader.GetBoolean(nameof(Inquilino.Estado)),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Persona.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Persona.Apellido)),
                                Direccion = reader.IsDBNull(nameof(Inquilino.Persona.Direccion)) ? null : reader.GetString(nameof(Inquilino.Persona.Direccion)),
                                Localidad = reader.IsDBNull(nameof(Inquilino.Persona.Localidad)) ? null : reader.GetString(nameof(Inquilino.Persona.Localidad)),
                                Correo = reader.IsDBNull(nameof(Inquilino.Persona.Correo)) ? null : reader.GetString(nameof(Inquilino.Persona.Correo)),
                                Telefono = reader.GetInt64(nameof(Inquilino.Persona.Telefono))
                            }
                        };
                        lista.Add(i);
                    }
                }
            }
            return lista;
        }

        public int ObtenerCantidad(string? busqueda = null, bool? estado = null, DateTime? desde = null, DateTime? hasta = null, string? estadoPago = null)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                    filtros.Add("(pe.nombre LIKE @busqueda OR pe.apellido LIKE @busqueda)");
                if (estado.HasValue)
                    filtros.Add("i.estado = @estado");

                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = $@"
                             SELECT COUNT(*) 
                             FROM inquilino i
                             JOIN persona pe ON pe.dni = i.dni
                             {where}";

                using (var command = new MySqlCommand(sql, connection))
                {
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
                }
            }
            return res;
        }


    }

}