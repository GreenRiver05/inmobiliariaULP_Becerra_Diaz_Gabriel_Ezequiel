using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace inmobiliariaBD.Models
{
    public class RepositorioGestion : RepositorioBase
    {
        public RepositorioGestion(IConfiguration configuration) : base(configuration) { }

        public int Alta(Gestion g)
        {
            int id = 0;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var sql = @"INSERT INTO gestion (Usuario_Id, Entidad_Id, Entidad_Tipo, Accion, Fecha)
                        VALUES (@usuarioId, @entidadId, @entidadTipo, @accion, @fecha);
                        SELECT LAST_INSERT_ID();";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@usuarioId", g.UsuarioId);
            command.Parameters.AddWithValue("@entidadId", g.EntidadId);
            command.Parameters.AddWithValue("@entidadTipo", g.EntidadTipo);
            command.Parameters.AddWithValue("@accion", g.Accion);
            command.Parameters.AddWithValue("@fecha", g.Fecha);
            id = Convert.ToInt32(command.ExecuteScalar());
            g.Id = id;
            return id;
        }

        public List<Gestion> ObtenerPorEntidad(string tipo, int entidadId)
        {
            var lista = new List<Gestion>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var sql = @"SELECT g.Id, g.Usuario_Id, g.Entidad_Id, g.Entidad_Tipo, g.Accion, g.Fecha,
                            p.Nombre, p.Apellido, p.Dni
                        FROM gestion g
                        INNER JOIN usuario u ON u.Id = g.Usuario_Id
                        INNER JOIN persona p ON p.dni= u.dni
                        WHERE g.Entidad_Tipo = @tipo AND g.Entidad_Id = @entidadId
                        ORDER BY g.Fecha DESC;
                        ";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@tipo", tipo);
            command.Parameters.AddWithValue("@entidadId", entidadId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Gestion
                {
                    Id = reader.GetInt32("Id"),
                    UsuarioId = reader.GetInt32("Usuario_Id"),
                    EntidadId = reader.GetInt32("Entidad_Id"),
                    EntidadTipo = reader.GetString("Entidad_Tipo"),
                    Accion = reader.GetString("Accion"),
                    Fecha = reader.GetDateTime("Fecha"),
                    Usuario = new Usuario
                    {
                        Id = reader.GetInt32("Usuario_Id"),
                        Persona = new Persona
                        {
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Dni = reader.GetInt32("Dni")
                        }
                    }
                });

            }
            return lista;
        }


    }


}
