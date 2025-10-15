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
            var sql = @"INSERT INTO gestion (UsuarioId, EntidadId, EntidadTipo, Accion, Fecha)
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



    }


}
