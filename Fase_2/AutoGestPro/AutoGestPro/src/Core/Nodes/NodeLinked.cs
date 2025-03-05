namespace AutoGestPro.Core.Nodes;

/**
 * Definicion del nodo de una lista enlazada simple
 */
public class NodeLinked
{
    // Dato almacenado en el nodo
    public object Data { get; set; }
    // Puntero al siguiente nodo
    public NodeLinked? Next { get; set; }
    
    // Constructor del nodo
    public NodeLinked(object data)
    {
        Data = data;
        Next = null;
    }
}