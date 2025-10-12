using System.Globalization;
using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
// Inicializa el constructor de la aplicación web, cargando configuración, servicios y argumentos.

builder.Services.AddControllersWithViews();
// Registra el servicio MVC con soporte para controladores y vistas (sin API ni Razor Pages).


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login"; // Ruta al formulario de login
        options.LogoutPath = "/Usuario/Logout"; // Ruta para cerrar sesión
        options.AccessDeniedPath = "/Home/Restringido"; // Vista si no tiene permisos
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    // Política para administradores solamente

    options.AddPolicy("EmpleadoOnly", policy => policy.RequireRole("Empleado"));
    // Política para Empleados solamente

    options.AddPolicy("AdminOEmpleado", policy => policy.RequireRole("Admin", "Empleado"));
    // Política para Empleados y administradores
});

builder.Services.AddSingleton<ChecklistService>();
// Registra el servicio ChecklistService como singleton para inyección de dependencias.

builder.Services.AddScoped<IRepositorioPropietario, RepositorioPropietario>();
builder.Services.AddScoped<IRepositorioPersona, RepositorioPersona>();
builder.Services.AddScoped<IRepositorioInquilino, RepositorioInquilino>();
builder.Services.AddScoped<IRepositorioInmueble, RepositorioInmueble>();
builder.Services.AddScoped<IRepositorioContrato, RepositorioContrato>();
builder.Services.AddScoped<IRepositorioPago, RepositorioPago>();
builder.Services.AddScoped<IRepositorioMulta, RepositorioMulta>();
builder.Services.AddScoped<RepositorioTipoInmueble, RepositorioTipoInmueble>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
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


app.UseAuthentication();
// Habilita la autenticación para identificar usuarios.
app.UseAuthorization();
// Aplica políticas de autorización (aunque no hay autenticación configurada aún).

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Define la ruta por defecto: si no se especifica, usa HomeController y acción Index.

app.Run();
// Inicia la aplicación y comienza a escuchar solicitudes HTTP.
