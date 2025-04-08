using System.Text.Json;
using AutoGestPro.Core.Models;

namespace AutoGestPro.Core.Services;

/// <summary>
/// Servicio para la gestión de carga masiva.
/// El usuario administrador podrá realizar carga masiva de las siguientes
/// entidades “USUARIOS”, “VEHICULOS”, “REPUESTOS”, “SERVICIOS”.
/// </summary>
public class CargaMasivaService
{
    // 📌 Cargar clientes desde un archivo JSON
    public bool CargarUsuariosDesdeArchivo(string rutaArchivo)
    {
        try
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(rutaArchivo);
            // Deserializar el contenido a una lista de clientes
            var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json);
            if (usuarios != null)
            {
                // Guardar los clientes en la base de datos
                foreach (var usuario in usuarios)
                {
                    // Aquí se llamaría al método para guardar el cliente en la base de datos
                    // GuardarCliente(cliente);
                }
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar usuarios: {ex.Message}");
        }
        return false;
    }
}