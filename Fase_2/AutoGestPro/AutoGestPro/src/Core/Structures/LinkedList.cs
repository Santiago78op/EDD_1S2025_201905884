using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

public class LinkedList : ILinkedList, IDisposable
{
    private NodeLinked? _head;
    private NodeLinked? _tail;
    private int _length;

    /**
     * Constructor de la lista enlazada
     */
    public LinkedList()
    {
        _head = null;
        _tail = null;
        _length = 0;
    }

    // Getters y setters
    public NodeLinked Head
    {
        get => _head ?? throw new InvalidOperationException("Head is null");
        set => _head = value ?? throw new ArgumentNullException(nameof(value));
    }

    public NodeLinked Tail
    {
        get => _tail ?? throw new InvalidOperationException("Tail is null");
        set => _tail = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Length
    {
        get => _length;
        set => _length = value;
    }

    // Metodos y Funciones de la lista enlazada simplemente enlazada

    /**
     * Metodo para agregar un nuevo nodo al final de la lista
     * @param data Dato a almacenar en el nodo
     * @return void
     * @complexity O(1)
     * @precondition Ninguna
     * @postcondition Se añade un nodo al final de la lista
     * @exception Ninguna
     * @test_cases
     */
    public void Append(object data)
    {
        NodeLinked newNodeLinked = new NodeLinked(data);

        // Se asigna el dato al nuevo nodo
        if (_length == 0)
        {
            _head = newNodeLinked; // El primer nodo es la cabeza
            _tail = newNodeLinked; // El primer nodo es la cola
        }
        else
        {
            // Se asigna el nuevo nodo al final de la lista
            _tail!.Next = newNodeLinked; // El nodo siguiente de la cola es el nuevo nodo
            _tail = newNodeLinked; // El nuevo nodo es la cola
        }

        _length++;
    }

    /**
     * Metodo para eliminar un nodo de la lista
     * @param data Dato a eliminar de la lista
     * @return bool
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se elimina un nodo de la lista
     * @exception Ninguna
     * @test_cases
     */
    public bool Remove(object data)
    {
        // Validar si la lista esta vacia
        if (_head == null) return false;

        // Validar si el nodo a eliminar es la cabeza
        if (_head.Data.Equals(data))
        {
            _head = _head.Next;
            if (_head == null) _tail = null;
            _length--;
            return true;
        }

        // Buscar el nodo a eliminar
        NodeLinked current = _head;
        while (current.Next != null)
        {
            // Validar si el nodo a eliminar es el siguiente
            if (current.Next.Data.Equals(data))
            {
                current.Next = current.Next.Next;
                if (current.Next == null) _tail = current;
                _length--;
                return true;
            }

            current = current.Next;
        }

        return false;
    }

    /**
     * Metodo para imprimir la lista
     * @return void
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se imprime la lista
     * @exception Ninguna
     * @test_cases
     */
    public void PrintList()
    {
        NodeLinked? current = _head;
        while (current != null)
        {
            Console.WriteLine(current.Data);
            current = current.Next;
        }
    }

    /**
     * Metodo para obtener un nodo de la lista
     * @param index Indice del nodo a obtener
     * @return NodeLinked
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se obtiene un nodo de la lista
     * @exception Ninguna
     * @test_cases
     */
    public NodeLinked GetNode(int index)
    {
        NodeLinked? current = _head;
        int i = 0;
        while (current != null)
        {
            if (i == index) return current;
            current = current.Next;
            i++;
        }

        return null;
    }

    /**
     * Metodo para modificar un nodo de la lista
     * @param id Identificador del nodo a modificar
     * @param data Dato a modificar
     * @return bool
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se modifica un nodo de la lista
     * @exception Ninguna
     * @test_cases
     */
    public bool ModifyNode(int id, object data)
    {
        NodeLinked? current = _head;
        while (current != null)
        {
            if (current.Data.GetType().GetProperty("Id")?.GetValue(current.Data)?.Equals(id) == true)
            {
                current.Data = data;
                return true;
            }

            current = current.Next;
        }

        return false;
    }
    
    /**
     * Metodo para eliminar un nodo de la lista
     * @param id Identificador del nodo a eliminar
     * @return bool
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se elimina un nodo de la lista
     * @exception Ninguna
     * @test_cases
     */
    public bool DeleteNode(int id)
    {
        NodeLinked? current = _head;
        NodeLinked? previous = null;

        while (current != null)
        {
            var currentId = current.Data.GetType().GetProperty("Id")?.GetValue(current.Data);
            if (currentId != null && currentId.Equals(id))
            {
                if (previous == null)
                {
                    _head = current.Next;
                    if (_head == null) _tail = null;
                }
                else
                {
                    previous.Next = current.Next;
                    if (current.Next == null) _tail = previous;
                }

                _length--;
                return true;
            }

            previous = current;
            current = current.Next;
        }

        return false;
    }
    
    /**
     * Metodo para buscar un nodo en la lista
     * @param id Identificador del nodo a buscar
     * @return NodeLinked
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se busca un nodo en la lista
     * @exception Ninguna
     * @test_cases
     */
    public NodeLinked SearchNode(int id)
    {
        NodeLinked? current = _head;
        while (current != null)
        {
            var currentId = current.Data.GetType().GetProperty("Id")?.GetValue(current.Data);
            if (currentId != null && currentId.Equals(id))
            {
                return current;
            }

            current = current.Next;
        }

        return null;
    }

    /**
     * Metodo para liberar los recursos de la lista enlazada
     * @return void
     * @complexity O(1)
     * @precondition Ninguna
     * @postcondition Se liberan los recursos de la lista enlazada
     * @exception Ninguna
     * @test_cases
     */
    public void Dispose()
    {
        _head = null;
        _tail = null;
        _length = 0;
    }
    
    /**
     * Destructor de la lista enlazada
     */
    ~LinkedList()
    {
        Dispose();
    }
}