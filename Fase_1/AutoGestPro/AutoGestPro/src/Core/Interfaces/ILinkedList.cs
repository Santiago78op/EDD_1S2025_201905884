using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Interfaces;

/**
 * Interfaz que define los métodos que debe tener una lista enlazada
 */
public unsafe interface ILinkedList<T> where T : class
{
    void append(T data); // Agrega un nodo al final de la lista
    void remove(T data); // Elimina un nodo de la lista
    void printList(); // Imprime la lista
    NodeLinked<T>* GetNode(int index); // Obtiene un nodo de la lista
    bool ModifyNode(int id, T data); // Modifica un nodo de la lista
    bool DeleteNode(int id); // Elimina un nodo de la lista
    NodeLinked<T>* SearchNode(int id); // Busca un nodo en la lista
}