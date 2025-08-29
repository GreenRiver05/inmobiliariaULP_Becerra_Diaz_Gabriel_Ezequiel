using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaBD.Models
{

    public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Propietario (Dni, Estado)
                             VALUES (@dni, true);
                             SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarEstado(int dni, bool estado)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Propietario
                             SET Estado=@estado
                             WHERE Dni=@dni";
                using (var command = new MySqlCommand(sql, connection))
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


        public int Modificacion(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Propietario
                             SET Estado=@estado
                             WHERE Dni=@dni";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@estado", p.Estado);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.id, pe.nombre, pe.apellido, p.dni, pe.direccion, pe.localidad, pe.correo, pe.telefono, p.estado
                             FROM propietario as p
                             JOIN persona pe ON pe.dni = p.dni";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Direccion = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Localidad = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Correo = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Telefono = reader.GetInt32(7)
                            },
                            Dni = reader.GetInt32(3),
                            Estado = reader.GetBoolean(8)
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Propietario ObtenerPorId(int id)
        {
            Propietario? p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.id, p.dni, p.estado, pe.nombre, pe.apellido, pe.direccion, pe.localidad, pe.correo, pe.telefono, pe.estado
                             FROM propietario p 
                             JOIN persona pe ON pe.dni = p.dni
                             WHERE p.id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Persona = new Persona
                            {
                                Nombre = reader.GetString(3),
                                Apellido = reader.GetString(4),
                                Direccion = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Localidad = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Correo = reader.IsDBNull(7) ? null : reader.GetString(7),
                                Telefono = reader.GetInt32(8),
                                Estado = reader.GetBoolean(9)
                            },
                            Estado = reader.GetBoolean(2)
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public Propietario ObtenerPorDni(int dni)
        {
            throw new NotImplementedException();
        }

        public IList<Propietario> BuscarPorNombre(string nombre)
        {
            throw new NotImplementedException();
        }
    }
}