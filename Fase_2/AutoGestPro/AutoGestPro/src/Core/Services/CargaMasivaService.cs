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

    /**
     * Método para validar si el cliente ya existe
     * Validaciones: El ID y Correo deben ser únicos en el sistema para evitar duplicados.
     * @param cliente Cliente a validar
     * @return bool
     */
    private bool clienteTrue(Cliente cliente)
    {
        // Si el cliente es diferente de null, significa que ya existe
        // Si el cliente es igual a null, significa que no existe y se valida que correo sea único
        NodeLinked? clienteTrue = Estructuras.Clientes.SearchNode(cliente.Id);
        NodeLinked? clienteCorreoTrue = Estructuras.Clientes.SearchNode(cliente.Correo);
        
        // Tabla de verdad - Validaciones: id y correo únicos
        /*
         *  clienteTrue | clienteCorreoTrue | Resultado
         *  -------------------------------------------
         *  null        | null              | true   -> Se puede agregar
         *  null        | !null             | false  -> No se puede agregar
         *  !null       | null              | false  -> No se puede agregar
         *  !null       | !null             | false  -> No se puede agregar
         *  -------------------------------------------
         */
        if (clienteTrue != null || clienteCorreoTrue != null)
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
            if (vehiculos != null && Estructuras.Clientes.Length > 0)
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
    
    /**
     * Método para validar si el vehículo ya existe
     * Validaciones: El ID del vehículo debe ser único y se debe verificar
     * que el usuario al que pertenece el vehículo exista en el sistema
     * antes de registrarlo.
     * @param vehiculo Vehículo a validar
     * @return bool
     */
    private bool vehiculoTrue(Vehiculo vehiculo)
    {
        // Si el vehículo es diferente de null, significa que ya existe
        // Si el id_usuario del vehículo no existe, no se puede agregar
        NodeDouble? vehiculoTrue = Estructuras.Vehiculos.SearchNode(vehiculo.Id);
        NodeLinked? usuarioTrue = Estructuras.Clientes.SearchNode(vehiculo.Id_Usuario);
        
        
        //Tabla de verdad - Validaciones: id único y usuario existente
        /*
         *  vehiculoTrue | usuarioTrue | Resultado
         *  -------------------------------------------
         *  null         | null        | true   -> Se puede agregar
         *  null         | !null       | false  -> No se puede agregar
         *  !null        | null        | false  -> No se puede agregar
         *  !null        | !null       | false  -> No se puede agregar
         *  -------------------------------------------
         */
        if (vehiculoTrue != null || usuarioTrue == null)
        {
            return false;
        }

        return true;
    }
}