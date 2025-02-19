namespace AutoGestPro.Core.Nodes;

public unsafe class NodeStack<T> where T : class
{
    // Dato almacenado en el nodo
    public T _data;
    // Puntero al siguiente nodo
    public NodeStack<T>* _next;
    
    /**
     * Constructor del nodo
     * @param data Dato a almacenar en el nodo
     */
    public NodeStack(T data)
    {
        _data = data;
        _next = null;
    }
}