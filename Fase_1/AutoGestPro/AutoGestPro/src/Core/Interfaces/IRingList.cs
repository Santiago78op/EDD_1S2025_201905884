using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Interfaces;

/**
 * Interfaz que define los métodos que debe tener una lista circular
 */
public unsafe interface IRingList<T> where T : class
{
    void append(T data); // Agrega un nodo al final de la lista
    void remove(T data); // Elimina un nodo de la lista
    void printList(); // Imprime la lista
    NodeRing<T>* searchNode(int id); // Busca un nodo en la lista
}