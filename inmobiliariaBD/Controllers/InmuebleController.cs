using System.Globalization;
using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inmobiliariaBD.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repositorioPropietario;


        public InmuebleController(IConfiguration config, IRepositorioInmueble repo, IRepositorioPropietario repoPropietario)
        {
            this.config = config;
            this.repositorio = repo;
            this.repositorioPropietario = repoPropietario;

        }

        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
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
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Persona.Apellido}, {p.Persona.Nombre} - {p.Dni}  "
                }).ToList();

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

        public IActionResult Baja(int id)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            repositorio.Baja(inmueble);
            TempData["Mensaje"] = $"Se Elimino Correctamente el inmueble";
            return RedirectToAction("Index");
}
        [HttpPost]
        public IActionResult ModificarEstado(int id, string nuevoEstado)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            inmueble.Estado = nuevoEstado;
            repositorio.ModificarEstado(inmueble);
            TempData["Mensaje"] = $"El estado del inmueble se cambió a {nuevoEstado}.";

            return RedirectToAction("CreateOrEdit", new { id = inmueble.Id });
        }

        public ActionResult Details(int id)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            return View(inmueble);
        }



    }

}