namespace AutoGestPro.Core.Nodes;

/**
 * Definicion del nodo de una lista enlazada doble
 */
public class NodeDouble
{
    // Dato almacenado en el nodo
    public object Data { get; set; }
    // Puntero al siguiente nodo
    public NodeDouble? Next { get; set; }
    // Puntero al nodo anterior
    public NodeDouble? Previous { get; set; }
    
    /**
     * Constructor de la clase
     * @param data Dato a almacenar en el nodo
     */
    public NodeDouble(object data)
    {
        Data = data;
        Next = null;
        Previous = null;
    }
}