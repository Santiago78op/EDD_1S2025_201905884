using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AutoGestPro.Core.Services;

/// <summary>
/// Servicio para la gestión de carga masiva.
/// El usuario administrador podrá realizar carga masiva de las siguientes
/// entidades “USUARIOS”, “VEHICULOS”, “REPUESTOS”, “SERVICIOS”.
/// </summary>
public class CargaMasivaService
{
    private readonly ServicioUsuarios _servicio = Estructuras.Clientes;
    private readonly DoubleList _vehiculos = Estructuras.Vehiculos;
    public string[][] DatosCargados { get; set; }

    /// <summary>
    /// Gestiona la carga masiva dependiendo del tipo de entidad.
    /// </summary>
    /// <param name="tipoCarga">Tipo de carga masiva (Usuarios, Vehiculos, Repuestos, Servicios).</param>
    /// <param name="rutaArchivo">Ruta del archivo a cargar.</param>
    /// <returns>True si la carga fue exitosa, false en caso contrario.</returns>
    public bool GestionarCargaMasiva(string tipoCarga, string rutaArchivo)
    {
        switch (tipoCarga.ToUpper())
        {
            case "USUARIOS":
                // Cargar usuarios desde un archivo JSON
                return CargarUsuariosDesdeArchivo(rutaArchivo);
            case "VEHICULOS":
                // Cargar vehiculos desde un archivo JSON
                return CargaVehiculosDesdeArchivo(rutaArchivo);
            case "REPUESTOS":
                // Cargar repuestos desde un archivo JSON
                return CargaRepuestosDesdeArchivo(rutaArchivo);
            case "SERVICIOS":
                // Cargar servicios desde un archivo JSON
                return CargaServiciosDesdeArchivo(rutaArchivo);
                break;
            default:
                Console.WriteLine("Tipo de carga no soportado.");
                return false;
        }
        return false;
    }
    
    // 📌 Cargar clientes desde un archivo JSON
    public bool CargarUsuariosDesdeArchivo(string rutaArchivo)
    {
        try
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(rutaArchivo);
            // Deserializar el contenido a una lista de clientes
            var usuarios = JsonConvert.DeserializeObject<Usuario[]>(json);
            if (usuarios != null)
            {
                // Guardar los clientes en la base de datos
                var datos = new List<string[]>();
                foreach (var usuario  in usuarios)
                {
                    // Registrar usuario en el Blockchain
                    var us = _servicio.RegistrarUsuario(usuario);

                    if (us != null)
                    {
                        datos.Add(new string[]
                        {
                            usuario.Id.ToString(),
                            usuario.Nombres,
                            usuario.Apellidos,
                            usuario.Correo,
                            usuario.Edad.ToString(),
                            usuario.ContraseniaHash
                        });
                    }
                    else
                    {
                        // Error al registrar el usuario
                        Console.WriteLine($"Error al registrar el usuario: {usuario.Correo}");
                    }
                }
                // Actualizar la propiedad DatosCargados
                DatosCargados = datos.ToArray();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar usuarios: {ex.Message}");
        }
        return false;
    }
    
    /// <summary>
    /// Validar si el vehículo se puede agregar a la lista de vehículos.
    /// </summary>
    /// <param name="vehiculo"></param>
    /// <returns></returns>
    private bool vehiculoTrue(Vehiculo vehiculo)
    {
        // Si el vehículo es diferente de null, significa que ya existe
        // Si el id_usuario del vehículo no existe, no se puede agregar
        NodeDouble? vehiculoTrue = Estructuras.Vehiculos.SearchNode(vehiculo.Id);
        
        // Si el id_usuario del vehículo no existe, no se puede agregar 
        // Validamos desde el blockchain
        var usuarioTrue = _servicio.BuscarUsuarioPorId(vehiculo.IdUsuario);
        
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
    
    /// <summary>
    /// 📌 Cargar vehiculos desde un archivo JSON
    /// </summary>
    /// <param name="rutaArchivo">Ruta del archivo JSON.</param>
    /// <returns>True si la carga fue exitosa, false en caso contrario.</returns>
    public bool CargaVehiculosDesdeArchivo(string rutaArchivo)
    {
        try
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(rutaArchivo);
            // Deserializar el contenido a una lista de vehiculos
            var vehiculos = JsonConvert.DeserializeObject<Vehiculo[]>(json);
            if (vehiculos != null)
            {
                // Guardar los vehiculos en la base de datos
                var datos = new List<string[]>();
                foreach (var vehiculo in vehiculos)
                {
                    if (vehiculoTrue(vehiculo))
                    {
                        Estructuras.Vehiculos.Append(vehiculo); 
                        
                        datos.Add(new string[]
                        {
                            vehiculo.Id.ToString(),
                            vehiculo.IdUsuario.ToString(),
                            vehiculo.Marca,
                            vehiculo.Modelo.ToString(),
                            vehiculo.Placa
                        });
                    }
                    else
                    {
                        // Error al registrar el vehiculo
                        Console.WriteLine($"Error al registrar el vehiculo: {vehiculo.Id}");
                    }
                }
                // Actualizar la propiedad DatosCargados
                DatosCargados = datos.ToArray();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar vehiculos: {ex.Message}");
        }
        return false;
    }
    
    /// <summary>
    /// 📌 Cargar repuestos desde un archivo JSON
    /// </summary>
    /// <param name="rutaArchivo">Ruta del archivo JSON.</param>
    /// <returns>True si la carga fue exitosa, false en caso contrario.</returns>
    public bool CargaRepuestosDesdeArchivo(string rutaArchivo)
    {
        try
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(rutaArchivo);
            // Deserializar el contenido a una lista de repuestos
            var repuestos = JsonConvert.DeserializeObject<Repuesto[]>(json);
            if (repuestos != null)
            {
                // Guardar los repuestos en la base de datos
                var datos = new List<string[]>();
                foreach (var repuesto in repuestos)
                {
                    Estructuras.Repuestos.Insert(repuesto.Id, repuesto);
                    if (!datos.Any(d => d[0] == repuesto.Id.ToString()))
                    {
                        datos.Add(new string[]
                        {
                            repuesto.Id.ToString(),
                            repuesto.Repuesto1,
                            repuesto.Detalles,
                            repuesto.Costo.ToString()
                        });
                    }
                }
                // Actualizar la propiedad DatosCargados
                DatosCargados = datos.ToArray();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar repuestos: {ex.Message}");
        }
        return false;
    }
    
    /// <summary>
    /// 📌 Cargar servicios desde un archivo JSON
    /// </summary>
    /// <param name="rutaArchivo">Ruta del archivo JSON.</param>
    /// <returns>True si la carga fue exitosa, false en caso contrario.</returns>
    public bool CargaServiciosDesdeArchivo(string rutaArchivo)
    {
        try
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(rutaArchivo);
            // Deserializar el contenido a una lista de servicios
            var servicios = JsonConvert.DeserializeObject<Servicio[]>(json);
            if (servicios != null)
            {
                // Guardar los servicios en la base de datos
                var datos = new List<string[]>();
                foreach (var servicio in servicios)
                {
                    // Verificar si el repuesto y el vehiculo existen
                    var repuesto = Estructuras.Repuestos.Search(servicio.IdRepuesto);
                    var vehiculo = Estructuras.Vehiculos.SearchNode(servicio.IdVehiculo);
                    // Asigna el ID del usuario al servicio
                    servicio.IdUsuario = ((Vehiculo)vehiculo.Data).IdUsuario;
                    
                    if (repuesto == null || vehiculo == null)
                    {
                        Console.WriteLine($"Error al registrar el servicio: {servicio.Id}");
                    }else
                    {
                        Estructuras.Servicios.Insert(servicio.Id, servicio);
                        if (!datos.Any(d => d[0] == servicio.Id.ToString()))
                        {
                            datos.Add(new string[]
                            {
                                servicio.Id.ToString(),
                                servicio.IdUsuario.ToString(),
                                servicio.IdRepuesto.ToString(),
                                servicio.IdVehiculo.ToString(),
                                servicio.Detalles,
                                servicio.Costo.ToString()
                            });
                        }
                        
                        // Crea nodo de tipo vehículo, para el grafo no dirigido
                        Vehiculo dataVehiculo = (Vehiculo)vehiculo.Data;
                            
                        // Crea nodo de tipo repuesto, para el grafo no dirigido
                        Repuesto dataRepuesto = repuesto;
                        
                        
                        // Establecimeinto de la relación en grafo no dirigido entre id vehículo y id repuesto.
                        // Agregar relación entre vehículo y repuesto en el grafo no dirigido
                        Estructuras.Gestor.RegistrarCompatibilidad("V"+dataVehiculo.Id, "R"+dataRepuesto.Id);   
                        
                        // Genera factura
                        GenerarFactura(servicio, (Vehiculo)vehiculo.Data, repuesto);
                    }
                }
                // Actualizar la propiedad DatosCargados
                DatosCargados = datos.ToArray();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar servicios: {ex.Message}");
        }
        return false;
    }
    
    /// <summary>
    /// Genera una factura para el servicio.
    /// </summary>
    /// <param name="servicio">Servicio a facturar.</param>
    /// <param name="vehiculo">Vehículo asociado al servicio.</param>
    /// <param name="repuesto">Repuesto asociado al servicio.</param>
    private void GenerarFactura(Servicio servicio, Vehiculo vehiculo, Repuesto repuesto)
    {
        try
        {
            // Total de la factura
            var total = servicio.Costo + repuesto.Costo;
            // Crear la factura
            var factura = new Factura(servicio.Id, vehiculo.IdUsuario, servicio.Id, total);
            // Establecer el método de pago
            factura.EstablecerMetodoPago(MetodoPago.TarjetaDeCredito, true);
            // Guardar la factura en el Árbol de Merkle 
            Estructuras.Facturas.Insert(factura.Id, factura);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar factura: {ex.Message}");
        }
    }
    
}