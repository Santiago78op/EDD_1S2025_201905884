using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/// <summary>
/// Clase que representa una lista doblemente enlazada.
/// </summary>
public class DoubleList : IDoubleList, IDisposable
{
    private NodeDouble _head;
    private NodeDouble _tail;
    private int _length;
    
    /**
     * Constructor de la lista doblemente enlazada
     */
    public DoubleList()
    {
        _head = null;
        _tail = null;
        _length = 0;
    }

    // Getters y setters
    public NodeDouble Head
    {
        get => _head;
        set => _head = value ?? throw new ArgumentNullException(nameof(value));
    }

    public NodeDouble Tail
    {
        get => _tail;
        set => _tail = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Length
    {
        get => _length;
        set => _length = value;
    }
    
    // Metodos y Funciones de la lista enlazada doblemente enlazada

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
    public void Append(Object data)
    {
        NodeDouble newNodeDouble = new NodeDouble(data);

        if (_length == 0)
        {
            _head = newNodeDouble;
            _tail = newNodeDouble;
        }
        else
        {
            _tail.Next = newNodeDouble;
            newNodeDouble.Previous = _tail;
            _tail = newNodeDouble;
        }

        _length++;
    }
    
    /**
     * Metodo para eliminar el primer nodo de la lista
     * @return void
     * @complexity O(1)
     * @precondition La lista no debe estar vacia
     * @postcondition Se elimina el primer nodo de la lista
     * @exception InvalidOperationException La lista esta vacia
     * @test_cases
     */
    public void RemoveFirst()
    {
        if (_length == 0)
        {
            throw new InvalidOperationException("The list is empty");
        }
        
        _head = _head.Next;
        if (_head == null)
        {
            _tail = null;
        }
        else
        {
            _head.Previous = null;
        }

        _length--;
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
    public bool Remove(Object data)
    {
        NodeDouble current = _head;
        NodeDouble previous = null;

        while (current != null)
        {
            if (current.Data.Equals(data))
            {
                if (previous != null)
                {
                    previous.Next = current.Next;
                    if (current.Next == null)
                    {
                        _tail = previous;
                    }
                    else
                    {
                        current.Next.Previous = previous;
                    }
                    _length--;
                }
                else
                {
                    RemoveFirst();
                }

                return true;
            }

            previous = current;
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
        NodeDouble current = _head;
        while (current != null)
        {
            Console.WriteLine(current.Data);
            current = current.Next;
        }
    }
    
    /**
     * Metodo para obtener un nodo de la lista
     * @param index Indice del nodo a obtener
     * @return NodeDouble
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se obtiene un nodo de la lista
     * @exception IndexOutOfRangeException Indice fuera de rango
     * @test_cases
     */
    public NodeDouble GetNode(int index)
    {
        if (index < 0 || index >= _length)
        {
            throw new IndexOutOfRangeException("Index out of range");
        }
        
        NodeDouble current = _head;
        
        if(index <  _length / 2)
        {
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
        }
        else
        {
            current = _tail;
            for (int i = _length - 1; i > index; i--)
            {
                current = current.Previous;
            }
        }
        
        return current;
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
    public bool ModifyNode(int id, Object data)
    {
        NodeDouble current = _head;
        while (current != null)
        {
            var currentId = current.Data.GetType().GetProperty("Id")?.GetValue(current.Data);
            if (currentId != null && currentId.Equals(id))
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
        NodeDouble current = _head;
        NodeDouble previous = null;

        while (current != null)
        {
            var currentId = current.Data.GetType().GetProperty("Id")?.GetValue(current.Data);
            if (currentId != null && currentId.Equals(id))
            {
                if (previous != null)
                {
                    previous.Next = current.Next;
                    if (current.Next == null)
                    {
                        _tail = previous;
                    }
                    else
                    {
                        current.Next.Previous = previous;
                    }
                    _length--;
                }
                else
                {
                    RemoveFirst();
                }

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
     * @return NodeDouble
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se busca un nodo en la lista
     * @exception Ninguna
     * @test_cases
     */
    public NodeDouble SearchNode(int id)
    {
        NodeDouble current = _head;
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
     * Metodo para liberar los recursos de la lista
     * @return void
     * @complexity O(1)
     * @precondition Ninguna
     * @postcondition Se liberan los recursos de la lista
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
     * Destructor de la lista doblemente enlazada
     */
    ~DoubleList()
    {
        Dispose();
    }

    public string GenerarDotVehiculos()
    {
        throw new NotImplementedException();
    }
}