using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace inmobiliariaBD.Controllers
{
    [Authorize]
    public class InquilinoController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioInquilino repositorio;
        private readonly IRepositorioPersona repositorioPersona;

        private readonly IRepositorioContrato repositorioContrato;

        public InquilinoController(IConfiguration config, IRepositorioInquilino repo, IRepositorioPersona repositorioPersona, IRepositorioContrato repositorioContrato)
        {
            this.config = config;
            this.repositorio = repo;
            this.repositorioPersona = repositorioPersona;
            this.repositorioContrato = repositorioContrato;
        }



        public ActionResult Index(int pagina = 1, string? busqueda = null, bool? estado = null)
        {
            int cantidadPorPagina = 5;
            var inquilinos = repositorio.ObtenerPaginados(pagina, cantidadPorPagina, busqueda, estado);
            int total = repositorio.ObtenerCantidad(busqueda, estado);
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / cantidadPorPagina);
            ViewBag.Busqueda = busqueda;
            ViewBag.Estado = estado;
            return View(inquilinos);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int? id)
        {
            Inquilino i;

            if (id.HasValue)
            {
                //se colaca id.value por que id es nullable y el metodo ObtenerPorId no acepta nullable
                // asi que se usa id.value para obtener el valor real
                i = repositorio.ObtenerPorId(id.Value);
                ViewBag.MostrarModal = false;
            }
            else
            {
                i = new Inquilino { Persona = new Persona() };  // Inicializar la propiedad Persona para evitar null reference
                ViewBag.MostrarModal = true;
            }

            return View(i);

        }

        [HttpPost]
        public IActionResult CreateOrEdit(Inquilino inquilino)
        {

            // 1. Valido el modelo

            if (inquilino.Dni == 0)
            {
                ModelState.AddModelError("Dni", "El DNI es obligatorio.");
            }

            if (inquilino.Persona.Telefono == 0)
            {
                ModelState.AddModelError("Persona.Telefono", "El teléfono es obligatorio.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.MostrarModal = false;
                return View("CreateOrEdit", inquilino);

            }

            // if (inquilino.Persona == null)
            // {
            //     return BadRequest("Faltan los datos Principales.");
            // }
            // no hace falta por que [Required] en el modelo y el ModelState.IsValid lo valida

            inquilino.Persona.Dni = inquilino.Dni;
            if (inquilino.Id == 0 || inquilino.Id == null)
            {

                Persona personaExistente = repositorioPersona.ObtenerPorDni(inquilino.Dni);

                if (personaExistente == null)
                {
                    repositorioPersona.Alta(inquilino.Persona);
                }
                else
                {
                    // Console.WriteLine("Modificando persona existente con DNI: " + inquilino.Dni);

                }

                repositorio.Alta(inquilino);
                TempData["Mensaje"] = "inquilino creado correctamente.";
            }
            else
            {


                Inquilino inquilinoriginal = repositorio.ObtenerPorId(inquilino.Id.Value);
                int dniAnterior = inquilinoriginal.Dni;

                repositorioPersona.Modificar(inquilino.Persona, dniAnterior);
                repositorio.Modificacion(inquilino);
                TempData["Mensaje"] = "inquilino actualizado correctamente.";
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Baja(int id)
        {
            var inquilino = repositorio.ObtenerPorId(id);
            inquilino.Persona.Dni = inquilino.Dni;
            repositorio.Baja(inquilino);
            TempData["Mensaje"] = $"Se Elimino Correctamente al Inquilino {inquilino.Persona.ToStringSimple()} ";
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult ModificarEstado(int id)
        {
            var inquilino = repositorio.ObtenerPorId(id);

            if (inquilino == null) return NotFound();

            inquilino.Estado = !inquilino.Estado;
            repositorio.ModificarEstado(inquilino);
            TempData["Mensaje"] = $"El inquilino fue {(inquilino.Estado ? "activado" : "dado de baja")}.";

            return RedirectToAction("CreateOrEdit", new { id = inquilino.Id });
        }

        [HttpPost]
        public IActionResult BuscarPorDni(Inquilino inquilino)
        {

            Inquilino i = repositorio.ObtenerPorDni(inquilino.Dni);
            ViewBag.UsuarioEncontrado = true;

            if (i == null)
            {
                i = new Inquilino { Persona = new Persona() };
                ViewBag.UsuarioEncontrado = false;

                // TempData["Error"] = "El DNI ya está registrado para otro Inquilino.";
                //return View("CreateOrEdit", inquilino);
            }

            ViewBag.MostrarModal = false;
            return View("CreateOrEdit", i);
        }

        public IActionResult Detalles(int id)
        {
            var inquilino = repositorio.ObtenerPorId(id);
            ViewBag.Contratos = repositorioContrato.ObtenerPorInquilino(id);

            return View(inquilino);
        }

    }
}