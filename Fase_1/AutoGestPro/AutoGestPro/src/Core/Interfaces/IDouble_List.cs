using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Interfaces;

/**
 * Interfaz que define los métodos que debe tener una lista doblemente enlazada
 */
public unsafe interface IDouble_List<T> where T : class
{
    void append(T data); // Agrega un nodo al final de la lista
    void remove(Vehiculo data); // Elimina un nodo de la lista
    void printList(); // Imprime la lista
    NodeDouble<T>* searchNode(int id); // Busca un nodo en la lista
}