using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliariaBD.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inmueble (PropietarioId, TipoId, Direccion, Localidad, Longitud, Latitud, Uso, Ambientes, Observacion, Estado, Precio)
                             VALUES (@propietarioId, @tipoId, @direccion, @localidad, @longitud, @latitud, @uso, @ambientes, @observacion, @estado, @precio);
                             SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
                    command.Parameters.AddWithValue("@tipoId", i.TipoId);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@localidad", i.Localidad);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@uso", i.Uso);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@observacion", i.Observacion);
                    command.Parameters.AddWithValue("@estado", i.Estado);
                    command.Parameters.AddWithValue("@precio", i.Precio);


                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarEstado(Inmueble p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inmueble
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

        public int Modificacion(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inmueble
                             SET PropietarioId=@propietarioId, TipoId=@tipoId, Direccion=@direccion, Localidad=@localidad, Longitud=@longitud, Latitud=@latitud, Uso=@uso, Ambientes=@ambientes, Observacion=@observacion, Estado=@estado, Precio=@precio
                             WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
                    command.Parameters.AddWithValue("@tipoId", i.TipoId);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@localidad", i.Localidad);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@uso", i.Uso);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@observacion", i.Observacion);
                    command.Parameters.AddWithValue("@estado", i.Estado);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@id", i.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.propietario_id, i.tipo_id, i.direccion, i.localidad, i.longitud, i.latitud, i.uso, i.ambientes, i.observacion, i.estado, i.precio,
                               p.dni, p.estado, pe.nombre, pe.apellido, pe.direccion, pe.localidad, pe.correo, pe.telefono,
                               t.tipo, t.descripcion
                               FROM Inmueble i
                               INNER JOIN Propietario p ON i.propietario_id = p.id 
                               INNER JOIN Persona pe ON p.dni = pe.dni
                               INNER JOIN tipo_inmueble t ON i.tipo_id = t.id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(i.Id)),
                            PropietarioId = reader.GetInt32(nameof(i.PropietarioId)),
                            TipoId = reader.GetInt32(nameof(i.TipoId)),
                            Direccion = reader.GetString(nameof(i.Direccion)),
                            Localidad = reader.GetString(nameof(i.Localidad)),
                            Longitud = reader.GetDecimal(nameof(i.Longitud)),
                            Latitud = reader.GetDecimal(nameof(i.Latitud)),
                            Uso = reader.GetString(nameof(i.Uso)),
                            Ambientes = reader.GetInt32(nameof(i.Ambientes)),
                            Observacion = reader.IsDBNull(nameof(i.Observacion)) ? null : reader.GetString(nameof(i.Observacion)),
                            Estado = reader.GetString(nameof(i.Estado)),
                            Precio = reader.GetDecimal(nameof(i.Precio)),

                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(nameof(i.PropietarioId)),
                                Dni = reader.GetInt32(nameof(i.Propietario.Dni)),
                                Estado = reader.GetBoolean(nameof(i.Propietario.Estado)),
                                Persona = new Persona
                                {
                                    Nombre = reader.GetString(nameof(i.Propietario.Persona.Nombre)),
                                    Apellido = reader.GetString(nameof(i.Propietario.Persona.Apellido)),
                                    Direccion = reader.IsDBNull(nameof(i.Propietario.Persona.Direccion)) ? null : reader.GetString(nameof(i.Propietario.Persona.Direccion)),
                                    Localidad = reader.IsDBNull(nameof(i.Propietario.Persona.Localidad)) ? null : reader.GetString(nameof(i.Propietario.Persona.Localidad)),
                                    Correo = reader.IsDBNull(nameof(i.Propietario.Persona.Correo)) ? null : reader.GetString(nameof(i.Propietario.Persona.Correo)),
                                    Telefono = reader.GetInt64(nameof(i.Propietario.Persona.Telefono))
                                }
                            },

                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(i.TipoInmueble.Id)),
                                Tipo = reader.GetString(nameof(i.TipoInmueble.Tipo)),
                                Descripcion = reader.IsDBNull(nameof(i.TipoInmueble.Descripcion)) ? null : reader.GetString(nameof(i.TipoInmueble.Descripcion))
                            }
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
                return res;
            }
        }

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble? i = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.PropietarioId, i.TipoId, i.Direccion, i.Localidad, i.Longitud, i.Latitud, i.Uso, i.Ambientes, i.Observacion, i.Estado, i.Precio,
                               p.Dni, p.Estado, pe.Nombre, pe.Apellido, pe.Direccion, pe.Localidad, pe.Correo, pe.Telefono,
                               t.Tipo, t.Descripcion
                               FROM Inmueble i
                               INNER JOIN Propietario p ON i.PropietarioId = p.Id
                               INNER JOIN Persona pe ON p.Dni = pe.Dni
                               INNER JOIN TipoInmueble t ON i.TipoId = t.Id
                               WHERE i.Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(i.Id)),
                            PropietarioId = reader.GetInt32(nameof(i.PropietarioId)),
                            TipoId = reader.GetInt32(nameof(i.TipoId)),
                            Direccion = reader.GetString(nameof(i.Direccion)),
                            Localidad = reader.GetString(nameof(i.Localidad)),
                            Longitud = reader.GetDecimal(nameof(i.Longitud)),
                            Latitud = reader.GetDecimal(nameof(i.Latitud)),
                            Uso = reader.GetString(nameof(i.Uso)),
                            Ambientes = reader.GetInt32(nameof(i.Ambientes)),
                            Observacion = reader.IsDBNull(nameof(i.Observacion)) ? null : reader.GetString(nameof(i.Observacion)),
                            Estado = reader.GetString(nameof(i.Estado)),
                            Precio = reader.GetDecimal(nameof(i.Precio)),

                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(nameof(i.PropietarioId)),
                                Dni = reader.GetInt32(nameof(i.Propietario.Dni)),
                                Estado = reader.GetBoolean(nameof(i.Propietario.Estado)),
                                Persona = new Persona
                                {
                                    Nombre = reader.GetString(nameof(i.Propietario.Persona.Nombre)),
                                    Apellido = reader.GetString(nameof(i.Propietario.Persona.Apellido)),
                                    Direccion = reader.IsDBNull(nameof(i.Propietario.Persona.Direccion)) ? null : reader.GetString(nameof(i.Propietario.Persona.Direccion)),
                                    Localidad = reader.IsDBNull(nameof(i.Propietario.Persona.Localidad)) ? null : reader.GetString(nameof(i.Propietario.Persona.Localidad)),
                                    Correo = reader.IsDBNull(nameof(i.Propietario.Persona.Correo)) ? null : reader.GetString(nameof(i.Propietario.Persona.Correo)),
                                    Telefono = reader.GetInt64(nameof(i.Propietario.Persona.Telefono))

                                }
                            },
                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(i.TipoInmueble.Id)),
                                Tipo = reader.GetString(nameof(i.TipoInmueble.Tipo)),
                                Descripcion = reader.IsDBNull(nameof(i.TipoInmueble.Descripcion)) ? null : reader.GetString(nameof(i.TipoInmueble.Descripcion))
                            }

                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public IList<TipoInmueble> ObtenerTiposInmueble()
        {
            IList<TipoInmueble> res = new List<TipoInmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM tipo_inmueble";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TipoInmueble t = new TipoInmueble
                        {
                            Id = reader.GetInt32(nameof(t.Id)),
                            Tipo = reader.GetString(nameof(t.Tipo)),
                            Descripcion = reader.IsDBNull(nameof(t.Descripcion)) ? null : reader.GetString(nameof(t.Descripcion))
                        };
                        res.Add(t);
                    }
                    connection.Close();
                }
            }
            return res;
        }



        public IList<Inmueble> BuscarPorDireccion(string direccion)
        {
            throw new NotImplementedException();
        }

        public IList<Inmueble> BuscarPorEstado(string estado)
        {
            throw new NotImplementedException();
        }

        public IList<Inmueble> BuscarPorLocalidad(string localidad)
        {
            throw new NotImplementedException();
        }

        public IList<Inmueble> BuscarPorPrecio(decimal precioMin, decimal precioMax)
        {
            throw new NotImplementedException();
        }

        public IList<Inmueble> BuscarPorPropietario(int propietarioId)
        {
            throw new NotImplementedException();
        }

        public IList<Inmueble> BuscarPorTipo(int tipoId)
        {
            throw new NotImplementedException();
        }



    }
}
