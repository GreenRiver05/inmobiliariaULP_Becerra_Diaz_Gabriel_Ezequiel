
using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliariaBD.Models
{
    public class RepositorioTipoInmueble : RepositorioBase, IRepositorio<TipoInmueble>
    {
        public RepositorioTipoInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(TipoInmueble t)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Tipo_Inmueble (Tipo, Descripcion) VALUES (@tipo, @descripcion);
                       SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@tipo", t.Tipo);
                    command.Parameters.AddWithValue("@descripcion", t.Descripcion);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    t.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(TipoInmueble t)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM Tipo_Inmueble WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", t.Id);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(TipoInmueble t)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Tipo_Inmueble
                     SET Tipo = @tipo,
                     descripcion = @descripcion
                     WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@tipo", t.Tipo);
                    command.Parameters.AddWithValue("@id", t.Id);
                    command.Parameters.AddWithValue("@descripcion", t.Descripcion);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int ObtenerCantidad()
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM Tipo_Inmueble";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public IList<TipoInmueble> ObtenerPaginados(int pagina, int cantidadPorPagina)
        {
            var lista = new List<TipoInmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Tipo, descripcion FROM Tipo_Inmueble
                       ORDER BY Tipo
                       LIMIT @offset, @cantidad";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * cantidadPorPagina);
                    command.Parameters.AddWithValue("@cantidad", cantidadPorPagina);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TipoInmueble tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(tipo.Id)),
                                Tipo = reader.GetString(nameof(tipo.Tipo)),
                                Descripcion = reader.IsDBNull(nameof(tipo.Descripcion)) ? null : reader.GetString(nameof(tipo.Descripcion))
                            };
                            lista.Add(tipo);
                        }
                    }
                    connection.Close();
                }
            }
            return lista;
        }



        public int ModificarEstado(TipoInmueble p)
        {
            throw new NotImplementedException();
        }


        public TipoInmueble ObtenerPorId(int id)
        {
            TipoInmueble? t = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Tipo, Descripcion FROM Tipo_Inmueble WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            t = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(t.Id)),
                                Tipo = reader.GetString(nameof(t.Tipo)),
                                Descripcion = reader.IsDBNull(nameof(t.Descripcion)) ? null : reader.GetString(nameof(t.Descripcion))
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return t;
        }


        public IList<TipoInmueble> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }


}