using System;
using System.Collections.Generic;
using System.IO;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
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
            
            // Procesar Json sin secciones
            var listaClientes = JsonConvert.DeserializeObject<Cliente[]>(json);
            
            if (clientes != null)
            {
                foreach (var cliente in listaClientes)
                {
                    if (clienteExiste(cliente))
                    {
                        clientes.append(cliente);
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
            
            // Procesar Json sin secciones
            var listaVehiculos = JsonConvert.DeserializeObject<Vehiculo[]>(json);
            
            if (vehiculos != null)
            {
                foreach (var vehiculo in listaVehiculos)
                {
                    if (!vehiculoExiste(vehiculo))
                    {
                        vehiculos.append(vehiculo);
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
            
            // Procesar Json sin secciones
            var listaRepuestos = JsonConvert.DeserializeObject<Repuesto[]>(json);
            
            if (repuestos != null)
            {
                foreach (var repuesto in listaRepuestos)
                {
                    if (!repuestoExiste(repuesto))
                    {
                        repuestos.append(repuesto);
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
    
    private bool clienteExiste(Cliente newCliente)
    {
        NodeLinked<Cliente>* cliente = clientes.SearchNode(newCliente.Id);
        
        if(cliente != null && cliente->_data is Cliente c && c.Id == newCliente.Id)
        {
            return false;
        }
        return true;
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