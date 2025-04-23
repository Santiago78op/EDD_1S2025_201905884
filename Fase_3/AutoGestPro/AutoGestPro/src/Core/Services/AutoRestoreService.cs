using System.Text.Json;
using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

/// <summary>
/// Servicio para la restauración automática de entidades del sistema al iniciar la aplicación
/// </summary>
public class AutoRestoreService
{
    // Servicios que contienen las entidades
    private readonly ServicioUsuarios _servicioUsuarios;
    private readonly DoubleList _servicioVehiculos;
    private readonly TreeAvl<Repuesto> _servicioRepuestos;

    // Ruta de los backups
    private readonly string _backupPath;

    // Métricas para validación de consistencia
    private int _vehiculosCount = 0;
    private int _repuestosCount = 0;
    private bool _blockchainIntegrity = true;

    /// <summary>
    /// Constructor que inicializa el servicio de restauración automática
    /// </summary>
    public AutoRestoreService()
    {
        // Usar las estructuras globales del sistema
        _servicioUsuarios = Estructuras.Clientes;
        _servicioVehiculos = Estructuras.Vehiculos;
        _servicioRepuestos = Estructuras.Repuestos;

        // Obtener la ruta de backups
        _backupPath = BackupService.GetBackupPath();
    }

    /// <summary>
    /// Realiza la restauración automática de todas las entidades del sistema
    /// </summary>
    /// <returns>Resultado de la restauración</returns>
    public RestoreResult RestoreAutomatically()
    {
        RestoreResult result = new RestoreResult();

        try
        {
            // Verificar que la carpeta de backups existe
            if (!Directory.Exists(_backupPath))
            {
                result.Success = false;
                result.Message = "No se encontró la carpeta de backups";
                Console.WriteLine(result.Message);
                return result;
            }

            // Obtener los archivos de backup más recientes
            string latestUsuariosFile = GetLatestFile("Usuarios_*.json");
            string latestVehiculosFile = GetLatestFile("Vehiculos_*.edd");
            string latestRepuestosFile = GetLatestFile("Repuestos_*.edd");

            if (latestUsuariosFile == null && latestVehiculosFile == null && latestRepuestosFile == null)
            {
                result.Success = false;
                result.Message = "No se encontraron archivos de backup";
                Console.WriteLine(result.Message);
                return result;
            }

            // Guardar los conteos actuales para validación posterior
            SaveCurrentCounts();

            // Restaurar usuarios (Blockchain)
            if (latestUsuariosFile != null)
            {
                result.UsuariosRestored = RestoreUsuariosWithValidation(latestUsuariosFile);
                result.UsuariosFile = Path.GetFileName(latestUsuariosFile);
                result.BlockchainIntegrity = _blockchainIntegrity;
            }

            // Si la blockchain está corrupta, detenemos la restauración
            if (!_blockchainIntegrity)
            {
                result.Success = false;
                result.Message = "Error en la integridad del Blockchain de usuarios. Restauración abortada.";
                Console.WriteLine(result.Message);
                return result;
            }

            // Restaurar vehículos
            if (latestVehiculosFile != null)
            {
                result.VehiculosRestored = RestoreVehiculosWithValidation(latestVehiculosFile);
                result.VehiculosFile = Path.GetFileName(latestVehiculosFile);
            }

            // Restaurar repuestos
            if (latestRepuestosFile != null)
            {
                result.RepuestosRestored = RestoreRepuestosWithValidation(latestRepuestosFile);
                result.RepuestosFile = Path.GetFileName(latestRepuestosFile);
            }

            // Verificar consistencia de datos
            bool consistencyCheck = ValidateConsistency();
            result.ConsistencyValid = consistencyCheck;

            if (!consistencyCheck)
            {
                result.Success = false;
                result.Message = "Error en la consistencia de los datos. Los conteos no coinciden con los esperados.";
                Console.WriteLine(result.Message);
                return result;
            }

            // Si todo está bien, marcamos como éxito
            result.Success = true;
            result.Message = "Restauración automática completada con éxito";
            Console.WriteLine(result.Message);

            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"Error en la restauración automática: {ex.Message}";
            Console.WriteLine($"{result.Message}\n{ex.StackTrace}");
            return result;
        }
    }

    /// <summary>
    /// Obtiene el archivo más reciente que coincide con el patrón
    /// </summary>
    /// <param name="pattern">Patrón de búsqueda para los archivos</param>
    /// <returns>Ruta del archivo más reciente o null si no hay archivos</returns>
    private string GetLatestFile(string pattern)
    {
        try
        {
            var files = Directory.GetFiles(_backupPath, pattern)
                .OrderByDescending(f => new FileInfo(f).LastWriteTime);

            return files.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al buscar archivos de backup: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Guarda los conteos actuales de las entidades para validación posterior
    /// </summary>
    private void SaveCurrentCounts()
    {
        try
        {
            // Contar vehículos actuales
            _vehiculosCount = 0;
            NodeDouble current = _servicioVehiculos.Head;
            while (current != null)
            {
                _vehiculosCount++;
                current = current.Next;
            }

            // Contar repuestos actuales
            _repuestosCount = _servicioRepuestos.Count;

            Console.WriteLine($"Conteos actuales: Vehículos={_vehiculosCount}, Repuestos={_repuestosCount}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar conteos actuales: {ex.Message}");
        }
    }

    /// <summary>
    /// Restaura los usuarios desde un archivo JSON validando la integridad del Blockchain
    /// </summary>
    /// <param name="filePath">Ruta del archivo JSON</param>
    /// <returns>Número de usuarios restaurados</returns>
    private int RestoreUsuariosWithValidation(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                _blockchainIntegrity = false;
                return 0;
            }

            // Leer el contenido del archivo
            string jsonContent = File.ReadAllText(filePath);

            // Deserializar el JSON
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var usuariosData = JsonSerializer.Deserialize<List<JsonElement>>(jsonContent, options);
            int count = 0;

            // Recrear los usuarios
            foreach (var userData in usuariosData)
            {
                // Extraer propiedades del elemento JSON
                int id = userData.GetProperty("Id").GetInt32();
                string nombres = userData.GetProperty("Nombres").GetString();
                string apellidos = userData.GetProperty("Apellidos").GetString();
                string correo = userData.GetProperty("Correo").GetString();
                int edad = userData.GetProperty("Edad").GetInt32();

                // Contraseña de restauración
                string contraseniaTemporal = "Restore" + id;

                // Registrar usuario en la blockchain
                var usuarioExistente = _servicioUsuarios.BuscarUsuarioPorId(id);
                if (usuarioExistente == null)
                {
                    _servicioUsuarios.RegistrarUsuario(id, nombres, apellidos, correo, edad, contraseniaTemporal);
                    count++;
                }
            }

            // Verificar integridad de la blockchain
            _blockchainIntegrity = _servicioUsuarios.VerificarIntegridad();

            if (!_blockchainIntegrity)
            {
                Console.WriteLine("Error en la integridad del Blockchain de usuarios");
            }

            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al restaurar usuarios: {ex.Message}");
            _blockchainIntegrity = false;
            return 0;
        }
    }

    /// <summary>
    /// Restaura los vehículos desde un archivo comprimido con Huffman
    /// </summary>
    /// <param name="filePath">Ruta del archivo .edd</param>
    /// <returns>Número de vehículos restaurados</returns>
    private int RestoreVehiculosWithValidation(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return 0;

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
            int initialCount = _vehiculosCount;

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
                    }
                }
            }

            // Contar vehículos después de la restauración
            int newCount = 0;
            NodeDouble current = _servicioVehiculos.Head;
            while (current != null)
            {
                newCount++;
                current = current.Next;
            }

            return newCount - initialCount;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al restaurar vehículos: {ex.Message}");
            return 0;
        }
    }

    /// <summary>
    /// Restaura los repuestos desde un archivo comprimido con Huffman
    /// </summary>
    /// <param name="filePath">Ruta del archivo .edd</param>
    /// <returns>Número de repuestos restaurados</returns>
    private int RestoreRepuestosWithValidation(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return 0;

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
            int initialCount = _repuestosCount;

            // Restaurar los repuestos
            foreach (var repuesto in repuestos)
            {
                // Verificar si el repuesto ya existe
                if (!_servicioRepuestos.ContainsKey(repuesto.Id))
                {
                    _servicioRepuestos.Insert(repuesto.Id, repuesto);
                }
            }

            // Calcular repuestos añadidos
            return _servicioRepuestos.Count - initialCount;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al restaurar repuestos: {ex.Message}");
            return 0;
        }
    }

    /// <summary>
    /// Valida la consistencia de los datos restaurados
    /// </summary>
    /// <returns>True si la consistencia es válida, False en caso contrario</returns>
    private bool ValidateConsistency()
    {
        try
        {
            // Contar vehículos después de la restauración
            int newVehiculosCount = 0;
            NodeDouble current = _servicioVehiculos.Head;
            while (current != null)
            {
                newVehiculosCount++;
                current = current.Next;
            }

            // Contar repuestos después de la restauración
            int newRepuestosCount = _servicioRepuestos.Count;

            Console.WriteLine($"Nuevos conteos: Vehículos={newVehiculosCount}, Repuestos={newRepuestosCount}");

            // La consistencia es válida si los nuevos conteos son iguales o mayores a los anteriores
            return newVehiculosCount >= _vehiculosCount && newRepuestosCount >= _repuestosCount;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al validar consistencia: {ex.Message}");
            return false;
        }
    }
}