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
                string sql = @"INSERT INTO Persona (Dni, Nombre, Apellido, Direccion, Localidad, Correo, Telefono, Estado)
                             VALUES (@dni, @nombre, @apellido, @direccion, @localidad, @correo, @telefono, 1);";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@direccion", (object?)p.Direccion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@localidad", (object?)p.Localidad ?? DBNull.Value);
                    command.Parameters.AddWithValue("@correo", (object?)p.Correo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);
                    
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarEstado(int dni, bool estado)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Persona
                             SET Estado=@estado
                             WHERE Dni=@dni";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@estado", estado);
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
                             SET Nombre=@nombre, Apellido=@apellido, Direccion=@direccion Localidad=@localidad, Correo=@correo, Telefono=@telefono
                             WHERE Dni=@dni";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@direccion", (object?)p.Direccion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@localidad", (object?)p.Localidad ?? DBNull.Value);
                    command.Parameters.AddWithValue("@correo", (object?)p.Correo ?? DBNull.Value);
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Dni, Nombre, Apellido, Direccion, Localidad, Correo, Telefono, Estado
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
                                Correo = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Telefono = reader.GetInt32(6),
                                Estado = reader.GetBoolean(7)

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
                string sql = @"SELECT Dni, Nombre, Apellido, Direccion, Localidad, Correo, Telefeno, Estado
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
                                Correo = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Telefono = reader.GetInt32(6),
                                Estado = reader.GetBoolean(7)
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