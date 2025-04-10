namespace AutoGestPro.Core.Nodes;

/// <summary>
/// Clase que representa un nodo en el árbol AVL.
/// </summary>
/// <typeparam name="TValue">Tipo de valor almacenado en el nodo</typeparam>
public class NodeTreeAvl<TValue>
{
    /// <summary>
    /// Referencia al hijo izquierdo.
    /// </summary>
    public NodeTreeAvl<TValue> Left { get; set; }

    /// <summary>
    /// Referencia al hijo derecho.
    /// </summary>
    public NodeTreeAvl<TValue> Right { get; set; }

    /// <summary>
    /// Altura del nodo en el árbol.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Clave única del nodo.
    /// </summary>
    public int Key { get; set; }

    /// <summary>
    /// Valor almacenado en el nodo.
    /// </summary>
    public TValue Value { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase NodeTreeAvl.
    /// </summary>
    /// <param name="key">Clave única del nodo</param>
    /// <param name="value">Valor a almacenar</param>
    public NodeTreeAvl(int key, TValue value)
    {
        Key = key;
        Value = value;
        Left = null;
        Right = null;
        Height = 1;
    }
}