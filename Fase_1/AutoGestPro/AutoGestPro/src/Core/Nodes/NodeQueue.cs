namespace AutoGestPro.Core.Nodes;

public unsafe class NodeQueue<T> where T : class    
{   
     // Dato almacenado en el nodo
     public T _data;
     // Puntero al siguiente nodo
     public NodeQueue<T>* _next;
     
     /**
      * Constructor del nodo
      * @param data Dato a almacenar en el nodo
      */
     public NodeQueue(T data)
     {
         _data = data;
         _next = null;
     }
}