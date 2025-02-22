using System.Runtime.InteropServices;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

public unsafe class RingList<T> : IRingList<T>, IDisposable where T : class
{
    private NodeRing<T>* _head;
    private NodeRing<T>* _tail;
    int length;

    public NodeRing<T>* Head
    {
        get => _head;
        set => _head = value;
    }

    public NodeRing<T>* Tail
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
     * Constructor de la lista circular
     */
    public RingList()
    {
        _head = null;
        _tail = null;
        length = 0;
    }
    
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
    public void append(T data){
        NodeRing<T>* newNodeRing = (NodeRing<T>*)Marshal.AllocHGlobal(sizeof(NodeRing<T>));
        
        if (newNodeRing == null)
        {
            throw new OutOfMemoryException("No se pudo reservar memoria para el nuevo nodo");
        }
        
        /* Se asigna el dato al nuevo nodo */
        *newNodeRing = new NodeRing<T>(data);
        newNodeRing->_next = null;

        if (length == 0)
        {
            _head = newNodeRing;
            _head->_next = _head;
            _tail = _head;
        }
        else
        {
            _tail->_next = newNodeRing;
            newNodeRing->_next = _head;
            _tail = newNodeRing;
        }
        
        length++;
    }
    
    /**
     * Metodo para eliminar un nodo de la lista
     * @param data Dato a eliminar de la lista
     * @return void
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se elimina un nodo de la lista
     * @exception Ninguna
     * @test_cases
     */
    public void remove(T data){
        NodeRing<T>* current = _head;
        NodeRing<T>* previous = _tail;
        
        do
        {
            if (current->_data == data)
            {
                if (current == _head)
                {
                    _head = current->_next;
                    _tail->_next = _head;
                }
                else if (current == _tail)
                {
                    _tail = previous;
                    _tail->_next = _head;
                }
                else
                {
                    previous->_next = current->_next;
                }
                
                Marshal.FreeHGlobal((IntPtr)current);
                length--;
                return;
            }
            
            previous = current;
            current = current->_next;
        } while (current != _head);
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
    public void printList(){
        NodeRing<T>* current = _head;
        
        do
        {
            Console.WriteLine(current->_data);
            current = current->_next;
        } while (current != _head);
    }
    
    /**
     * Metodo para liberar la memoria de la lista
     * @return void
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Se libera la memoria de la lista
     * @exception Ninguna
     * @test_cases
     */
    public void Dispose()
    {
        NodeRing<T>* current = _head;
        NodeRing<T>* next;
        
        do
        {
            next = current->_next;
            Marshal.FreeHGlobal((IntPtr)current);
            current = next;
        } while (current != _head);
        
        _head = null;
        _tail = null;
        length = 0;
    }
    
    /**
     * Destructor de la lista circular
     */
    ~RingList()
    {
        Dispose();
    }
    
    /**
     * Metodo para obtener un nodo de la lista
     * @param index Indice del nodo a obtener
     * @return NodeRing<T> Nodo de la lista
     * @complexity O(n)
     * @precondition Ninguna
     * @postcondition Ninguna
     * @exception Ninguna
     * @test_cases
     */
    public NodeRing<T>* GetNode(int index)
    {
        if (index < 0 || index >= length) return null;

        NodeRing<T>* current = _head;

        for (int i = 0; i < index; i++)
        {
            current = current->_next;
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
     * @postcondition Ninguna
     * @exception Ninguna
     * @test_cases
     */
    public NodeRing<T>* searchNode(int id)
    {
        NodeRing<T>* current = _head;
        
        do
        {
            if (current->_data is Repuesto r && r.Id == id)
            {
                return current;
            }
            
            current = current->_next;
        } while (current != _head);
        
        return null;
    }
}