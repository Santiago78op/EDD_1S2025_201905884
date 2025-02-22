using System.Runtime.InteropServices;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

public unsafe class StackList<T> : IStackList<T>, IDisposable where T : class
{
    private NodeStack<T>* _top;
    int height;
    
    public StackList()
    {
        _top = null;
        height = 0;
    }

    public NodeStack<T>* Top
    {
        get => _top;
        set => _top = value;
    }

    public int Height
    {
        get => height;
        set => height = value;
    }
    
    /**
     * Metodo para agregar un nuevo nodo al inicio de la pila
     * @param data Dato a almacenar en el nodo
     * @return void
     * @complexity O(1)
     * @precondition Ninguna
     * @postcondition Se añade un nodo al inicio de la pila
     * @exception Ninguna
     * @test_cases
     */
    public void push(T data)
    {
        NodeStack<T>* newNodeStack = (NodeStack<T>*)Marshal.AllocHGlobal(sizeof(NodeStack<T>));
        if (newNodeStack == null)
        {
            throw new OutOfMemoryException("No se pudo reservar memoria para el nuevo nodo");
        }

        *newNodeStack = new NodeStack<T>(data);
        newNodeStack->_next = _top;
        _top = newNodeStack;
        height++;
    }
    
    /**
     * Metodo para eliminar el nodo del inicio de la pila
     * @return void
     * @complexity O(1)
     * @precondition La pila no debe estar vacía
     * @postcondition Se elimina el nodo del inicio de la pila
     * @exception Ninguna
     * @test_cases
     */
    public NodeStack<T>* pop()
    {
        if (_top == null)
        {
            throw new InvalidOperationException("La pila está vacía");
        }

        NodeStack<T>* temp = _top;
        _top = _top->_next;
        height--;
        return temp;
    }
    
    
    public NodeStack<T>* peek()
    {
        return _top;
    }
    
    /**
     * Metodo para imprimir la pila
     * @return void
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se imprime la pila
     * @exception Ninguna
     * @test_cases
     */
    public void printStack()
    {
        NodeStack<T>* temp = _top;
        while (temp != null)
        {
            Console.WriteLine(temp->_data);
            temp = temp->_next;
        }
    }
    
    /**
     * Metodo para liberar la memoria de la pila
     * @return void
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se libera la memoria de la pila
     * @exception Ninguna
     * @test_cases
     */
    public void Dispose()
    {
        NodeStack<T>* temp = _top;
        while (temp != null)
        {
            NodeStack<T>* next = temp->_next;
            Marshal.FreeHGlobal((IntPtr)temp);
            temp = next;
        }
        
        _top = null;
        GC.SuppressFinalize(this);
    }
    
    /**
     * Destructor de la clase
     */
    ~StackList()
    {
        Dispose();
    }
    
    /**
     * Metodo para obtener un nodo de la pila
     * @param index Indice del nodo a obtener
     * @return NodeStack<T> Nodo de la pila
     * @complexity O(n)
     * @precondition El índice debe estar dentro del rango
     * @postcondition Se obtiene el nodo de la pila
     * @exception IndexOutOfRangeException
     * @test_cases
     */
    public NodeStack<T>* GetNode(int index)
    {
        if (index < 0 || index >= height)
        {
            throw new IndexOutOfRangeException("El índice está fuera de rango");
        }

        NodeStack<T>* temp = _top;
        for (int i = 0; i < index; i++)
        {
            temp = temp->_next;
        }

        return temp;
    }
    
    /**
     * Metodo para buscar un nodo en la pila
     * @param id Identificador del nodo a buscar
     * @return NodeStack<T> Nodo de la pila
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se obtiene el nodo de la pila
     * @exception Ninguna
     * @test_cases
     */
    public NodeStack<T>* searchNode(int id)
    {
        NodeStack<T>* temp = _top;
        while (temp != null)
        {
            if (temp->_data is Factura f && f.Id == id)
            {
                return temp;
            }

            temp = temp->_next;
        }

        return null;
    }
    
}