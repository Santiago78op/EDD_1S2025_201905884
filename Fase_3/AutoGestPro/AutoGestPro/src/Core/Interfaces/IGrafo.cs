namespace AutoGestPro.Core.Interfaces;

/// <summary>
/// Define las operaciones básicas de un grafo
/// </summary>
public interface IGrafo
{
    /// <summary>
    /// Obtiene todos los nodos del grafo
    /// </summary>
    IReadOnlyCollection<string> Nodos { get; }

    /// <summary>
    /// Inserta una conexión entre dos nodos
    /// </summary>
    /// <param name="nodo1">Primer nodo</param>
    /// <param name="nodo2">Segundo nodo</param>
    void Insertar(string nodo1, string nodo2);

    /// <summary>
    /// Verifica si existe una conexión entre dos nodos
    /// </summary>
    /// <param name="nodo1">Primer nodo</param>
    /// <param name="nodo2">Segundo nodo</param>
    /// <returns>True si existe la conexión, false en caso contrario</returns>
    bool ExisteConexion(string nodo1, string nodo2);

    /// <summary>
    /// Obtiene los vecinos de un nodo específico
    /// </summary>
    /// <param name="nodo">Identificador del nodo</param>
    /// <returns>Conjunto con los vecinos del nodo</returns>
    IReadOnlyCollection<string> ObtenerVecinos(string nodo);

    /// <summary>
    /// Genera una representación del grafo en formato DOT para visualización con Graphviz
    /// </summary>
    /// <returns>String en formato DOT</returns>
    string GenerarDot();
}
