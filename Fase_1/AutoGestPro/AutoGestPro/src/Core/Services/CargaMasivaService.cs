using System;
using System.Collections.Generic;
using System.IO;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Structures;
using Newtonsoft.Json;

namespace AutoGestPro.Core.Services;

public unsafe class CargaMasivaService
{   
    public static Linked_List<Cliente> clientes = new Linked_List<Cliente>();
    public static Double_List<Vehiculo> vehiculos = new Double_List<Vehiculo>();
    public static RingList<Repuesto> repuestos = new RingList<Repuesto>();
    public static ListQueue<Servicio> servicios = new ListQueue<Servicio>();
    
    public Linked_List<Cliente> CargarCleintesDesdeCSV(string rutaArchivo)
    {
        try
        {
            string json = File.ReadAllText(rutaArchivo);
            // Dividir el archivo en secciones
            var secciones = json.Split(new[] { "## " }, StringSplitOptions.RemoveEmptyEntries);

            // Recorrer cada sección
            foreach (var seccion in secciones)
            {
                // Si la sección comienza con "Usuario", se cargan los usuarios
                if (seccion.StartsWith("Usuario"))
                {
                    // Extraer el JSON de la sección de la sigueitne forma
                    /*
                     * Formato de la sección:
                     * Usuario: [{...}, {...}, {...}]
                     */
                    var jsonUsuarios = seccion.Substring("Usuario".Length).Trim();
                    // Deserializar el JSON a una lista de objetos Cliente
                    var listaClientes = JsonConvert.DeserializeObject<List<Cliente>>(jsonUsuarios);

                    if (listaClientes != null)
                    {
                        foreach (var cliente in listaClientes)
                        {
                            if (!clienteExiste(cliente))
                            {
                                clientes.append(cliente);
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"✅ {clientes.Length} usuarios cargados correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al cargar usuarios: {ex.Message}");
        }

        return clientes;
    }

    public Double_List<Vehiculo> CargarVehiculosDesdeCSV(string rutaArchivo)
    {
        try
        {
            string json = File.ReadAllText(rutaArchivo);
            // Dividir el archivo en secciones
            var secciones = json.Split(new[] { "## " }, StringSplitOptions.RemoveEmptyEntries);

            // Recorrer cada sección
            foreach (var seccion in secciones)
            {
                // Si la sección comienza con "Vehiculos", se cargan los usuarios
                if (seccion.StartsWith("Vehículos"))
                {
                    // Extraer el JSON de la sección de la sigueitne forma
                    /*
                     * Formato de la sección:
                     * Vehiculos: [{...}, {...}, {...}]
                     */
                    var jsonVehiculos = seccion.Substring("Vehículos".Length).Trim();
                    // Deserializar el JSON a una lista de objetos Vehiculo
                    var listaCVehiculos = JsonConvert.DeserializeObject<List<Vehiculo>>(jsonVehiculos);

                    if (listaCVehiculos != null)
                    {
                        foreach (var vehiculo in listaCVehiculos)
                        {
                            if (!vehiculoExiste(vehiculo))
                            {
                                vehiculos.append(vehiculo);
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"✅ {vehiculos.Length} vehiculos cargados correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al cargar vehiculos: {ex.Message}");
        }

        return vehiculos;
    }

    public RingList<Repuesto> CargarRepuestosDesdeCSV(string rutaArchivo)
    {
        try
        {
            string json = File.ReadAllText(rutaArchivo);
            // Dividir el archivo en secciones
            var secciones = json.Split(new[] { "## " }, StringSplitOptions.RemoveEmptyEntries);

            // Recorrer cada sección
            foreach (var seccion in secciones)
            {
                // Si la sección comienza con "Repuesto", se cargan los usuarios
                if (seccion.StartsWith("Repuestos"))
                {
                    // Extraer el JSON de la sección de la sigueitne forma
                    /*
                     * Formato de la sección:
                     * Repuesto: [{...}, {...}, {...}]
                     */
                    var jsonRepuestos = seccion.Substring("Repuestos".Length).Trim();
                    // Deserializar el JSON a una lista de objetos Repuesto
                    var listaRepuestos = JsonConvert.DeserializeObject<List<Repuesto>>(jsonRepuestos);

                    if (listaRepuestos != null)
                    {
                        foreach (var repuesto in listaRepuestos)
                        {
                            if (!repuestoExiste(repuesto))
                            {
                                repuestos.append(repuesto);
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"✅ {repuestos.Length} repuestos cargados correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al cargar repuestos: {ex.Message}");
        }

        return repuestos;
    }
    
    private bool clienteExiste(Cliente cliente)
    {
        for (int i = 0; i < clientes.Length; i++)
        {
            if (clientes.GetNode(i)->_data is Cliente c)
            {
                if (c.Equals(cliente))
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    private bool vehiculoExiste(Vehiculo vehiculo)
    {
        for (int i = 0; i < vehiculos.Length; i++)
        {
            if (vehiculos.GetNode(i)->_data is Vehiculo v)
            {
                if (v.Equals(vehiculo))
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    private bool repuestoExiste(Repuesto repuesto)
    {
        for (int i = 0; i < repuestos.Length; i++)
        {
            if (repuestos.GetNode(i)->_data is Repuesto r)
            {
                if (r.Equals(repuesto))
                {
                    return true;
                }
            }
        }

        return false;
    }
}