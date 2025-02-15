namespace AutoGestPro.Core.Nodes;

/**
 * Definicion del nodo de una lista circular
 */
public unsafe class NodeRing<T> where T : class
{
    public T _data;
    public NodeRing<T>* _next;
    
    /**
     * Constructor del nodo
     * @param data Dato a almacenar en el nodo
     */
    public NodeRing(T data)
    {
        _data = data;
        _next = null;
    }
}