namespace AutoGestPro.Core.Nodes;

/**
 * Definicion del nodo de una lista enlazada simple
 */
public unsafe class NodeLinked<T> where T : class
{
    // Dato almacenado en el nodo
    public T _data;
    // Puntero al siguiente nodo
    public NodeLinked<T>* _next;

    /**
     * Constructor del nodo
     * @param data Dato a almacenar en el nodo
     */
    public NodeLinked(T data)
    {
        _data = data;
        _next = null;
    }
}