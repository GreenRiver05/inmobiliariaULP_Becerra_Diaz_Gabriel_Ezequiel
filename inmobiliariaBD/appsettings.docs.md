  Este archivo guarda configuraciones clave para tu aplicación ASP.NET Core. Su función principal es centralizar valores como cadenas de conexión, niveles de log, parámetros personalizados, y más. Se carga automáticamente al iniciar la app, y permite modificar comportamientos sin recompilar el código. 


{

  "Logging": {
    "LogLevel": {
      "Default": "Information",          // Nivel de log general: muestra eventos informativos
      "Microsoft.AspNetCore": "Warning" // Nivel específico para ASP.NET Core: solo advertencias y errores
    }
  },
  "AllowedHosts": "*" // Permite que la app responda a cualquier host (útil en desarrollo)
}
