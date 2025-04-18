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
                    if (clientes.Length > 0)
                    {
                        NodeLinked<Cliente>* cliente = clientes.SearchNode(vehiculo.Id_Usuario);

                        if (cliente != null)
                        {
                            if (vehiculoExiste(vehiculo))
                            {
                                vehiculos.append(vehiculo);
                            }
                        } 
                        else
                        {
                            Console.WriteLine($"❌ No se pueden cargar el Vehiculo al Usuario {vehiculo.Id}.");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"❌ No se pueden cargar vehiculos sin usuarios.");
                        break;
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
                    if (repuestoExiste(repuesto))
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

        if (cliente != null)
        {
            return false;
        }

        return true;
    }

    private bool vehiculoExiste(Vehiculo newVehiculo)
    {
        NodeDouble<Vehiculo>* vehiculo = vehiculos.searchNode(newVehiculo.Id);

        if (vehiculo != null)
        {
            return false;
        }

        return true;
    }

    private bool repuestoExiste(Repuesto repuesto)
    {
        NodeRing<Repuesto>* repuestoNode = repuestos.searchNode(repuesto.Id);

        if (repuestoNode != null)
        {
            return false;
        }

        return true;
    }
}