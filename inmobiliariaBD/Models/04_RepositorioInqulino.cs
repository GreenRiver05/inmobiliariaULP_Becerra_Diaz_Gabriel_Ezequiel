using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
                                    Telefono = reader.GetInt64(nameof(i.Persona.Telefono))
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

        public IList<Inquilino> BuscarPorNombre(string nombre)
        {
            throw new NotImplementedException();
        }

        public int Baja(Inquilino p)
        {
            throw new NotImplementedException();
        }
    }

}