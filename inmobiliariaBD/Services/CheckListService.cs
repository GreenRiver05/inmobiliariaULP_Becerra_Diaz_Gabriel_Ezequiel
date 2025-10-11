using System.Text.Json; // Para leer y escribir archivos JSON
using inmobiliariaBD.Models; // Ajustá el namespace según tu estructura

public class ChecklistService
{
    private readonly string filePath; // Ruta completa al archivo checklist.json

    public ChecklistService(IWebHostEnvironment env)
    {
        // Construye la ruta al archivo JSON dentro de App_Data
        filePath = Path.Combine(env.ContentRootPath, "App_Data", "checklist.json");
    }

    public List<CheckList> GetItems()
    {
        // Si el archivo no existe, devuelve una lista vacía
        if (!System.IO.File.Exists(filePath)) return new List<CheckList>();

        // Lee el contenido del archivo
        var json = System.IO.File.ReadAllText(filePath);

        // Deserializa el JSON a una lista de objetos ChecklistItem
        return JsonSerializer.Deserialize<List<CheckList>>(json) ?? new();
    }

    public void Toggle(int id)
    {
        // Carga todos los ítems
        var items = GetItems();

        // Busca el ítem por ID
        var item = items.FirstOrDefault(i => i.Id == id);

        if (item != null)
        {
            // Cambia el estado (true ↔ false)
            item.Completado = !item.Completado;

            // Convierte la lista actualizada a JSON
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });


            // Guarda el nuevo contenido en el archivo
            System.IO.File.WriteAllText(filePath, json);
        }
    }
}
