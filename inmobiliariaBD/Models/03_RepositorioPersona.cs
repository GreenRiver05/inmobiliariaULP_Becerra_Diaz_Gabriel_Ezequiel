using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Persona (Dni, Nombre, Apellido, Direccion, Localidad, Estado, Avatar)
                             VALUES (@dni, @nombre, @apellido, @direccion, @localidad, @estado, @avatar);";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@direccion", (object?)p.Direccion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@localidad", (object?)p.Localidad ?? DBNull.Value);
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@avatar", (object?)p.Avatar ?? DBNull.Value);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int dni)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Persona WHERE Dni=@dni";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", dni);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Persona p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Persona
                             SET Nombre=@nombre, Apellido=@apellido, Direccion=@direccion Localidad=@localidad, Estado=@estado, Avatar=@avatar
                             WHERE Dni=@dni";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@direccion", (object?)p.Direccion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@localidad", (object?)p.Localidad ?? DBNull.Value);
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@avatar", (object?)p.Avatar ?? DBNull.Value);

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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Dni, Nombre, Apellido, Direccion, Localidad, Estado, Avatar
                             FROM Persona";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Persona p = new Persona
                            {
                                Dni = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Direccion = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Localidad = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Estado = reader.GetBoolean(5),
                                Avatar = reader.IsDBNull(6) ? null : (byte[])reader.GetValue(6)

                            };
                            res.Add(p);
                        }
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Persona ObtenerPorId(int id)
        {
            Persona p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Dni, Nombre, Apellido, Direccion, Localidad, Estado, Avatar
                             FROM Persona 
                             WHERE Dni=@dni";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            p = new Persona
                            {
                                Dni = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Direccion = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Localidad = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Estado = reader.GetBoolean(5),
                                Avatar = reader.IsDBNull(6) ? null : (byte[])reader.GetValue(6)
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public Persona ObtenerPorDni(int dni)
        {
            throw new NotImplementedException();
        }

        public IList<Persona> BuscarPorNombre(string nombre)
        {
            throw new NotImplementedException();
        }
    }
}