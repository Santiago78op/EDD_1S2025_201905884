using System.Text.Json;
using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;
using System.Reflection;

namespace AutoGestPro.Core.Services;

/// <summary>
/// Servicio para la generación de backups de las entidades del sistema
/// </summary>
public class BackupService
{
    // Servicios que contienen las entidades
    private readonly ServicioUsuarios _servicioUsuarios;
    private readonly DoubleList _servicioVehiculos;
    private readonly TreeAvl<Repuesto> _servicioRepuestos;

    // Ruta de destino para los backups
    private readonly string _backupPath;

    /// <summary>
    /// Constructor que inicializa el servicio de backup
    /// </summary>
    /// <param name="backupPath">Ruta donde se almacenarán los archivos de backup (opcional)</param>
    public BackupService(string backupPath = null)
    {
        // Usar las estructuras globales del sistema
        _servicioUsuarios = Estructuras.Clientes;
        _servicioVehiculos = Estructuras.Vehiculos;
        _servicioRepuestos = Estructuras.Repuestos;

        // Definir ruta de backup (usar la ruta proporcionada o la predeterminada)
        _backupPath = backupPath ?? GetBackupPath();

        // Asegurar que el directorio de backup exista
        if (!Directory.Exists(_backupPath))
        {
            Directory.CreateDirectory(_backupPath);
        }
    }

    /// <summary>
    /// Obtiene la ruta absoluta a la carpeta de backups
    /// </summary>
    public static string GetBackupPath()
    {
        string projectRootPath = GetProjectRootPath();
        string backupPath = Path.Combine(projectRootPath, "Backup");

        // Crear la carpeta "Backup" si no existe
        if (!Directory.Exists(backupPath))
        {
            Directory.CreateDirectory(backupPath);
        }

        return backupPath;
    }

    /// <summary>
    /// Obtiene la ruta absoluta a la raíz del proyecto
    /// </summary>
    private static string GetProjectRootPath()
    {
        // Obtiene la ubicación del ejecutable actual
        string exePath = Assembly.GetExecutingAssembly().Location;

        // Sube cuatro niveles para llegar a la raíz del proyecto
        string projectRoot = new DirectoryInfo(Path.GetDirectoryName(exePath))
            .Parent?.Parent?.Parent?.FullName;

        if (projectRoot == null)
            throw new DirectoryNotFoundException("No se pudo encontrar la raíz del proyecto");

        return projectRoot;
    }

    /// <summary>
    /// Genera un backup de los usuarios en formato JSON sin comprimir
    /// </summary>
    /// <returns>Ruta del archivo generado</returns>
    public string BackupUsuarios()
    {
        try
        {
            // Obtener la lista de usuarios
            var usuarios = _servicioUsuarios.ObtenerTodos();

            // Crear una lista de objetos anónimos para evitar serializar propiedades sensibles
            var usuariosData = usuarios.Select(u => new
            {
                Id = u.Id,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Correo = u.Correo,
                Edad = u.Edad,
                Contrasenia = u.ContraseniaHash
                // No incluimos la contraseña por seguridad
            }).ToList();

            // Serializar a JSON con formato legible
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonData = JsonSerializer.Serialize(usuariosData, options);

            // Generar nombre de archivo con timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filePath = Path.Combine(_backupPath, $"Usuarios_{timestamp}.json");

            // Guardar el archivo
            File.WriteAllText(filePath, jsonData);

            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar backup de usuarios: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Genera un backup comprimido con algoritmo Huffman para vehículos
    /// </summary>
    /// <returns>Ruta del archivo generado</returns>
    public string BackupVehiculos()
    {
        try
        {
            // Lista para almacenar los vehículos
            List<Vehiculo> vehiculos = new List<Vehiculo>();

            // Recorrer la lista doblemente enlazada de vehículos
            NodeDouble current = _servicioVehiculos.Head;
            while (current != null)
            {
                if (current.Data is Vehiculo vehiculo)
                {
                    // Verificar que los datos del vehículo son válidos
                    if (vehiculo.Id > 0 &&
                        vehiculo.IdUsuario > 0 &&
                        !string.IsNullOrEmpty(vehiculo.Marca) &&
                        !string.IsNullOrEmpty(vehiculo.Placa))
                    {
                        vehiculos.Add(vehiculo);
                    }
                }

                current = current.Next;
            }

            // Verificar que tenemos vehículos para guardar
            if (vehiculos.Count == 0)
            {
                Console.WriteLine("No hay vehículos para guardar en el backup");
                throw new InvalidOperationException("No hay vehículos válidos para backup");
            }

            // Crear un objeto anónimo para serializar solo las propiedades necesarias
            var vehiculosData = vehiculos.Select(v => new
            {
                Id = v.Id,
                IdUsuario = v.IdUsuario,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Placa = v.Placa
            }).ToList();

            // Serializar la lista a JSON
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonData = JsonSerializer.Serialize(vehiculosData, options);

            // Guardar una copia del JSON original para debug si es necesario
            string debugPath = Path.Combine(_backupPath,
                $"Vehiculos_debug_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.json");
            File.WriteAllText(debugPath, jsonData);

            // Comprimir con Huffman
            HuffmanCompression huffman = new HuffmanCompression();
            (byte[] compressedData, string huffmanTree) = huffman.Compress(jsonData);

            // Crear el archivo .edd
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filePath = Path.Combine(_backupPath, $"Vehiculos_{timestamp}.edd");

            // Guardar datos comprimidos y el árbol Huffman
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var writer = new BinaryWriter(fs))
            {
                // Guardar el tamaño del árbol Huffman
                writer.Write(huffmanTree.Length);

                // Guardar el árbol Huffman
                writer.Write(huffmanTree);

                // Guardar el tamaño de los datos originales
                writer.Write(jsonData.Length);

                // Guardar el tamaño de los datos comprimidos
                writer.Write(compressedData.Length);

                // Guardar los datos comprimidos
                writer.Write(compressedData);
            }

            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar backup de vehículos: {ex.Message}\n{ex.StackTrace}");
            throw;
        }
    }

    /// <summary>
    /// Genera un backup comprimido con algoritmo Huffman para repuestos
    /// </summary>
    /// <returns>Ruta del archivo generado</returns>
    public string BackupRepuestos()
    {
        try
        {
            // Lista para almacenar los repuestos del árbol AVL
            List<Repuesto> repuestos = _servicioRepuestos.ToList();
            
            // Verificar que tenemos repuestos para guardar
            if(repuestos.Count == 0)
            {
                Console.WriteLine("No hay repuestos para guardar en el backup");
                throw new InvalidOperationException("No hay repuestos válidos para backup");
            }
            
            // Crear un objeto anónimo para serializar solo las propiedades necesarias
            var repuestosData = repuestos.Select(r => new
            {
                Id = r.Id,             
                Repuesto1 = r.Repuesto1, 
                Detalles = r.Detalles,
                Costo = r.Costo
            }).ToList();
            
            // Serializar la lista a JSON
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonData = JsonSerializer.Serialize(repuestosData, options);
            
            // Guardar una copia del JSON original para debug si es necesario
            string debugPath = Path.Combine(_backupPath,
                $"Repuestos_debug_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.json");
            File.WriteAllText(debugPath, jsonData);

            // Comprimir con Huffman
            HuffmanCompression huffman = new HuffmanCompression();
            (byte[] compressedData, string huffmanTree) = huffman.Compress(jsonData);

            // Crear el archivo .edd
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filePath = Path.Combine(_backupPath, $"Repuestos_{timestamp}.edd");

            // Guardar datos comprimidos y el árbol Huffman
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var writer = new BinaryWriter(fs))
            {
                // Guardar el tamaño del árbol Huffman
                writer.Write(huffmanTree.Length);

                // Guardar el árbol Huffman
                writer.Write(huffmanTree);

                // Guardar el tamaño de los datos originales
                writer.Write(jsonData.Length);

                // Guardar el tamaño de los datos comprimidos
                writer.Write(compressedData.Length);

                // Guardar los datos comprimidos
                writer.Write(compressedData);
            }

            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar backup de repuestos: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Genera todos los backups (usuarios, vehículos y repuestos)
    /// </summary>
    /// <returns>Diccionario con los paths de los archivos generados</returns>
    public Dictionary<string, string> GenerarTodosLosBackups()
    {
        Dictionary<string, string> resultados = new Dictionary<string, string>();

        try
        {
            // Generar backup de usuarios (JSON sin comprimir)
            string pathUsuarios = BackupUsuarios();
            resultados.Add("Usuarios", pathUsuarios);

            // Generar backup de vehículos (Huffman comprimido)
            string pathVehiculos = BackupVehiculos();
            resultados.Add("Vehiculos", pathVehiculos);

            // Generar backup de repuestos (Huffman comprimido)
            string pathRepuestos = BackupRepuestos();
            resultados.Add("Repuestos", pathRepuestos);

            return resultados;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar todos los backups: {ex.Message}");
            throw;
        }
    }
}