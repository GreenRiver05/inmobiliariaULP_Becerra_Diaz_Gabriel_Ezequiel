using System.Globalization;
using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inmobiliariaBD.Controllers
{
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

        public IActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int? id)
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
                    Inquilino = new Inquilino { Persona = new Persona() },
                    Inmueble = new Inmueble
                    {
                        Propietario = new Propietario { Persona = new Persona() }
                    }
                };
            }

            CargarViewBags();

            return View(contrato);
        }

        [HttpPost]
        public IActionResult CreateOrEdit(Contrato contrato)
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
                CargarViewBags();
                return View("CreateOrEdit", contrato);
            }

            if (contrato.Id == 0)
            {
                repositorio.Alta(contrato);
                TempData["Mensaje"] = "Contrato creado correctamente.";
            }
            else
            {
                repositorio.Modificacion(contrato);
                TempData["Mensaje"] = "Contrato actualizado correctamente.";
            }

            return RedirectToAction("Index");
        }

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
            TempData["Mensaje"] = $"El estado del contrato se cambio a {nuevoEstado}.";
            return RedirectToAction("CreateOrEdit", new { id = contrato.Id });
        }

        private void CargarViewBags()
        {
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos()
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.Persona.ToStringSimple()}"
                }).ToList();

            ViewBag.Inmuebles = repoInmueble.ObtenerTodos()
                .Select(im => new SelectListItem
                {
                    Value = im.Id.ToString(),
                    Text = $"{im.Direccion} - {im.Localidad}"
                }).ToList();
        }

    }
}