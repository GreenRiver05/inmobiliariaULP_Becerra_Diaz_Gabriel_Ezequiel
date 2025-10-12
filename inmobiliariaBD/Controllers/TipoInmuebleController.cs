using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaBD.Controllers
{
    [Authorize]
    public class TipoInmuebleController : Controller
    {
        private readonly IConfiguration config;
        private readonly RepositorioTipoInmueble repositorio;

        public TipoInmuebleController(IConfiguration config, RepositorioTipoInmueble repo)
        {
            this.config = config;
            this.repositorio = repo;
        }

        public ActionResult Index(int pagina = 1, string? busqueda = null, bool? estado = null)
        {
            int cantidadPorPagina = 5;
            var tipos = repositorio.ObtenerPaginados(pagina, cantidadPorPagina, busqueda, estado);
            int total = repositorio.ObtenerCantidad();
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / cantidadPorPagina);
            ViewBag.Busqueda = busqueda;
            ViewBag.Estado = estado;
            return View(tipos);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int? id)
        {
            TipoInmueble tipoInmueble;

            if (id.HasValue)
            {
                tipoInmueble = repositorio.ObtenerPorId(id.Value);
            }
            else
            {
                tipoInmueble = new TipoInmueble();
            }

            return View(tipoInmueble);
        }

        [HttpPost]
        public IActionResult CreateOrEdit(TipoInmueble tipoInmueble)
        {
            if (ModelState.IsValid)
            {
                if (tipoInmueble.Id > 0)
                {
                    repositorio.Modificacion(tipoInmueble);
                    TempData["Mensaje"] = "Tipo Inmueble actualizado correctamente.";
                }
                else
                {
                    repositorio.Alta(tipoInmueble);
                    TempData["Mensaje"] = "Tipo Inmueble creado correctamente.";
                }
                return RedirectToAction("Index");
            }
            return View(tipoInmueble);
        }



        [Authorize(Roles = "Administrador")]
        public IActionResult Baja(int id)
        {
            var tipoInmueble = repositorio.ObtenerPorId(id);
            repositorio.Baja(tipoInmueble);
            TempData["Mensaje"] = "El tipo de inmueble fue eliminado";
            return RedirectToAction("Index");
        }
    }
}