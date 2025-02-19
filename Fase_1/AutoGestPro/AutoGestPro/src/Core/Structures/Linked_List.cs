using System.Runtime.InteropServices;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Models;
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
    public Linked_List()
    {
        _head = null;
        _tail = null;
        length = 0;
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
    public void append(T data)
    {
        /* Se reserva memoria para el nuevo nodo */
        NodeLinked<T>* newNodeLinked = (NodeLinked<T>*)Marshal.AllocHGlobal(sizeof(NodeLinked<T>));
        if (newNodeLinked == null)
        {
            throw new OutOfMemoryException("No se pudo reservar memoria para el nuevo nodo");
        }
        
        /* Se asigna el dato al nuevo nodo */
        *newNodeLinked = new NodeLinked<T>(data);
        newNodeLinked->_next = null;
        
        // Se asigna el dato al nuevo nodo
        if (length == 0)
        {
            _head = newNodeLinked; // El primer nodo es el nuevo nodo
            _tail = newNodeLinked; // El ultimo nodo es el nuevo nodo
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

    /**
     * Metodo para obtener un nodo de la lista
     * @param index Indice del nodo a obtener
     * @return Nodo de la lista
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Ninguna
     * @exception Ninguna
     * @test_cases
     */
    public NodeLinked<T>* GetNode(int index)
    {   
        NodeLinked<T>* temp = _head;
        for (int i = 0; i < index; i++)
        {
            temp = temp->_next;
        }

        return temp;
    }
    
    /**
     * Metodo para Modificar un nodo de la lista
     * @param id Identificador del nodo a modificar
     * @param data Dato a modificar
     * @return bool
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Ninguna
     * @exception Ninguna
     * @test_cases
     */
    public bool ModifyNode(int id, T data)
    {
        NodeLinked<T>* temp = _head;
        while (temp != null)
        {
            if (data is Cliente c && temp->_data is Cliente c2 && c.Id == c2.Id)
            {
                temp->_data = data;
                return true;
            }
            temp = temp->_next;
        }
        return false;
    } 
    
    /**
     * Metodo para Eliminar un nodo de la lista
     * @param id Identificador del nodo a eliminar
     * @return bool
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Ninguna
     * @exception Ninguna
     * @test_cases
     */
    public bool DeleteNode(int id)
    {
        NodeLinked<T>* temp = _head;
        NodeLinked<T>* prev = null;
        while (temp != null)
        {
            if (temp->_data is Cliente c && c.Id == id)
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
                return true;
            }

            prev = temp;
            temp = temp->_next;
        }
        return false;
    }
    
    /**
     * Metodo para buscar un nodo en la lista
     * @param id Identificador del nodo a buscar
     * @return Nodo de la lista
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Ninguna
     * @exception Ninguna
     * @test_cases
     */
    public NodeLinked<T>* SearchNode(int id)
    {
        NodeLinked<T>* temp = _head;
        while (temp != null)
        {
            if (temp->_data is Cliente c && c.Id == id)
            {
                return temp;
            }
            temp = temp->_next;
        }
        return null;
    }
    
}