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
    public class InquilinoController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioInquilino repositorio;
        private readonly IRepositorioPersona repositorioPersona;

        public InquilinoController(IConfiguration config, IRepositorioInquilino repo, IRepositorioPersona repositorioPersona)
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
            Inquilino i;

            if (id.HasValue)
            {
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


        [HttpPost]
        public IActionResult Guardar(Inquilino inquilino)
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

            Persona personaExistente = repositorioPersona.ObtenerPorDni(inquilino.Dni);


            if (personaExistente == null)
            {
                inquilino.Persona.Dni = inquilino.Dni;
                repositorioPersona.Alta(inquilino.Persona);
            }
            else
            {
                Console.WriteLine("Modificando persona existente con DNI: " + inquilino.Dni);
                inquilino.Persona.Dni = inquilino.Dni;
                repositorioPersona.Modificacion(inquilino.Persona);

            }

            if (inquilino.Id == 0 || inquilino.Id == null)
            {
                repositorio.Alta(inquilino);
                TempData["Mensaje"] = "Inquilino creado correctamente.";
            }
            else
            {
                repositorio.Modificacion(inquilino);
                TempData["Mensaje"] = "Inquilino actualizado correctamente.";
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult AltaBaja(int id)
        {
            var inquilino = repositorio.ObtenerPorId(id);

            if (inquilino == null) return NotFound();

            inquilino.Estado = !inquilino.Estado;
            repositorio.ModificarEstado(inquilino);
            TempData["Mensaje"] = $"El inquilino fue {(inquilino.Estado ? "activado" : "dado de baja")}.";

            return RedirectToAction("CreateOrEdit", new { id = inquilino.Id });
        }


    }
}