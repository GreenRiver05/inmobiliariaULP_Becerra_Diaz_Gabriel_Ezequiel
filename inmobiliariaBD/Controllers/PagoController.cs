using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace inmobiliariaBD.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly IRepositorioPago repo;
        private readonly IRepositorioContrato repoContrato;
        private readonly IConfiguration config;

        public PagoController(IConfiguration config, IRepositorioPago repo, IRepositorioContrato repoContrato)
        {
            this.config = config;
            this.repo = repo;
            this.repoContrato = repoContrato;
        }
        public IActionResult Index(int pagina = 1, string? busqueda = null, bool? estado = null, DateTime? desde = null, DateTime? hasta = null, string? estadoPago = null)
        {
            int cantidadPorPagina = 5;

            // Aunque estado no lo uso en Pago, lo dejo para cumplir con la firma común
            var pagos = repo.ObtenerPaginados(pagina, cantidadPorPagina, busqueda, estado, desde, hasta, estadoPago);
            int total = repo.ObtenerCantidad(busqueda, estado, desde, hasta, estadoPago);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / cantidadPorPagina);
            ViewBag.Busqueda = busqueda;
            ViewBag.EstadoPago = estadoPago;
            ViewBag.Desde = desde?.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd");

            return View(pagos);
        }




        [HttpGet]
        public IActionResult CreateOrEdit(int? id, int? contratoId)
        {
            if (id == null)
            {
                var nuevo = new Pago
                {
                    ContratoId = contratoId ?? 0,
                    Fecha = DateTime.Today,
                    Contrato = new Contrato { Inquilino = new Inquilino { Persona = new Persona() } }
                };

                ViewBag.ContratoFijo = contratoId.HasValue;
                ViewBag.Contratos = repoContrato.ObtenerTodos()
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = $"Contrato #{c.Id} - {c.Inquilino.Persona.Nombre} {c.Inquilino.Persona.Apellido}"
                    }).ToList();

                return View(nuevo);
            }

            var pago = repo.ObtenerPorId(id.Value);

            ViewBag.ContratoFijo = contratoId.HasValue;

            ViewBag.Contratos = repoContrato.ObtenerTodos()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"Contrato #{c.Id} - {c.Inquilino.Persona.Nombre} {c.Inquilino.Persona.Apellido}"
                }).ToList();

            return View(pago);
        }


        [HttpPost]
        public IActionResult CreateOrEdit(Pago pago, bool volverAContrato = false)
        {
            if (!decimal.TryParse(pago.Monto, NumberStyles.Any, new CultureInfo("es-AR"), out var montoDecimal) || montoDecimal <= 0)
            {
                ModelState.AddModelError("Monto", "El monto debe ser un número válido mayor a cero.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Contratos = repoContrato.ObtenerTodos()
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = $"Contrato #{c.Id} - {c.Inquilino.Persona.Apellido}, {c.Inquilino.Persona.Nombre}"
                    }).ToList();
                return View(pago);
            }

            if (pago.Id == 0)
            {
                repo.Alta(pago);
                TempData["Mensaje"] = "Pago registrado correctamente.";
            }
            else
            {
                repo.Modificacion(pago);
                TempData["Mensaje"] = "Pago actualizado correctamente.";
            }

            if (volverAContrato)
            {
                return RedirectToAction("Detalles", "Contrato", new { id = pago.ContratoId });
            }

            return RedirectToAction("Index");
        }

        public IActionResult Detalles(int id,
                                        [FromServices] IRepositorioInquilino repoInquilino,
                                        [FromServices] IRepositorioPropietario repoPropietario,
                                        [FromServices] IRepositorioInmueble repoInmueble,
                                        bool volverAContrato = false)
        {
            var pago = repo.ObtenerPorId(id);

            var contrato = repoContrato.ObtenerPorId(pago.ContratoId);
            var inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);
            var inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);
            var propietario = repoPropietario.ObtenerPorId(inmueble.PropietarioId);
            ViewBag.VolverAContrato = volverAContrato;

            var modelo = new PagoDetalleViewModel
            {
                Pago = pago,
                Contrato = contrato,
                Inquilino = inquilino,
                Propietario = propietario,
                Inmueble = inmueble
            };

            return View(modelo);
        }


        public IActionResult ModificarEstado(int id, string nuevoEstado)
        {
            var pago = repo.ObtenerPorId(id);
            pago.Estado = nuevoEstado;
            repo.ModificarEstado(pago);
            TempData["Mensaje"] = $"Se modificó el estado del inmueble correctamente.";

            return RedirectToAction("CreateOrEdit", new { id = pago.Id, contratoId = pago.ContratoId });
        }





        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Baja(int id, bool volverAContrato = false)
        {
            var pago = repo.ObtenerPorId(id);
            repo.Baja(pago);
            TempData["Mensaje"] = "Pago eliminado correctamente.";
            if (volverAContrato)
            {
                return RedirectToAction("Detalles", "Contrato", new { id = pago.ContratoId });
            }
            return RedirectToAction("Index");
        }
    }
}
