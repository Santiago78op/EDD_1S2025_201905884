namespace AutoGestPro.Core.Nodes;

public unsafe class NodeDouble<T> where T : class
{
       // Dato almacenado en el nodo
       public T _data;
       // Puntero al siguiente nodo
       public NodeDouble<T>* _next;
       // Puntero al nodo anterior
       public NodeDouble<T>* _prev;
       
       /**
        * Constructor del nodo
        * @param data Dato a almacenar en el nodo
        */
       public NodeDouble(T data)
       {
              _data = data;
              _next = null;
              _prev = null;
       }
}