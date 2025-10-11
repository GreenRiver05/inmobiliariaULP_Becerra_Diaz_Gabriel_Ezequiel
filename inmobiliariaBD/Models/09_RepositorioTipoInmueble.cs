
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


        public IList<TipoInmueble> ObtenerPaginados(int pagina, int cantidadPorPagina, string? busqueda = null, bool? estado = null)
        {
            var lista = new List<TipoInmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                    filtros.Add("(Tipo LIKE @busqueda OR descripcion LIKE @busqueda)");


                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = $@"
                             SELECT Id, Tipo, descripcion 
                             FROM Tipo_Inmueble
                             {where}
                             ORDER BY Tipo
                             LIMIT @cantidad OFFSET @offset";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@cantidad", cantidadPorPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * cantidadPorPagina);
                    if (!string.IsNullOrEmpty(busqueda))
                        command.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");


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

        public int ObtenerCantidad(string? busqueda = null, bool? estado = null)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                var filtros = new List<string>();
                if (!string.IsNullOrEmpty(busqueda))
                    filtros.Add("(Tipo LIKE @busqueda OR descripcion LIKE @busqueda)");

                string where = filtros.Count > 0 ? "WHERE " + string.Join(" AND ", filtros) : "";

                string sql = $@"
                             SELECT COUNT(*) 
                             FROM Tipo_Inmueble
                             {where}";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(busqueda))
                        command.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");

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