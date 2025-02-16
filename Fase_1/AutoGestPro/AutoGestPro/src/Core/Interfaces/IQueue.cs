using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Interfaces;

/**
 * Interfaz que define los métodos que debe tener una cola
 */
public unsafe interface IQueue<T> where T : class
{
    void enqueue(T data); // Agrega un nodo al final de la cola
    void dequeue(); // Elimina un nodo de la cola
    void printQueue(); // Imprime la cola
    NodeQueue<T>* GetNode(int index); // Obtiene un nodo de la cola
}