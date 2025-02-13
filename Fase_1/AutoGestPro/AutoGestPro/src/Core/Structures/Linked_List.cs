using System.Runtime.InteropServices;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

public unsafe class Linked_List<T> : ILinkedList<T>, IDisposable where T : class
{
    private NodeLinked<T>* _head;
    private NodeLinked<T>* _tail;
    int length;

    public NodeLinked<T>* Head
    {
        get => _head;
        set => _head = value;
    }

    public NodeLinked<T>* Tail
    {
        get => _tail;
        set => _tail = value;
    }

    public int Length
    {
        get => length;
        set => length = value;
    }

    /**
     * Constructor de la lista enlazada
     * @param persona Dato a almacenar en el nodo
     */
    public Linked_List(T estudiante)
    {
        /* Se reserva memoria para el nuevo nodo */
        NodeLinked<T>* newNodeLinked = (NodeLinked<T>*)Marshal.AllocHGlobal(sizeof(NodeLinked<T>));
        if (newNodeLinked == null)
        {
            throw new OutOfMemoryException("No se pudo reservar memoria para el nuevo nodo");
        }

        /* Se asigna el dato al nuevo nodo */
        *newNodeLinked = new NodeLinked<T>(estudiante);
        newNodeLinked->_next = null;

        _head = newNodeLinked; // El primer nodo es el nuevo nodo
        _tail = newNodeLinked; // El ultimo nodo es el nuevo nodo
        length = 1; // La lista tiene un solo nodo
    }

    /**
     * Metodo para agregar un nuevo nodo al final de la lista
     * @param persona Dato a almacenar en el nodo
     * @return void
     * @complexity O(1)
     * @precondition Ninguna
     * @postcondition Se añade un nodo al final de la lista
     * @exception Ninguna
     * @test_cases
     */
    public void append(T persona)
    {
        // Agregar un nuevo nodo al final de la lista
        NodeLinked<T>* newNodeLinked = (NodeLinked<T>*)Marshal.AllocHGlobal(sizeof(NodeLinked<T>));

        // Se asigna el dato al nuevo nodo
        *newNodeLinked = new NodeLinked<T>(persona);
        newNodeLinked->_next = null;

        // Se asigna el dato al nuevo nodo
        if (length == 0)
        {
            _head = newNodeLinked;
            _tail = newNodeLinked;
        }
        else
        {
            // Se asigna el puntero al siguiente nodo
            _tail->_next = newNodeLinked;
            _tail = newNodeLinked;
        }

        length++;
    }

    /**
     * Metodo para eliminar un nodo de la lista
     * @param id Identificador de la persona a eliminar
     * @return void
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se elimina el último nodo de la lista
     * @exception Ninguna
     * @test_cases
     */
    public void remove(T data)
    {
        NodeLinked<T>* temp = _head;
        NodeLinked<T>* prev = null;
        while (temp != null)
        {
            if (temp->_data.Equals(data))
            {
                if (prev == null)
                {
                    _head = temp->_next;
                }
                else
                {
                    prev->_next = temp->_next;
                }

                length--;
                Marshal.FreeHGlobal((IntPtr)temp);
                return;
            }

            prev = temp;
            temp = temp->_next;
        }
    }

    /**
     * Metodo para imprimir la lista
     * @return void
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Ninguna
     * @exception Ninguna
     * @test_cases
     */
    public void printList()
    {
        NodeLinked<T>* temp = _head;

        if (temp == null)
        {
            Console.WriteLine("La lista esta vacia");
        }
        else
        {
            while (temp != null)
            {
                Console.WriteLine("Persona: " + temp->_data.ToString());
                temp = temp->_next;
            }
        }
    }

    /**
     * Metodo Dispose para liberar la memoria de la lista enlazada
     * Se libera la memoria de cada nodo de la lista
     * Se asigna null a la cabeza de la lista
     * Se suprime la finalizacion del objeto
     * Se invoca al recolector de basura
     */
    public void Dispose()
    {
        NodeLinked<T>* temp = _head;
        while (temp != null)
        {
            NodeLinked<T>* next = temp->_next;
            Marshal.FreeHGlobal((IntPtr)temp);
            temp = next;
        }

        _head = null;

        GC.SuppressFinalize(this);
    }

    /**
     * Destructor de la lista enlazada
     */
    ~Linked_List()
    {
        Dispose();
    }
}