using System.Text.Json;
using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

/// <summary>
/// Servicio para la restauración de backups de las entidades del sistema
/// </summary>
public class RestoreService
{
    // Servicios que contienen las entidades
    private readonly ServicioUsuarios _servicioUsuarios;
    private readonly DoubleList _servicioVehiculos;
    private readonly TreeAvl<Repuesto> _servicioRepuestos;

    // Ruta de destino para los backups
    private readonly string _backupPath;

    /// <summary>
    /// Constructor que inicializa el servicio de restauración
    /// </summary>
    public RestoreService()
    {
        // Usar las estructuras globales del sistema
        _servicioUsuarios = Estructuras.Clientes;
        _servicioVehiculos = Estructuras.Vehiculos;
        _servicioRepuestos = Estructuras.Repuestos;

        // Usar la misma ruta de backup que el servicio de backup
        _backupPath = BackupService.GetBackupPath();
    }

    /// <summary>
    /// Restaura los usuarios desde un archivo JSON
    /// </summary>
    /// <param name="filePath">Ruta del archivo JSON</param>
    /// <returns>Número de usuarios restaurados</returns>
    public int RestoreUsuarios(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("El archivo de backup no existe", filePath);

            // Leer el contenido del archivo
            string jsonContent = File.ReadAllText(filePath);

            // Deserializar el JSON
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var usuariosData = JsonSerializer.Deserialize<List<dynamic>>(jsonContent, options);
            int count = 0;

            // Recrear los usuarios
            foreach (var userData in usuariosData)
            {
                // Extraer propiedades del objeto dinámico
                int id = Convert.ToInt32(userData.GetProperty("Id"));
                string nombres = userData.GetProperty("Nombres").ToString();
                string apellidos = userData.GetProperty("Apellidos").ToString();
                string correo = userData.GetProperty("Correo").ToString();
                int edad = Convert.ToInt32(userData.GetProperty("Edad"));

                // Crear usuario con contraseña genérica que tendrá que cambiar después
                string contraseniaTemporal = "Restore" + id;

                // Registrar el usuario
                var usuarioExistente = _servicioUsuarios.BuscarUsuarioPorId(id);
                if (usuarioExistente == null)
                {
                    _servicioUsuarios.RegistrarUsuario(usuarioExistente);
                    count++;
                }
            }

            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al restaurar usuarios: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Restaura los vehículos desde un archivo comprimido con Huffman
    /// </summary>
    /// <param name="filePath">Ruta del archivo .edd</param>
    /// <returns>Número de vehículos restaurados</returns>
    public int RestoreVehiculos(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("El archivo de backup no existe", filePath);

            // Leer el archivo comprimido
            byte[] compressedData;
            string huffmanTree;
            int originalLength;

            using (var fs = new FileStream(filePath, FileMode.Open))
            using (var reader = new BinaryReader(fs))
            {
                // Leer el tamaño del árbol Huffman
                int treeLength = reader.ReadInt32();

                // Leer el árbol Huffman
                huffmanTree = reader.ReadString();

                // Leer el tamaño de los datos originales
                originalLength = reader.ReadInt32();

                // Leer el tamaño de los datos comprimidos
                int compressedLength = reader.ReadInt32();

                // Leer los datos comprimidos
                compressedData = reader.ReadBytes(compressedLength);
            }

            // Descomprimir los datos
            HuffmanCompression huffman = new HuffmanCompression();
            string jsonData = huffman.Decompress(compressedData, huffmanTree, originalLength);

            // Deserializar los vehículos
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var vehiculos = JsonSerializer.Deserialize<List<Vehiculo>>(jsonData, options);
            int count = 0;

            // Restaurar los vehículos
            foreach (var vehiculo in vehiculos)
            {
                // Verificar si el usuario existe
                var usuario = _servicioUsuarios.BuscarUsuarioPorId(vehiculo.IdUsuario);
                if (usuario != null)
                {
                    // Verificar si el vehículo ya existe
                    var nodoExistente = _servicioVehiculos.SearchNode(vehiculo.Id);
                    if (nodoExistente == null)
                    {
                        _servicioVehiculos.Append(vehiculo);
                        count++;
                    }
                }
            }

            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al restaurar vehículos: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Restaura los repuestos desde un archivo comprimido con Huffman
    /// </summary>
    /// <param name="filePath">Ruta del archivo .edd</param>
    /// <returns>Número de repuestos restaurados</returns>
    public int RestoreRepuestos(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("El archivo de backup no existe", filePath);

            // Leer el archivo comprimido
            byte[] compressedData;
            string huffmanTree;
            int originalLength;

            using (var fs = new FileStream(filePath, FileMode.Open))
            using (var reader = new BinaryReader(fs))
            {
                // Leer el tamaño del árbol Huffman
                int treeLength = reader.ReadInt32();

                // Leer el árbol Huffman
                huffmanTree = reader.ReadString();

                // Leer el tamaño de los datos originales
                originalLength = reader.ReadInt32();

                // Leer el tamaño de los datos comprimidos
                int compressedLength = reader.ReadInt32();

                // Leer los datos comprimidos
                compressedData = reader.ReadBytes(compressedLength);
            }

            // Descomprimir los datos
            HuffmanCompression huffman = new HuffmanCompression();
            string jsonData = huffman.Decompress(compressedData, huffmanTree, originalLength);

            // Deserializar los repuestos
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var repuestos = JsonSerializer.Deserialize<List<Repuesto>>(jsonData, options);
            int count = 0;

            // Restaurar los repuestos
            foreach (var repuesto in repuestos)
            {
                int id = Convert.ToInt32(repuesto.Id);
                string nombre = repuesto.Repuesto1;
                string detalles = repuesto.Detalles;
                decimal costo = Convert.ToDecimal(repuesto.Costo);
                
                // Verificar si el repuesto ya existe
                if (!_servicioRepuestos.ContainsKey(repuesto.Id))
                {
                    // Crear el objeto Repuesto manualmente
                    Repuesto r = new Repuesto(id, nombre, detalles, costo);
                    _servicioRepuestos.Insert(r.Id, r);
                    count++;
                }
            }

            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al restaurar repuestos: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Restaura todos los backups disponibles en la carpeta de backup
    /// </summary>
    /// <returns>Diccionario con el resultado de cada restauración</returns>
    public Dictionary<string, int> RestoreAll()
    {
        return RestoreAll(_backupPath);
    }

    /// <summary>
    /// Restaura todos los backups disponibles en la carpeta especificada
    /// </summary>
    /// <param name="directoryPath">Ruta de la carpeta de backups</param>
    /// <returns>Diccionario con el resultado de cada restauración</returns>
    public Dictionary<string, int> RestoreAll(string directoryPath)
    {
        Dictionary<string, int> results = new Dictionary<string, int>();

        try
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException("El directorio de backup no existe");

            // Buscar archivos de backup
            var usuariosFiles = Directory.GetFiles(directoryPath, "Usuarios_*.json").OrderByDescending(f => f);
            var vehiculosFiles = Directory.GetFiles(directoryPath, "Vehiculos_*.edd").OrderByDescending(f => f);
            var repuestosFiles = Directory.GetFiles(directoryPath, "Repuestos_*.edd").OrderByDescending(f => f);

            // Restaurar los usuarios (el archivo más reciente)
            if (usuariosFiles.Any())
            {
                string latestUsuariosFile = usuariosFiles.First();
                int usuariosCount = RestoreUsuarios(latestUsuariosFile);
                results.Add("Usuarios", usuariosCount);
            }

            // Restaurar los vehículos (el archivo más reciente)
            if (vehiculosFiles.Any())
            {
                string latestVehiculosFile = vehiculosFiles.First();
                int vehiculosCount = RestoreVehiculos(latestVehiculosFile);
                results.Add("Vehiculos", vehiculosCount);
            }

            // Restaurar los repuestos (el archivo más reciente)
            if (repuestosFiles.Any())
            {
                string latestRepuestosFile = repuestosFiles.First();
                int repuestosCount = RestoreRepuestos(latestRepuestosFile);
                results.Add("Repuestos", repuestosCount);
            }

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al restaurar todos los backups: {ex.Message}");
            throw;
        }
    }
}