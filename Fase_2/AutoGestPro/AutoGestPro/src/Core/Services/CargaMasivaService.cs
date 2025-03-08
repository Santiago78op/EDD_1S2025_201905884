using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using Newtonsoft.Json;

namespace AutoGestPro.Core.Services;

public class CargaMasivaService
{
    public void CargarClientesJson(string path)
    {
        // 📌 Cargar clientes desde un archivo JSON
        try
        {
            string json = File.ReadAllText(path);
            
            // 📌 Deserializar el JSON a una lista de clientes
            var clientes = JsonConvert.DeserializeObject<Cliente[]>(json);
            
            // 📌 Guardar los clientes en la lista simple
            if (clientes != null)
            {
                foreach (var cliente in clientes)
                {
                    if (clienteTrue(cliente))
                    {
                        Estructuras.Clientes.Append(cliente);
                    }
                }
            }
            
            Console.WriteLine($"✅ {Estructuras.Clientes.Length} clientes cargados correctamente.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error al cargar los clientes desde el archivo JSON");
        }
    }

    private bool clienteTrue(Cliente cliente)
    {
        NodeLinked? clienteTrue = Estructuras.Clientes.SearchNode(cliente.Id);
        
        if (clienteTrue != null)
        {
            return false;
        }

        return true;
    }
    
    public void CargarVehiculosJson(String path)
    {
        // 📌 Cargar vehículos desde un archivo JSON
        try
        {
            string json = File.ReadAllText(path);
            
            // 📌 Deserializar el JSON a una lista de vehículos
            var vehiculos = JsonConvert.DeserializeObject<Vehiculo[]>(json);
            
            // 📌 Guardar los vehículos en la lista doble
            if (vehiculos != null)
            {
                foreach (var vehiculo in vehiculos)
                {
                    if(vehiculoTrue(vehiculo))
                    {
                        Estructuras.Vehiculos.Append(vehiculo);
                    }
                }
            }
            
            Console.WriteLine($"✅ {Estructuras.Vehiculos.Length} vehículos cargados correctamente.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error al cargar los vehículos desde el archivo JSON");
        }
    }
    
    private bool vehiculoTrue(Vehiculo vehiculo)
    {
        NodeDouble? vehiculoTrue = Estructuras.Vehiculos.SearchNode(vehiculo.Id);
        
        if (vehiculoTrue != null)
        {
            return false;
        }

        return true;
    }
}