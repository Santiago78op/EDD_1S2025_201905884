using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Interfaces;

public unsafe interface IStackList<T> where T : class
{
    void push(T data); // Agrega un nodo al inicio de la pila
    NodeStack<T>* pop(); // Elimina un nodo de la pila
    NodeStack<T>* peek(); // Obtiene el nodo del inicio de la pila
    void printStack(); // Imprime la pila
    NodeStack<T>* GetNode(int index); // Obtiene un nodo de la pila
    NodeStack<T>* searchNode(int id); // Busca un nodo en la pila
}