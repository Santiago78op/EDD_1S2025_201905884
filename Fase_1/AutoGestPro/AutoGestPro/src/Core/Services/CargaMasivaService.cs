using System;
using System.Collections.Generic;
using System.IO;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

public class CargaMasivaService
{
    public Linked_List<Cliente> CargarUsuariosDesdeCSV(string rutaArchivo)
        {   
            
            Cliente cliente_1 = new Cliente(0, "Nombre", "Apellido", "Email", "Telefono");
            Linked_List<Cliente> clientes = new Linked_List<Cliente>(cliente_1);
            clientes.remove(cliente_1);
            
            try
            {
                using (var reader = new StreamReader(rutaArchivo))
                {
                    string linea;
                    while ((linea = reader.ReadLine()) != null)
                    {
                        string[] datos = linea.Split(',');

                        if (datos.Length < 3) continue;

                        Cliente cliente = new Cliente
                        (
                            int.Parse(datos[0]),
                            datos[1],
                            datos[2],
                            datos[3],
                            datos[4]
                        );

                        clientes.append(cliente);
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

        public List<Vehiculo> CargarVehiculosDesdeCSV(string rutaArchivo)
        {
            List<Vehiculo> vehiculos = new List<Vehiculo>();

            try
            {
                using (var reader = new StreamReader(rutaArchivo))
                {
                    string linea;
                    while ((linea = reader.ReadLine()) != null)
                    {
                        string[] datos = linea.Split(',');

                        if (datos.Length < 3) continue;

                        Vehiculo vehiculo = new Vehiculo
                        (
                            int.Parse(datos[0]),
                            int.Parse(datos[1]),
                            datos[2],
                            datos[3],
                            datos[4]
                        );

                        vehiculos.Add(vehiculo);
                    }
                }
                Console.WriteLine($"✅ {vehiculos.Count} vehículos cargados correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al cargar vehículos: {ex.Message}");
            }

            return vehiculos;
        }

        public List<Repuesto> CargarRepuestosDesdeCSV(string rutaArchivo)
        {
            List<Repuesto> repuestos = new List<Repuesto>();

            try
            {
                using (var reader = new StreamReader(rutaArchivo))
                {
                    string linea;
                    while ((linea = reader.ReadLine()) != null)
                    {
                        string[] datos = linea.Split(',');

                        if (datos.Length < 3) continue;

                        Repuesto repuesto = new Repuesto
                        (
                            int.Parse(datos[0]),
                            datos[1],
                            datos[2],
                            double.Parse(datos[3])
                        );

                        repuestos.Add(repuesto);
                    }
                }
                Console.WriteLine($"✅ {repuestos.Count} repuestos cargados correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al cargar repuestos: {ex.Message}");
            }

            return repuestos;
        }
}