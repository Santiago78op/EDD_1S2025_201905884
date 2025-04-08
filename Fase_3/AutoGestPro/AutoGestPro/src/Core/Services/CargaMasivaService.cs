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
                break;
            case "REPUESTOS":
                // Implementar carga de repuestos
                break;
            case "SERVICIOS":
                // Implementar carga de servicios
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
                    var us = _servicio.RegistrarUsuario(usuario.Id, usuario.Nombres, usuario.Apellidos, usuario.Correo, usuario.Edad, usuario.ContraseniaHash);

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
                    // Registrar vehiculo en el Blockchain
                    var vehi = _servicio.RegistrarVehiculo(vehiculo.Id, vehiculo.Marca, vehiculo.Modelo, vehiculo.Anio, vehiculo.Patente);

                    if (vehi != null)
                    {
                        if(vehiculoTrue(vehiculo))
                        {
                            Estructuras.Vehiculos.Append(vehiculo);
                        }
                        
                        datos.Add(new string[]
                        {
                            vehiculo.Id.ToString(),
                            vehiculo.Marca,
                            vehiculo.Modelo,
                            vehiculo.Anio.ToString(),
                            vehiculo.Patente
                        });
                    }
                    else
                    {
                        // Error al registrar el vehiculo
                        Console.WriteLine($"Error al registrar el vehiculo: {vehiculo.Patente}");
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
}