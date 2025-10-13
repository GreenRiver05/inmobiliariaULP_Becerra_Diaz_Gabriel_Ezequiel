using System;
using System.Security.Claims;
using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;


namespace inmobiliariaBD.Controllers
{


    public class UsuarioController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioUsuario repo;
        private readonly IRepositorioPersona repositorioPersona;

        private readonly IWebHostEnvironment environment;

        public UsuarioController(IConfiguration config, IRepositorioUsuario repo, IRepositorioPersona repositorioPersona, IWebHostEnvironment env)
        {
            this.config = config;
            this.repo = repo;
            this.repositorioPersona = repositorioPersona;
            this.environment = env;
        }


        [AllowAnonymous] // Permite acceder sin estar logueado
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl; // Guarda la URL original para redirigir después del login
            return View(); // Muestra la vista de login
        }

        [HttpPost]
        [AllowAnonymous] //permite que usuarios no autenticados accedan a esta acción.
        [ValidateAntiForgeryToken] // verifica que el token antifalsificación esté presente y válido (protección contra CSRF).
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



                    var e = repo.ObtenerPorEmail(login.Usuario);

                    // Verifico si el usuario existe y si la clave coincide
                    if (e == null || e.Contraseña != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl; //Guarda el returnUrl para mantener el flujo.
                        return View();
                    }

                    // Verifico si el usuario está activo 
                    if (e.Estado is false)
                    {
                        ModelState.AddModelError("", "El usuario está inactivo. Contacte al administrador.");
                        TempData["returnUrl"] = returnUrl; //Guarda el returnUrl para mantener el flujo.
                        return View();
                    }

                    // Crea los claims (datos que se guardan en la cookie de sesión)
                    var claims = new List<Claim>
                    {
                       new Claim(ClaimTypes.Name, $"{e.Persona.Nombre} {e.Persona.Apellido}"), // Nombre completo aparece en el menú
                       new Claim(ClaimTypes.NameIdentifier, e.Id.ToString()), // ID del usuario
                       new Claim(ClaimTypes.Email, e.Persona.Correo), // Email del usuario
                       new Claim(ClaimTypes.Role, e.RolNombre),
                       new Claim("Avatar", e.Avatar ?? string.Empty) // Avatar del usuario o cadena vacía si es null
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

        public async Task<IActionResult> Logout()
        {
            // Cierra la sesión actual y elimina la cookie de autenticación
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige al Home o al Login después de cerrar sesión
            return RedirectToAction("Index", "Home");
        }


        // GET: Usuario
        public IActionResult Index(int pagina = 1, string? busqueda = null, bool? estado = null, DateTime? desde = null, DateTime? hasta = null, string? estadoPago = null)
        {
            int cantidadPorPagina = 5;

            // Aunque estado no lo uso en Pago, lo dejo para cumplir con la firma común
            var usuarios = repo.ObtenerPaginados(pagina, cantidadPorPagina, busqueda, estado, desde, hasta, estadoPago);
            int total = repo.ObtenerCantidad(busqueda, estado, desde, hasta, estadoPago);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / cantidadPorPagina);
            ViewBag.Busqueda = busqueda;
            ViewBag.Estado = estado;
            return View(usuarios);
        }


        // GET: /Usuario/CreateOrEdit
        [HttpGet]
        public IActionResult CreateOrEdit(int? id, bool esPerfil = false)
        {
            Usuario u = id.HasValue ? repo.ObtenerPorId(id.Value) : new Usuario { Persona = new Persona() };
            ViewBag.EsPerfil = esPerfil;
            return View(u);
        }

        // POST: /Usuario/CreateOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(Usuario usuario, bool esPerfil = false, bool eliminarAvatar = false)

        {

            if (usuario.Dni == 0)
                ModelState.AddModelError("Dni", "El DNI es obligatorio.");

            if (usuario.Id == 0 && string.IsNullOrEmpty(usuario.Contraseña))
                ModelState.AddModelError("Contraseña", "La contraseña es obligatoria para un nuevo usuario.");

            if (!ModelState.IsValid)
                return View(usuario);

            // Hashear la contraseña usando PBKDF2 con salt fijo 
            if (!string.IsNullOrEmpty(usuario.Contraseña))
            {
                string salt = config["Salt"];
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: usuario.Contraseña,
                    salt: System.Text.Encoding.ASCII.GetBytes(salt),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                usuario.Contraseña = hashed;
            }
            else if (usuario.Id != 0)
            {
                // Si está editando y no ingresa clave, mantener la actual
                var original = repo.ObtenerPorId(usuario.Id);
                usuario.Contraseña = original.Contraseña;
            }

            if (eliminarAvatar && usuario.Id != 0)
            {
                var original = repo.ObtenerPorId(usuario.Id);
                if (!string.IsNullOrEmpty(original.Avatar))
                {
                    string rutaCompleta = Path.Combine(environment.WebRootPath, original.Avatar);
                    if (System.IO.File.Exists(rutaCompleta))
                    {
                        System.IO.File.Delete(rutaCompleta);
                    }
                }
                usuario.Avatar = null;
            }



            // Procesar avatar si se subió
            if (usuario.AvatarFile != null)
            {
                string ruta = Path.Combine(environment.WebRootPath, "uploads");

                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }


                string nombreArchivo = $"avatar_{usuario.Id}_{Path.GetExtension(usuario.AvatarFile.FileName)}";
                string rutaCompleta = Path.Combine(ruta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    usuario.AvatarFile.CopyTo(stream);
                }

                usuario.Avatar = Path.Combine("uploads", nombreArchivo);
            }
            else if (usuario.Id != 0)
            {
                // Si está editando y no sube avatar, mantener el actual
                var original = repo.ObtenerPorId(usuario.Id);
                usuario.Avatar = original.Avatar;
            }

            usuario.Persona.Dni = usuario.Dni;



            if (usuario.Id == 0)
            {
                var personaExistente = repositorioPersona.ObtenerPorDni(usuario.Dni);
                if (personaExistente == null)
                {
                    repositorioPersona.Alta(usuario.Persona);
                }
                usuario.Estado = true;
                repo.Alta(usuario);
                TempData["Mensaje"] = "Usuario creado correctamente.";
            }
            else
            {
                var usuarioOriginal = repo.ObtenerPorId(usuario.Id);
                int dniAnterior = usuarioOriginal.Dni;

                repositorioPersona.Modificar(usuario.Persona, dniAnterior);
                repo.Modificacion(usuario);
                TempData["Mensaje"] = esPerfil
                                        ? "Perfil actualizado correctamente."
                                        : "Usuario actualizado correctamente.";
            }
            if (esPerfil)
            {
                ViewBag.EsPerfil = true;
                return View(usuario); // Me quedo en la misma vista
            }

            return RedirectToAction("Index", "Usuario"); // vuelvo al listado
        }


        // POST: /Usuario/Baja
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Baja(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            repo.Baja(usuario);
            TempData["Mensaje"] = $"Se eliminó correctamente al usuario {usuario.Persona.Nombre} {usuario.Persona.Apellido}.";
            return RedirectToAction("Index");
        }

        // POST: /Usuario/ModificarEstado
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult ModificarEstado(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            if (usuario == null) return NotFound();

            usuario.Estado = usuario.Estado is true ? false : true;
            repo.ModificarEstado(usuario);
            TempData["Mensaje"] = $"El usuario fue {(usuario.Estado is true ? "activado" : "dado de baja")}.";
            return RedirectToAction("CreateOrEdit", new { id = usuario.Id });
        }

        // GET: /Usuario/Detalles
        public IActionResult Detalles(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            return View(usuario);
        }






    }

}