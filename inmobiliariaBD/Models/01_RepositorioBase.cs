//Permite acceder a la interfaz IConfiguration, que se usa para leer valores del archivo appsettings.json(como cadenas de conexión, claves, etc.).
using Microsoft.Extensions.Configuration;

//tipos básicos como Exception, DateTime, etc.
using System;

//listas, diccionarios (List<T>, Dictionary<K,V>, etc.).
using System.Collections.Generic;

//consultas sobre colecciones (Where, Select, etc.).
using System.Linq;

//soporte para programación asincrónica (Task, async, await).
using System.Threading.Tasks;

namespace inmobiliariaBD.Models
{
    public abstract class RepositorioBase
    {

        //Guarda la configuración inyectada y extrae la cadena de conexión.
        protected readonly IConfiguration configuration;
        protected readonly string connectionString;


        //Constructor que recibe la configuración (por inyección de dependencias) y obtiene la cadena de conexión desde appsettings.json.
        protected RepositorioBase(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:MySql"];
        }
    }
}


/**

Clase base para repositorios que centraliza el acceso a la cadena de conexión.
Utiliza IConfiguration para obtener valores desde appsettings.json.
Diseñada para ser heredada por repositorios concretos.

**/