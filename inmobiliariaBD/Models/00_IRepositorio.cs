//Trae clases básicas del sistema como Console, DateTime, Exception, etc.
using System;

//Permite usar colecciones genéricas como List<T>, Dictionary<TKey, TValue>, etc.
using System.Collections.Generic;

//Habilita LINQ (Language Integrated Query), útil para consultar colecciones con expresiones como .Where(), .Select(), .FirstOrDefault(), etc.
using System.Linq;

//Proporciona soporte para programación asincrónica con Task, async, await.
using System.Threading.Tasks;


//Define el espacio de nombres propio del proyecto, agrupando tus clases e interfaces bajo una misma etiqueta lógica
namespace inmobiliariaBD.Models

{
    public interface IRepositorio<T>
    {
        int Alta(T p);

        int Baja(T p);
        int ModificarEstado(T p);
        int Modificacion(T p);

        IList<T> ObtenerTodos();
        T ObtenerPorId(int id);
        IList<T> ObtenerPaginados(int pagina, int cantidadPorPagina, string? busqueda, bool? estado, DateTime? desde, DateTime? hasta,string? estadoPago);

        int ObtenerCantidad( string? busqueda, bool? estado, DateTime? desde, DateTime? hasta, string? estadoPago);
    }
}


/**
Define un contrato genérico para operaciones CRUD sobre entidades de tipo T.
Se usa para abstraer el acceso a datos y facilitar la reutilización y el testeo.

USING:
Evitan escribir nombres largos: en vez de System.Collections.Generic.List<T>, podés escribir simplemente List<T>.
Organizan el código: te permiten saber qué funcionalidades estás usando.
Facilitan el mantenimiento: si ves un using System.Linq, ya sabés que probablemente hay consultas LINQ en el archivo. **/
