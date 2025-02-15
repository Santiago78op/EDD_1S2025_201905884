using System;
using System.Collections.Generic;
using System.IO;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Structures;
using Newtonsoft.Json;

namespace AutoGestPro.Core.Services;

public class CargaMasivaService
{
    public Linked_List<Cliente> CargarCleintesDesdeCSV(string rutaArchivo)
    {
        Linked_List<Cliente> clientes = new Linked_List<Cliente>();

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
                            clientes.append(cliente);
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
        Double_List<Vehiculo> vehiculos = new Double_List<Vehiculo>();

        try
        {
            string json = File.ReadAllText(rutaArchivo);
            // Dividir el archivo en secciones
            var secciones = json.Split(new[] { "## " }, StringSplitOptions.RemoveEmptyEntries);

            // Recorrer cada sección
            foreach (var seccion in secciones)
            {
                // Si la sección comienza con "Usuario", se cargan los usuarios
                if (seccion.StartsWith("Vehículos"))
                {
                    // Extraer el JSON de la sección de la sigueitne forma
                    /*
                     * Formato de la sección:
                     * Usuario: [{...}, {...}, {...}]
                     */
                    var jsonUsuarios = seccion.Substring("Vehículos".Length).Trim();
                    // Deserializar el JSON a una lista de objetos Cliente
                    var listaClientes = JsonConvert.DeserializeObject<List<Vehiculo>>(jsonUsuarios);

                    if (listaClientes != null)
                    {
                        foreach (var cliente in listaClientes)
                        {
                            vehiculos.append(cliente);
                        }
                    }
                }
            }

            Console.WriteLine($"✅ {vehiculos.Length} usuarios cargados correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al cargar usuarios: {ex.Message}");
        }

        return vehiculos;
    }

    public RingList<Repuesto> CargarRepuestosDesdeCSV(string rutaArchivo)
    {
        RingList<Repuesto> repuestos = new RingList<Repuesto>();

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

                    repuestos.append(repuesto);
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
}