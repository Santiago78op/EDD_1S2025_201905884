using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Interfaces;

public interface ILinkedList
{
    void Append(object data); // Agrega un nodo al final de la lista
    bool Remove(object data); // Elimina un nodo de la lista
    void PrintList(); // Imprime la lista
    NodeLinked GetNode(int index); // Obtiene un nodo de la lista
    bool ModifyNode(int id, object data); // Modifica un nodo de la lista
    bool DeleteNode(int id); // Elimina un nodo de la lista
    NodeLinked SearchNode(int id); // Busca un nodo en la lista
}