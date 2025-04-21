using System.Text;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/// <summary>
/// Implementación de un grafo no dirigido que conecta vehículos con sus repuestos compatibles
/// </summary>
public class GrafoNoDirigido : IGrafo
{
    /// <summary>
    /// Lista de adyacencia que almacena las conexiones entre nodos
    /// </summary>
    private readonly Dictionary<string, HashSet<string>> _listaAdyacencia;

    /// <summary>
    /// Inicializa un nuevo grafo no dirigido vacío
    /// </summary>
    public GrafoNoDirigido()
    {
        _listaAdyacencia = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
    }

    /// <summary>
    /// Obtiene todos los nodos del grafo
    /// </summary>
    public IReadOnlyCollection<string> Nodos => _listaAdyacencia.Keys;

    /// <summary>
    /// Obtiene los vecinos de un nodo específico
    /// </summary>
    /// <param name="nodo">Identificador del nodo</param>
    /// <returns>Conjunto con los vecinos del nodo</returns>
    /// <exception cref="ArgumentException">Si el identificador del nodo está vacío</exception>
    /// <exception cref="KeyNotFoundException">Si el nodo no existe en el grafo</exception>
    public IReadOnlyCollection<string> ObtenerVecinos(string nodo)
    {
        if (string.IsNullOrEmpty(nodo))
            throw new ArgumentException("El identificador del nodo no puede estar vacío", nameof(nodo));

        if (!_listaAdyacencia.TryGetValue(nodo, out var vecinos))
            throw new KeyNotFoundException($"El nodo '{nodo}' no existe en el grafo");

        return vecinos;
    }

    /// <summary>
    /// Inserta una conexión entre un vehículo y un repuesto
    /// </summary>
    /// <param name="idVehiculo">Identificador del vehículo</param>
    /// <param name="idRepuesto">Identificador del repuesto</param>
    /// <exception cref="ArgumentException">Si alguno de los identificadores está vacío</exception>
    public void Insertar(string idVehiculo, string idRepuesto)
    {
        // Verificamos que los IDs no estén vacíos
        if (string.IsNullOrEmpty(idVehiculo))
            throw new ArgumentException("El ID de vehículo no puede estar vacío", nameof(idVehiculo));

        if (string.IsNullOrEmpty(idRepuesto))
            throw new ArgumentException("El ID de repuesto no puede estar vacío", nameof(idRepuesto));

        // Insertamos la conexión en ambas direcciones (grafo no dirigido)
        InsertarConexion(idVehiculo, idRepuesto);
        InsertarConexion(idRepuesto, idVehiculo);
    }

    /// <summary>
    /// Verifica si existe una conexión entre dos nodos
    /// </summary>
    /// <param name="nodo1">Primer nodo</param>
    /// <param name="nodo2">Segundo nodo</param>
    /// <returns>True si existe la conexión, false en caso contrario</returns>
    public bool ExisteConexion(string nodo1, string nodo2)
    {
        if (string.IsNullOrEmpty(nodo1) || string.IsNullOrEmpty(nodo2))
            return false;

        return _listaAdyacencia.TryGetValue(nodo1, out var vecinos) && vecinos.Contains(nodo2);
    }

    /// <summary>
    /// Genera una representación del grafo en formato DOT para visualización con Graphviz
    /// </summary>
    /// <returns>String en formato DOT</returns>
    public string GenerarDot()
    {
        var dot = new StringBuilder();

        dot.AppendLine("graph GrafoVehiculosRepuestos {");
        dot.AppendLine("    // Configuración del grafo");
        dot.AppendLine("    node [shape=ellipse, fontname=\"Arial\"];");
        dot.AppendLine("    graph [rankdir=LR, splines=true, fontname=\"Arial\"];");
        dot.AppendLine("    edge [fontname=\"Arial\"];");
        
        // Agregar título al grafo
        string titulo = "Grafo de Compatibilidad de Vehículos y Repuestos";
        dot.AppendLine($"    label=\"{titulo}\";");
        
        // Agregar marco al grafo
        dot.AppendLine("    graph [style=rounded, penwidth=2, color=navy];");

        // Conjunto para evitar conexiones duplicadas
        var conexionesProcesadas = new HashSet<string>();

        // Procesar todos los nodos y sus conexiones
        foreach (var par in _listaAdyacencia)
        {
            string nodoActual = par.Key;

            // Para cada vecino, agregamos una arista si no se ha procesado
            foreach (var nodoVecino in par.Value)
            {
                // Creamos una clave única para la conexión (ordenando los nodos alfabéticamente)
                string claveConexion = string.Compare(nodoActual, nodoVecino, StringComparison.Ordinal) < 0
                    ? $"{nodoActual}--{nodoVecino}"
                    : $"{nodoVecino}--{nodoActual}";

                // Solo agregamos la conexión si no ha sido procesada previamente
                if (conexionesProcesadas.Add(claveConexion))
                {
                    dot.AppendLine($"    \"{nodoActual}\" -- \"{nodoVecino}\";");
                }
            }
        }

        dot.AppendLine("}");
        return dot.ToString();
    }

    #region Métodos privados

    /// <summary>
    /// Inserta una conexión unidireccional entre dos nodos
    /// </summary>
    private void InsertarConexion(string origen, string destino)
    {
        // Si el nodo origen no existe, lo creamos con un conjunto vacío
        if (!_listaAdyacencia.TryGetValue(origen, out var vecinos))
        {
            vecinos = new HashSet<string>(StringComparer.Ordinal);
            _listaAdyacencia[origen] = vecinos;
        }

        // Agregamos el destino al conjunto de vecinos (HashSet garantiza unicidad)
        vecinos.Add(destino);
    }

    #endregion
}