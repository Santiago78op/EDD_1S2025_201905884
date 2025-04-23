namespace AutoGestPro.Core.Services;

/// <summary>
/// Clase que representa el resultado de la restauración automática
/// </summary>
public class RestoreResult
{
    /// <summary>
    /// Indica si la restauración tuvo éxito
    /// </summary>
    public bool Success { get; set; }
            
    /// <summary>
    /// Mensaje descriptivo del resultado
    /// </summary>
    public string Message { get; set; }
            
    /// <summary>
    /// Número de usuarios restaurados
    /// </summary>
    public int UsuariosRestored { get; set; }
            
    /// <summary>
    /// Número de vehículos restaurados
    /// </summary>
    public int VehiculosRestored { get; set; }
            
    /// <summary>
    /// Número de repuestos restaurados
    /// </summary>
    public int RepuestosRestored { get; set; }
            
    /// <summary>
    /// Nombre del archivo de usuarios restaurado
    /// </summary>
    public string UsuariosFile { get; set; }
            
    /// <summary>
    /// Nombre del archivo de vehículos restaurado
    /// </summary>
    public string VehiculosFile { get; set; }
            
    /// <summary>
    /// Nombre del archivo de repuestos restaurado
    /// </summary>
    public string RepuestosFile { get; set; }
            
    /// <summary>
    /// Indica si la integridad del Blockchain es válida
    /// </summary>
    public bool BlockchainIntegrity { get; set; }
            
    /// <summary>
    /// Indica si la consistencia de los datos es válida
    /// </summary>
    public bool ConsistencyValid { get; set; }
}