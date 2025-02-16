using System.Runtime.InteropServices;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

public unsafe class ListQueue<T> : IQueue<T>, IDisposable where T : class
{
    private NodeQueue<T>* _head;
    private NodeQueue<T>* _tail;
    int length;

    public ListQueue()
    {
        _head = null;
        _tail = null;
        length = 0;
    }

    public NodeQueue<T>* Head
    {
        get => _head;
        set => _head = value;
    }

    public NodeQueue<T>* Tail
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
     * Metodo para agregar un nuevo nodo al final de la cola
     * @param data Dato a almacenar en el nodo
     * @return void
     * @complexity O(1)
     * @precondition Ninguna
     * @postcondition Se añade un nodo al final de la cola
     * @exception Ninguna
     * @test_cases
     */
    public void enqueue(T data)
    {
        NodeQueue<T>* newNodeQueue = (NodeQueue<T>*)Marshal.AllocHGlobal(sizeof(NodeQueue<T>));
        if (newNodeQueue == null)
        {
            throw new OutOfMemoryException("No se pudo reservar memoria para el nuevo nodo");
        }

        *newNodeQueue = new NodeQueue<T>(data);
        newNodeQueue->_next = null;

        if (length == 0)
        {
            _head = newNodeQueue;
            _tail = newNodeQueue;
        }
        else
        {
            _tail->_next = newNodeQueue;
            _tail = newNodeQueue;
        }

        length++;
    }
    
    /**
     * Metodo para eliminar un nodo de la cola
     * @return void
     * @complexity O(1)
     * @precondition La cola no debe estar vacía
     * @postcondition Se elimina un nodo de la cola
     * @exception InvalidOperationException
     * @test_cases
     */
    public void dequeue()
    {
        if (length == 0)
        {
            throw new InvalidOperationException("No se puede eliminar un nodo de una cola vacía");
        }
        
        NodeQueue<T>* temp = _head;

        if (length == 1)
        {
            _head = null;
            _tail = null;
        }
        else
        {
            _head = _head->_next;
        }
        
        Marshal.FreeHGlobal((IntPtr)temp);
        length--;
    }
    
    /**
     * Metodo para imprimir la cola
     * @return void
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se imprime la cola
     * @exception Ninguna
     * @test_cases
     */
    public void printQueue()
    {
        if (length == 0)
        {
            Console.WriteLine("La cola está vacía");
        }
        else
        {
            NodeQueue<T>* temp = _head;
            while (temp != null)
            {
                Console.WriteLine(temp->_data);
                temp = temp->_next;
            }
        }
    }
    
    /**
     * Metodo para liberar la memoria de la cola
     */
    public void Dispose()
    {
        NodeQueue<T>* temp = _head;
        NodeQueue<T>* next = null;
        while (temp != null)
        {
            next = temp->_next;
            Marshal.FreeHGlobal((IntPtr)temp);
            temp = next;
        }
        
        _head = null;
        _tail = null;
        GC.SuppressFinalize(this);
    }
    
    /**
     * Destructor de la clase
     */
    ~ListQueue()
    {
        Dispose();
    }
    
    /**
     * Metodo para obtener un nodo de la cola
     * @param index Indice del nodo a obtener
     * @return NodeQueue<T> Nodo de la cola
     * @complexity O(n)
     * @precondition El índice debe estar dentro del rango de la cola
     * @postcondition Se obtiene un nodo de la cola
     * @exception IndexOutOfRangeException
     * @test_cases
     */
    public NodeQueue<T>* GetNode(int index)
    {
        if (index < 0 || index >= length)
        {
            throw new IndexOutOfRangeException("El índice está fuera de rango");
        }

        NodeQueue<T>* temp = _head;
        for (int i = 0; i < index; i++)
        {
            temp = temp->_next;
        }

        return temp;
    }
}