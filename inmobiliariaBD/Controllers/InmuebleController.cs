using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaBD.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repositorioPropietario;


        public InmuebleController(IConfiguration config, IRepositorioInmueble repo, IRepositorioPropietario repoPropietario)
        {
            this.config = config;
            this.repositorio = repo;
            this.repositorioPropietario = repoPropietario;

        }

        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int? id)
        {
            Inmueble i;
            
            if (id.HasValue)
            {
                i = repositorio.ObtenerPorId(id.Value);
                
            }
            else
            {
                i = new Inmueble
                {
                    Propietario = new Propietario { Persona = new Persona() },
                   TipoInmueble = new TipoInmueble()   
                };  
            }
            return View(i);
        }


        [HttpPost]
        public IActionResult CreateOrEdit(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                if (inmueble.Id > 0)
                {
                    repositorio.Modificacion(inmueble);
                }
                else
                {
                    repositorio.Alta(inmueble);
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                return View(inmueble);
            }
        }



        public ActionResult Details(int id)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            return View(inmueble);
        }



    }

}