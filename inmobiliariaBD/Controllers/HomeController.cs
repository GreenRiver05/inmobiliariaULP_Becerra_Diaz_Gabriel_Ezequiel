using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaBD.Models;

namespace inmobiliariaBD.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Retorno la vista con el mismo nombre que el controlador si no especifico otra.
        //sigue la convención de nombres.
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
