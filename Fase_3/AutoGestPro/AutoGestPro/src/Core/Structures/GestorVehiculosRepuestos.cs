using AutoGestPro.Core.Interfaces;

namespace AutoGestPro.Core.Structures;

/// <summary>
/// Gestor especializado para las relaciones entre vehículos y repuestos
/// </summary>
public class GestorVehiculosRepuestos
{
    private readonly IGrafo _grafo;

    /// <summary>
    /// Inicializa un nuevo gestor de vehículos y repuestos
    /// </summary>
    /// <param name="grafo">Grafo que almacenará las relaciones</param>
    /// <exception cref="ArgumentNullException">Si el grafo es nulo</exception>
    public GestorVehiculosRepuestos(IGrafo grafo)
    {
        _grafo = grafo ?? throw new ArgumentNullException(nameof(grafo));
    }

    /// <summary>
    /// Registra una compatibilidad entre un vehículo y un repuesto
    /// </summary>
    /// <param name="idVehiculo">Identificador del vehículo</param>
    /// <param name="idRepuesto">Identificador del repuesto</param>
    public void RegistrarCompatibilidad(string idVehiculo, string idRepuesto)
    {
        _grafo.Insertar(idVehiculo, idRepuesto);
    }

    /// <summary>
    /// Registra múltiples compatibilidades para un vehículo
    /// </summary>
    /// <param name="idVehiculo">Identificador del vehículo</param>
    /// <param name="idsRepuestos">Colección de identificadores de repuestos</param>
    /// <exception cref="ArgumentNullException">Si alguno de los parámetros es nulo</exception>
    public void RegistrarCompatibilidades(string idVehiculo, IEnumerable<string> idsRepuestos)
    {
        if (string.IsNullOrEmpty(idVehiculo))
            throw new ArgumentException("El ID de vehículo no puede estar vacío", nameof(idVehiculo));

        if (idsRepuestos == null)
            throw new ArgumentNullException(nameof(idsRepuestos));

        foreach (var idRepuesto in idsRepuestos)
        {
            if (!string.IsNullOrEmpty(idRepuesto))
            {
                _grafo.Insertar(idVehiculo, idRepuesto);
            }
        }
    }

    /// <summary>
    /// Verifica si un repuesto es compatible con un vehículo
    /// </summary>
    /// <param name="idVehiculo">Identificador del vehículo</param>
    /// <param name="idRepuesto">Identificador del repuesto</param>
    /// <returns>True si son compatibles, false en caso contrario</returns>
    public bool EsCompatible(string idVehiculo, string idRepuesto)
    {
        return _grafo.ExisteConexion(idVehiculo, idRepuesto);
    }

    /// <summary>
    /// Obtiene todos los repuestos compatibles con un vehículo
    /// </summary>
    /// <param name="idVehiculo">Identificador del vehículo</param>
    /// <returns>Colección de repuestos compatibles</returns>
    /// <exception cref="ArgumentException">Si el identificador del vehículo está vacío</exception>
    public IReadOnlyCollection<string> ObtenerRepuestosCompatibles(string idVehiculo)
    {
        if (string.IsNullOrEmpty(idVehiculo))
            throw new ArgumentException("El ID de vehículo no puede estar vacío", nameof(idVehiculo));

        try
        {
            return _grafo.ObtenerVecinos(idVehiculo);
        }
        catch (KeyNotFoundException)
        {
            // Si el vehículo no existe en el grafo, devolvemos una colección vacía
            return Array.Empty<string>();
        }
    }

    /// <summary>
    /// Obtiene todos los vehículos compatibles con un repuesto
    /// </summary>
    /// <param name="idRepuesto">Identificador del repuesto</param>
    /// <returns>Colección de vehículos compatibles</returns>
    /// <exception cref="ArgumentException">Si el identificador del repuesto está vacío</exception>
    public IReadOnlyCollection<string> ObtenerVehiculosCompatibles(string idRepuesto)
    {
        if (string.IsNullOrEmpty(idRepuesto))
            throw new ArgumentException("El ID de repuesto no puede estar vacío", nameof(idRepuesto));

        try
        {
            return _grafo.ObtenerVecinos(idRepuesto);
        }
        catch (KeyNotFoundException)
        {
            // Si el repuesto no existe en el grafo, devolvemos una colección vacía
            return Array.Empty<string>();
        }
    }

    /// <summary>
    /// Genera una visualización del grafo de compatibilidades en formato DOT
    /// </summary>
    /// <returns>String en formato DOT</returns>
    public string GenerarVisualizacion()
    {
        return _grafo.GenerarDot();
    }
}