using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaBD.Controllers
{

    [Authorize]
    public class ChecklistController : Controller
    {
        private readonly ChecklistService service; // Servicio inyectado para manejar la lógica del checklist Readonly porque no se va a modificar

        public ChecklistController(ChecklistService service)
        {
            this.service = service; // Guarda la instancia del servicio
        }

        public IActionResult Index()
        {
            // Obtiene los ítems y los pasa a la vista
            var items = service.GetItems();
            return View(items);
        }

        [HttpPost]
        public IActionResult Toggle(int id)
        {
            // Cambia el estado del ítem con el ID dado
            service.Toggle(id);
            return RedirectToAction("Index");
        }
    }


}
