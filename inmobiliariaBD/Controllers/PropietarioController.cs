using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace inmobiliariaBD.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioPropietario repositorio;
        private readonly IRepositorioPersona repositorioPersona;

        public PropietarioController(IConfiguration config, IRepositorioPropietario repo, IRepositorioPersona repositorioPersona)
        {
            this.config = config;
            this.repositorio = repo;
            this.repositorioPersona = repositorioPersona;
        }



        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
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

            Persona personaExistente = repositorioPersona.ObtenerPorDni(propietario.Dni);
            Console.WriteLine(propietario.Persona);
            Console.WriteLine(propietario);

            if (personaExistente == null)
            {
                propietario.Persona.Dni = propietario.Dni;
                repositorioPersona.Alta(propietario.Persona);
            }
            else
            {
                Console.WriteLine("Modificando persona existente con DNI: " + propietario.Dni);
                propietario.Persona.Dni = propietario.Dni;
                repositorioPersona.Modificacion(propietario.Persona);

            }

            if (propietario.Id == 0 || propietario.Id == null)
            {
                repositorio.Alta(propietario);
                TempData["Mensaje"] = "Propietario creado correctamente.";
            }
            else
            {
                repositorio.Modificacion(propietario);
                TempData["Mensaje"] = "Propietario actualizado correctamente.";
            }

            return RedirectToAction("Index");
        }

        // [HttpGet]
        // public IActionResult Baja(int id)
        // {

        //     var propietario = repositorio.ObtenerPorId(id);
        //     return View(propietario);
        // }

        [HttpPost]
        public IActionResult Baja(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);

            repositorio.Baja(propietario);
            TempData["Mensaje"] = $"Se Elimino Correctamente al Propietario {propietario.Persona.ToStringSimple()} ";
            return RedirectToAction("Index");

        }

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
    }

}