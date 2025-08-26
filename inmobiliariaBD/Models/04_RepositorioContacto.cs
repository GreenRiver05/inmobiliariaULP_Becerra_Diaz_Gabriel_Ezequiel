using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaBD.Models
{
    public class RepositorioContacto : RepositorioBase, IRepositorioContacto
    {
        public RepositorioContacto(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Contacto c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Contacto (Dni, Correo, Telefono, TelefonoSecundario)
                             VALUES (@dni, @correo, @telefono, @telefonoSecundario);
                             SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", c.Dni);
                    command.Parameters.AddWithValue("@correo", (object?)c.Correo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefono", (object?)c.Telefono ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefonoSecundario", (object?)c.TelefonoSecundario ?? DBNull.Value);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.Id = res; 
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Contacto WHERE Id= @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Contacto c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Contacto
                             SET Dni=@dni, Correo=@correo, Telefono=@telefono, TelefonoSecundario=@telefonoSecundario
                             WHERE Id=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", c.Id);
                    command.Parameters.AddWithValue("@dni", c.Dni);
                    command.Parameters.AddWithValue("@correo", (object?)c.Correo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefono", (object?)c.Telefono ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefonoSecundario", (object?)c.TelefonoSecundario ?? DBNull.Value);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Contacto> ObtenerTodos()
        {
            IList<Contacto> res = new List<Contacto>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Dni, Correo, Telefono, TelefonoSecundario
                             FROM Contacto";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Contacto c = new Contacto
                            {
                                Id = reader.GetInt32(nameof(c.Id)),
                                Dni = reader.GetInt32(nameof(c.Dni)),
                                Correo = reader.IsDBNull(2) ? null : reader.GetString(nameof(c.Correo)),
                                Telefono = reader.IsDBNull(3) ? null : reader.GetInt32(nameof(c.Telefono)),
                                TelefonoSecundario = reader.IsDBNull(4) ? null : reader.GetInt32(nameof(c.TelefonoSecundario)),
                               
                            };
                            res.Add(c);
                        }
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Contacto ObtenerPorId(int id)
        {
            Contacto c = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Dni, Correo, Telefono, TelefonoSecundario
                             FROM Contacto
                             WHERE Id=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            c = new Contacto
                            {
                                Id = reader.GetInt32(0),
                                Dni = reader.GetInt32(1),
                                Correo = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Telefono = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                                TelefonoSecundario = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                               
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return c;
        }

        public IList<Persona> BuscarPorDni(int dni)
        {
            throw new NotImplementedException();
        }
    }


}