using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Security.Claims;

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

                if (contratoId.HasValue)
                {
                    var pagos = repo.ObtenerPagosPorContrato(contratoId.Value);
                    nuevo.NumeroPago = pagos.Count + 1;
                }
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


        [HttpGet]
        public IActionResult ObtenerNumeroPago(int contratoId)
        {
            var pagos = repo.ObtenerPagosPorContrato(contratoId);
            int numero = pagos.Count + 1;
            return Json(new { numero });
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

            int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (pago.Id == 0)
            {
                var pagosExistentes = repo.ObtenerPagosPorContrato(pago.ContratoId);
                pago.NumeroPago = pagosExistentes.Count + 1;
                var nuevoId = repo.Alta(pago);
                RegistrarGestion(usuarioId, nuevoId, "Pago", "Alta");
                TempData["Mensaje"] = "Pago registrado correctamente.";
            }
            else
            {
                repo.Modificacion(pago);
                RegistrarGestion(usuarioId, pago.Id, "Pago", "Modificación");
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
                                        [FromServices] RepositorioGestion repoGestion,
                                        bool volverAContrato = false)
        {
            var pago = repo.ObtenerPorId(id);

            var contrato = repoContrato.ObtenerPorId(pago.ContratoId);
            var inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);
            var inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);
            var propietario = repoPropietario.ObtenerPorId(inmueble.PropietarioId);
            var auditoria = repoGestion.ObtenerPorEntidad("Pago", id);
            ViewBag.VolverAContrato = volverAContrato;

            var modelo = new PagoDetalleViewModel
            {
                Pago = pago,
                Contrato = contrato,
                Inquilino = inquilino,
                Propietario = propietario,
                Inmueble = inmueble,
                Auditoria = auditoria
            };

            return View(modelo);
        }






        public IActionResult ModificarEstado(int id, string nuevoEstado)
        {
            var pago = repo.ObtenerPorId(id);
            pago.Estado = nuevoEstado;
            repo.ModificarEstado(pago);
            TempData["Mensaje"] = $"Se modificó el estado del inmueble correctamente.";

            int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            RegistrarGestion(usuarioId, pago.Id, "Pago", $"Cambio de estado a {nuevoEstado}");



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





        private void RegistrarGestion(int usuarioId, int entidadId, string entidadTipo, string accion)
        {
            var repoGestion = new RepositorioGestion(config);
            var gestion = new Gestion
            {
                UsuarioId = usuarioId,
                EntidadId = entidadId,
                EntidadTipo = entidadTipo,
                Accion = accion,
                Fecha = DateTime.Now
            };
            repoGestion.Alta(gestion);
        }
    }
}
