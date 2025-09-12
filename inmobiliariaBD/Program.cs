using System.Globalization;
using inmobiliariaBD.Models;

var builder = WebApplication.CreateBuilder(args);
// Inicializa el constructor de la aplicación web, cargando configuración, servicios y argumentos.

builder.Services.AddControllersWithViews();
// Registra el servicio MVC con soporte para controladores y vistas (sin API ni Razor Pages).

builder.Services.AddScoped<IRepositorioPropietario, RepositorioPropietario>();
builder.Services.AddScoped<IRepositorioPersona, RepositorioPersona>();
builder.Services.AddScoped<IRepositorioInquilino, RepositorioInquilino>();
builder.Services.AddScoped<IRepositorioInmueble, RepositorioInmueble>();
builder.Services.AddScoped<IRepositorioContrato, RepositorioContrato>();
// Registra los repositorios personalizados para inyección de dependencias con ciclo de vida Scoped.


//para trabajar con fechas en formato argentino pero no lo pude implementar bien
// var cultura = new CultureInfo("es-AR");
// CultureInfo.DefaultThreadCurrentCulture = cultura;
// CultureInfo.DefaultThreadCurrentUICulture = cultura;



var app = builder.Build();
// Compila la aplicación con los servicios configurados.


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // En producción, redirige errores a una vista controlada en /Home/Error.

    app.UseHsts();
    // Activa HTTP Strict Transport Security (HSTS) por 30 días para reforzar HTTPS.
}

app.UseHttpsRedirection();
// Redirige automáticamente las solicitudes HTTP a HTTPS.

app.UseStaticFiles();
// Habilita el acceso a archivos estáticos (CSS, JS, imágenes) desde wwwroot.

app.UseRouting();
// Habilita el sistema de enrutamiento para mapear URLs a controladores.

app.UseAuthorization();
// Aplica políticas de autorización (aunque no hay autenticación configurada aún).

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Define la ruta por defecto: si no se especifica, usa HomeController y acción Index.

app.Run();
// Inicia la aplicación y comienza a escuchar solicitudes HTTP.
