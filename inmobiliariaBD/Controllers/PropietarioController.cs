using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaBD.Controllers
{
    [Authorize]
    public class PropietarioController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioPropietario repositorio;
        private readonly IRepositorioPersona repositorioPersona;
        private readonly IRepositorioInmueble repositorioInmueble;

        public PropietarioController(IConfiguration config, IRepositorioPropietario repo, IRepositorioPersona repositorioPersona, IRepositorioInmueble repositorioInmueble)
        {
            this.config = config;
            this.repositorio = repo;
            this.repositorioPersona = repositorioPersona;
            this.repositorioInmueble = repositorioInmueble;
        }



        public ActionResult Index(int pagina = 1, string? busqueda = null, bool? estado = null, DateTime? desde = null, DateTime? hasta = null, string? estadoPago = null)
        {
            int cantidadPorPagina = 5;
            var propietarios = repositorio.ObtenerPaginados(pagina, cantidadPorPagina, busqueda, estado, desde, hasta, estadoPago);
            int total = repositorio.ObtenerCantidad(busqueda, estado, desde, hasta, estadoPago);
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / cantidadPorPagina);
            ViewBag.Busqueda = busqueda;
            ViewBag.Estado = estado;
            return View(propietarios);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int? id)
        {
            Propietario p;

            if (id.HasValue)
            {
                //se colaca id.value por que id es nullable y el metodo ObtenerPorId no acepta nullable
                // asi que se usa id.value para obtener el valor real
                p = repositorio.ObtenerPorId(id.Value);
                ViewBag.MostrarModal = false;
            }
            else
            {
                p = new Propietario { Persona = new Persona() };  // Inicializar la propiedad Persona para evitar null reference
                ViewBag.MostrarModal = true;
            }

            return View(p);

        }

        [HttpPost]
        public IActionResult CreateOrEdit(Propietario propietario)
        {

            //Console.WriteLine("Guardando propietario con DNI: " + propietario.Dni);
            // 1. Valido el modelo

            if (propietario.Dni == 0)
            {
                ModelState.AddModelError("Dni", "El DNI es obligatorio.");
            }

            if (propietario.Persona.Telefono == 0)
            {
                ModelState.AddModelError("Persona.Telefono", "El teléfono es obligatorio.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.MostrarModal = false;
                return View("CreateOrEdit", propietario);

            }

            // if (propietario.Persona == null)
            // {
            //     return BadRequest("Faltan los datos Principales.");
            // }
            // no hace falta por que [Required] en el modelo y el ModelState.IsValid lo valida

            // Console.WriteLine(propietario.Persona);
            // Console.WriteLine(propietario);


            propietario.Persona.Dni = propietario.Dni;
            if (propietario.Id == 0 || propietario.Id == null)
            {

                Persona personaExistente = repositorioPersona.ObtenerPorDni(propietario.Dni);

                if (personaExistente == null)
                {
                    repositorioPersona.Alta(propietario.Persona);
                }
                else
                {
                    // Console.WriteLine("Modificando persona existente con DNI: " + propietario.Dni);

                }

                repositorio.Alta(propietario);
                TempData["Mensaje"] = "Propietario creado correctamente.";
            }
            else
            {


                Propietario propietariOriginal = repositorio.ObtenerPorId(propietario.Id.Value);
                int dniAnterior = propietariOriginal.Dni;

                repositorioPersona.Modificar(propietario.Persona, dniAnterior);
                repositorio.Modificacion(propietario);
                TempData["Mensaje"] = "Propietario actualizado correctamente.";
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Baja(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);
            propietario.Persona.Dni = propietario.Dni;
            repositorio.Baja(propietario);
            TempData["Mensaje"] = $"Se Elimino Correctamente al Propietario {propietario.Persona.ToStringSimple()} ";
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult ModificarEstado(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);

            if (propietario == null) return NotFound();

            propietario.Estado = !propietario.Estado;
            repositorio.ModificarEstado(propietario);
            TempData["Mensaje"] = $"El propietario fue {(propietario.Estado ? "activado" : "dado de baja")}.";

            return RedirectToAction("CreateOrEdit", new { id = propietario.Id });
        }

        public IActionResult BuscarPorDni(Propietario propietario)
        {
            Propietario p = repositorio.ObtenerPorDni(propietario.Dni);
            ViewBag.UsuarioEncontrado = true;
            if (p == null)
            {
                p = new Propietario { Persona = new Persona() };
                ViewBag.UsuarioEncontrado = false;
                // TempData["Error"] = "El DNI ya está registrado para otro propietario.";
                //return View("CreateOrEdit", propietario);
            }

            ViewBag.MostrarModal = false;
            return View("CreateOrEdit", p);
        }

        public IActionResult Detalles(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);
            ViewBag.Inmuebles = repositorioInmueble.ObtenerPorPropietario(id);

            return View(propietario);
        }

        [Route("Propietario/Buscar/{q}")]
        public IActionResult Buscar(string q)
        {
            try
            {
                var res = repositorio.Buscar(q);
                return Json(new { datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }



    }
}