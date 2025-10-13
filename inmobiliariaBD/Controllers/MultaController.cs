using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace inmobiliariaBD.Controllers
{
    [Authorize]
    public class MultaController : Controller
    {
        private readonly IRepositorioMulta repo;
        private readonly IRepositorioContrato repoContrato;
        private readonly IConfiguration config;

        public MultaController(IConfiguration config, IRepositorioMulta repo, IRepositorioContrato repoContrato)
        {
            this.config = config;
            this.repo = repo;
            this.repoContrato = repoContrato;
        }

        public IActionResult Index(int pagina = 1, string? busqueda = null, bool? estado = null, DateTime? desde = null, DateTime? hasta = null, string? estadoPago = null)
        {
            int cantidadPorPagina = 5;
            var multas = repo.ObtenerPaginados(pagina, cantidadPorPagina, busqueda, estado, desde, hasta, estadoPago);
            int total = repo.ObtenerCantidad(busqueda, estado, desde, hasta, estadoPago);
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / cantidadPorPagina);
            ViewBag.Busqueda = busqueda;
            return View(multas);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int? id)
        {
            Multa multa = id.HasValue
                ? repo.ObtenerPorId(id.Value)
                : new Multa { Contrato = new Contrato { Inquilino = new Inquilino { Persona = new Persona() } } };

            ViewBag.Contratos = repoContrato.ObtenerTodos()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"Contrato #{c.Id} - {c.Inquilino.Persona.Apellido}, {c.Inquilino.Persona.Nombre}"
                }).ToList();

            return View(multa);
        }

        [HttpPost]
        public IActionResult CreateOrEdit(Multa multa)
        {
            if (!decimal.TryParse(multa.Monto, NumberStyles.Any, new CultureInfo("es-AR"), out var montoDecimal) || montoDecimal <= 0)
            {
                ModelState.AddModelError("Monto", "El monto debe ser un número válido mayor a cero.");
            }

            if (multa.FechaAviso >= multa.FechaTerminacion)
            {
                ModelState.AddModelError("FechaTerminacion", "La fecha de terminación debe ser posterior a la de aviso.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Contratos = repoContrato.ObtenerTodos()
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = $"Contrato #{c.Id} - {c.Inquilino.Persona.Apellido}, {c.Inquilino.Persona.Nombre}"
                    }).ToList();
                return View(multa);
            }

            if (multa.Id == 0)
            {
                repo.Alta(multa);
                TempData["Mensaje"] = "Multa registrada correctamente.";
            }
            else
            {
                repo.Modificacion(multa);
                TempData["Mensaje"] = "Multa actualizada correctamente.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Detalles(int id,
                                        [FromServices] IRepositorioInquilino repoInquilino,
                                        [FromServices] IRepositorioPropietario repoPropietario,
                                        [FromServices] IRepositorioInmueble repoInmueble)
        {
            var multa = repo.ObtenerPorId(id);
            var contrato = repoContrato.ObtenerPorId(multa.ContratoId);
            var inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);
            var inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);
            var propietario = repoPropietario.ObtenerPorId(inmueble.PropietarioId);

            var modelo = new MultaDetalleViewModel
            {
                Multa = multa,
                Contrato = contrato,
                Inquilino = inquilino,
                Propietario = propietario,
                Inmueble = inmueble
            };

            return View(modelo);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Baja(int id)
        {
            var multa = repo.ObtenerPorId(id);
            repo.Baja(multa);
            TempData["Mensaje"] = "Multa eliminada correctamente.";
            return RedirectToAction("Index");
        }
    }
}
