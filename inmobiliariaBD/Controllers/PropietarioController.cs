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
            Propietario modelo = id.HasValue
                ? repositorio.ObtenerPorId(id.Value)
                : new Propietario();

            return View(modelo);
        }

        [HttpPost]
        public IActionResult Guardar(Propietario propietario)
        {


            // 1. Validación del modelo
            if (!ModelState.IsValid)
            {
                // Podés devolver la vista con los errores para que el usuario los corrija
                return View("CreateOrEdit", propietario);

            }

            // if (propietario.Persona == null)
            // {
            //     return BadRequest("Faltan los datos Principales.");
            // }
            // no hace falta por que ya usamos [Required] en el modelo y el ModelState.IsValid lo valida

            Persona personaExistente = repositorioPersona.ObtenerPorDni(propietario.Dni);

            if (personaExistente == null)
            {
                repositorioPersona.Alta(propietario.Persona);
            }
            else
            {
                repositorioPersona.Modificacion(propietario.Persona);

            }

            if (propietario.Id == 0)
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int res = repositorio.Alta(p);
                    if (res > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo agregar el propietario");
                        return View(p);
                    }
                }
                else
                {
                    return View(p);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(p);
            }
        }


        public ActionResult Edit(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int res = repositorio.Modificacion(p);
                    if (res > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo modificar el propietario");
                        return View(p);
                    }
                }
                else
                {
                    return View(p);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(p);
            }
        }

        public ActionResult Delete(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario p)
        {
            try
            {
                int res = repositorio.ModificarEstado(id, false);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(p);
            }

        }
    }

}