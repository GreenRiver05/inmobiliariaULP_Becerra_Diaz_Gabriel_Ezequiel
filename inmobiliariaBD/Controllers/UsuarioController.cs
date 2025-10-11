using System.Security.Claims;
using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;


namespace inmobiliariaBD.Controllers
{

    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioUsuario repositorio;
        private readonly IRepositorioPersona repositorioPersona;

        public UsuarioController(IConfiguration config, IRepositorioUsuario repo, IRepositorioPersona repositorioPersona)
        {
            this.config = config;
            this.repositorio = repo;
            this.repositorioPersona = repositorioPersona;
        }

        // GET: Usuario
        public ActionResult Index(int pagina = 1, string? busqueda = null, bool? estado = null)
        {
            int cantidadPorPagina = 5;
            var usuarios = repositorio.ObtenerPaginados(pagina, cantidadPorPagina, busqueda, estado);
            int total = repositorio.ObtenerCantidad(busqueda, estado);
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / cantidadPorPagina);
            ViewBag.Busqueda = busqueda;
            ViewBag.Estado = estado;
            return View(usuarios);
        }

        [AllowAnonymous] // Permite acceder sin estar logueado
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl; // Guarda la URL original para redirigir después del login
            return View(); // Muestra la vista de login
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                // Si no hay returnUrl, redirige al Home
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();

                if (ModelState.IsValid)
                {
                    // Hasheo la clave ingresada usando PBKDF2 con un salt fijo
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));



                    var e = repositorio.ObtenerPorEmail(login.Usuario);

                    // Verifico si el usuario existe y si la clave coincide
                    if (e == null || e.Contraseña != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    // Verifico si el usuario está activo 
                    if (e.Estado is false)
                    {
                        ModelState.AddModelError("", "El usuario está inactivo. Contacte al administrador.");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    // Crea los claims (datos que se guardan en la cookie de sesión)
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Persona.Correo),
                        new Claim("FullName", e.Persona.Nombre + " " + e.Persona.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };


                    // Crea la identidad con los claims
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Inicia la sesión con cookies
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    TempData.Remove("returnUrl"); // Limpia la URL temporal
                    return Redirect(returnUrl); // Redirige a la página original o al Home
                }

                TempData["returnUrl"] = returnUrl;
                return View(); // Si el modelo no es válido, vuelve a mostrar el formulario
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message); // Muestra el error si algo falla
                return View(); // Vuelve a mostrar el formulario
            }
        }


    }

}