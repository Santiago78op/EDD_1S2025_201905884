using System.Runtime.InteropServices;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

public unsafe class Double_List<T> : IDouble_List<T>, IDisposable where T : class
{
     private NodeDouble<T>* _head;
     private NodeDouble<T>* _tail;
     int length;

     public NodeDouble<T>* Head
     {
          get => _head;
          set => _head = value;
     }

     public NodeDouble<T>* Tail
     {
          get => _tail;
          set => _tail = value;
     }

     public int Length
     {
          get => length;
          set => length = value;
     }

     public Double_List()
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
     public void append(T data)
     {
          NodeDouble<T>* newNode = (NodeDouble<T>*)Marshal.AllocHGlobal(sizeof(NodeDouble<T>));
          if (newNode == null)
          {
               throw new OutOfMemoryException("No se pudo reservar memoria para el nuevo nodo");
          }

          if (length == 0)
          {
               _head = newNode;
               _tail = newNode;
          }
          else
          {
               _tail->_next = newNode;
               newNode->_prev = _tail;
               _tail = newNode;
          }
          
          length++;
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
     public void remove(T data)
     {
          NodeDouble<T>* temp = _head;
          NodeDouble<T>* prev = null;
          
          while (temp != null)
          {
               if (temp->_data.Equals(data))
               {
                    if (prev == null)
                    {
                         _head = temp->_next;
                         if (_head != null)
                         {
                              _head->_prev = null;
                         }
                    }
                    else
                    {
                         prev->_next = temp->_next;
                         if (temp->_next != null)
                         {
                              temp->_next->_prev = prev;
                         }
                    }
                    Marshal.FreeHGlobal((IntPtr)temp);
                    length--;
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
          NodeDouble<T>* temp = _head;
          
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
      * Metodo para liberar la memoria de la lista
      * @return void
      * @complexity O(n)
      * @precondition Ninguna
      * @postcondition Ninguna
      * @exception Ninguna
      * @test_cases
      */
     public void Dispose()
     {
          NodeDouble<T>* temp = _head;
          NodeDouble<T>* next = null;
          
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
     
     ~Double_List()
     {
          Dispose();
     }
     
     /**
      * Metodo para obtener un nodo de la lista
      * @param index Indice del nodo a obtener
      * @return NodeDouble<T> Nodo de la lista
      * @complexity O(n)
      * @precondition Ninguna
      * @postcondition Ninguna
      * @exception Ninguna
      * @test_cases
      */
     public NodeDouble<T>* GetNode(int index)
     {
          NodeDouble<T>* temp = _head;
          int i = 0;
          
          while (temp != null)
          {
               if (i == index)
               {
                    return temp;
               }
               temp = temp->_next;
               i++;
          }
          
          return null;
     }
}