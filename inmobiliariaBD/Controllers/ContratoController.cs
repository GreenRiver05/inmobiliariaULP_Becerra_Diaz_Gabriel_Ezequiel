using System.Globalization;
using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inmobiliariaBD.Controllers
{
    [Authorize]
    public class ContratoController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioContrato repositorio;
        private readonly IRepositorioInquilino repoInquilino;
        private readonly IRepositorioInmueble repoInmueble;


        public ContratoController(IConfiguration config, IRepositorioContrato repo, IRepositorioInquilino repoInq, IRepositorioInmueble repoInm)
        {
            this.config = config;
            this.repositorio = repo;
            this.repoInquilino = repoInq;
            this.repoInmueble = repoInm;
        }

        public ActionResult Index(int pagina = 1, string? busqueda = null, bool? estado = null, DateTime? desde = null, DateTime? hasta = null, string? estadoPago = null)
        {
            int cantidadPorPagina = 5;
            var contratos = repositorio.ObtenerPaginados(pagina, cantidadPorPagina, busqueda, estado, desde, hasta, estadoPago);
            int total = repositorio.ObtenerCantidad(busqueda, estado, desde, hasta, estadoPago);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / cantidadPorPagina);
            ViewBag.Busqueda = busqueda;
            ViewBag.Estado = estado;
            ViewBag.Desde = desde?.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd");
            return View(contratos);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int? id, bool esRenovacion = false, int? inquilinoId = null, int? inmuebleId = null, string? monto = null)
        {
            Contrato contrato;

            if (id.HasValue)
            {
                contrato = repositorio.ObtenerPorId(id.Value);
            }
            else
            {
                contrato = new Contrato
                {
                    Estado = "Vigente",
                    Desde = DateTime.Today,
                    Hasta = DateTime.Today.AddMonths(12),
                    InquilinoId = inquilinoId ?? 0,
                    InmuebleId = inmuebleId ?? 0,
                    Monto = monto ?? "",
                    Inquilino = inquilinoId.HasValue ? repoInquilino.ObtenerPorId(inquilinoId.Value) : new Inquilino { Persona = new Persona() },
                    Inmueble = inmuebleId.HasValue ? repoInmueble.ObtenerPorId(inmuebleId.Value) : new Inmueble { Propietario = new Propietario { Persona = new Persona() } }
                };
            }

            ViewBag.EsRenovacion = esRenovacion;

            return View(contrato);
        }

        [HttpPost]
        public IActionResult CreateOrEdit(Contrato contrato, bool esRenovacion = false)
        {
            decimal montoDecimal = decimal.Parse(contrato.Monto, new CultureInfo("es-AR"));
            if (montoDecimal <= 0)
            {
                ModelState.AddModelError("Monto", "El monto debe ser mayor a cero.");
            }

            if (contrato.Desde >= contrato.Hasta)
                ModelState.AddModelError("Hasta", "La fecha de finalización debe ser posterior a la de inicio.");

            if (!ModelState.IsValid)
            {
                ViewBag.EsRenovacion = esRenovacion;
                // CargarViewBags();
                return View("CreateOrEdit", contrato);
            }

            if (repositorio.ExisteSuperposicion(contrato.InmuebleId, contrato.Desde, contrato.Hasta, contrato.Id))
            {
                contrato.Inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);
                contrato.Inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);

                TempData["Mensaje"] = "❌ El inmueble ya tiene un contrato vigente en ese rango de fechas.";
                ViewBag.EsRenovacion = esRenovacion;
                return View(contrato);
            }


            if (contrato.Id == 0)
            {
                var nuevoId = repositorio.Alta(contrato);
                TempData["Mensaje"] = "Contrato creado correctamente.";

                return esRenovacion
                    ? RedirectToAction("Detalles", new { id = nuevoId })
                    : RedirectToAction("Index");
            }
            else
            {
                repositorio.Modificacion(contrato);
                TempData["Mensaje"] = "Contrato actualizado correctamente.";
                return RedirectToAction("Index");
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Baja(int id)
        {
            var contrato = repositorio.ObtenerPorId(id);
            repositorio.Baja(contrato);
            TempData["Mensaje"] = $"Contrato eliminado correctamente (N° Contrato: {contrato.Id}).";
            return RedirectToAction("Index");
        }
        public IActionResult ModificarEstado(int id, string nuevoEstado)
        {
            var contrato = repositorio.ObtenerPorId(id);
            contrato.Estado = nuevoEstado;
            repositorio.ModificarEstado(contrato);

            if (nuevoEstado == "Rescindido")
            {
                TempData["Mensaje"] = "El contrato fue rescindido. Registrá la multa correspondiente.";
                return RedirectToAction("CreateOrEdit", "Multa", new
                {
                    contratoId = contrato.Id,
                    fechaAviso = DateTime.Today.ToString("yyyy-MM-dd"),
                    fechaTerminacion = contrato.Hasta.ToString("yyyy-MM-dd")
                });
            }

            TempData["Mensaje"] = $"El estado del contrato se cambió a {nuevoEstado}.";
            return RedirectToAction("CreateOrEdit", new { id = contrato.Id });
        }


        public IActionResult Detalles(
                            int id,
                            [FromServices] IRepositorioPago repositorioPago,
                            [FromServices] IRepositorioMulta repositorioMulta)
        {

            var contrato = repositorio.ObtenerPorId(id);
            ViewBag.Pagos = repositorioPago.ObtenerPagosPorContrato(id);
            ViewBag.Multas = repositorioMulta.ObtenerMultasPorContrato(id);


            return View(contrato);

        }

        // private void CargarViewBags()
        // {
        //     ViewBag.Inquilinos = repoInquilino.ObtenerTodos()
        //         .Select(i => new SelectListItem
        //         {
        //             Value = i.Id.ToString(),
        //             Text = $"{i.Persona.ToStringSimple()}"
        //         }).ToList();

        //     ViewBag.Inmuebles = repoInmueble.ObtenerTodos()
        //         .Select(im => new SelectListItem
        //         {
        //             Value = im.Id.ToString(),
        //             Text = $"{im.Direccion} - {im.Localidad}"
        //         }).ToList();
        // }

    }
}