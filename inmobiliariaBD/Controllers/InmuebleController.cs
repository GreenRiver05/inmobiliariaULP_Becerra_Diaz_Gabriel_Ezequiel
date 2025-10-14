using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inmobiliariaBD.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repositorioPropietario;
        private readonly IRepositorioContrato repositorioContrato;


        public InmuebleController(IConfiguration config, IRepositorioInmueble repo, IRepositorioPropietario repoPropietario, IRepositorioContrato repositorioContrato)
        {
            this.config = config;
            this.repositorio = repo;
            this.repositorioPropietario = repoPropietario;
            this.repositorioContrato = repositorioContrato;

        }

        public ActionResult Index(int pagina = 1, string? busqueda = null, bool? estado = null, DateTime? desde = null, DateTime? hasta = null, string? estadoPago = null)
        {
            int cantidadPorPagina = 5;
            var inmuebles = repositorio.ObtenerPaginados(pagina, cantidadPorPagina, busqueda, estado, desde, hasta, estadoPago);
            int total = repositorio.ObtenerCantidad(busqueda, estado, desde, hasta, estadoPago);
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / cantidadPorPagina);
            ViewBag.Busqueda = busqueda;
            ViewBag.Estado = estado;
            return View(inmuebles);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int? id)
        {
            Inmueble i;

            if (id.HasValue)
            {
                //se colaca id.value por que id es nullable y el metodo ObtenerPorId no acepta nullable
                // asi que se usa id.value para obtener el valor real
                i = repositorio.ObtenerPorId(id.Value);

            }
            else
            {
                i = new Inmueble
                {
                    Propietario = new Propietario { Persona = new Persona() },
                    TipoInmueble = new TipoInmueble()
                };
            }

            // Arma el combo de propietarios para la vista:
            // - Value: el Id del propietario (como string)
            // - Text: DNI - Apellido, Nombre (datos tomados desde la propiedad Persona)
            // Esto se usa en Razor con asp-items para poblar el <select> de PropietarioId
            //.Select(...) → Recorre cada propietario y lo transforma en un SelectListItem, que es lo que Razor necesita para armar un <select>
            //.ToList() → Convierte el resultado en una lista que se guarda en ViewBag.Propietarios
            // ViewBag.Propietarios = repositorioPropietario.ObtenerTodos()
            //     .Select(p => new SelectListItem
            //     {
            //         Value = p.Id.ToString(),
            //         Text = $"{p.Persona.Apellido}, {p.Persona.Nombre} - {p.Dni}  "
            //     }).ToList();

            ViewBag.TiposInmueble = repositorio.ObtenerTiposInmueble()
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Tipo
                }).ToList();

            return View(i);
        }

        [HttpPost]
        public IActionResult CreateOrEdit(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {

                if (inmueble.Id > 0)
                {
                    repositorio.Modificacion(inmueble);
                    TempData["Mensaje"] = $"El inmueble {inmueble.Id} se modificó correctamente";
                }
                else
                {
                    repositorio.Alta(inmueble);
                    TempData["Mensaje"] = $"El inmueble {inmueble.Id} se creó correctamente";
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "No se pudo guardar el inmueble";
                return View("CreateOrEdit", inmueble);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Baja(int id)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            repositorio.Baja(inmueble);
            TempData["Mensaje"] = $"Se Elimino Correctamente el inmueble";
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ModificarEstado(int id, bool nuevoEstado)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            inmueble.Estado = nuevoEstado;
            repositorio.ModificarEstado(inmueble);
            TempData["Mensaje"] = $"Se modificó el estado del inmueble correctamente.";

            return RedirectToAction("CreateOrEdit", new { id = inmueble.Id });
        }

        public ActionResult Detalles(int id)
        {

            var inmueble = repositorio.ObtenerPorId(id);
            ViewBag.Contratos = repositorioContrato.ObtenerPorInmueble(id);
            return View(inmueble);
        }



    }

}