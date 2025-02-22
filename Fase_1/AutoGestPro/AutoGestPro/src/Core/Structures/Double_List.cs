using System.Runtime.InteropServices;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Models;
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
          
          /* Se asigna el dato al nuevo nodo */
          *newNode = new NodeDouble<T>(data);
          newNode->_next = null;
          newNode->_prev = null;

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
          if (index < 0 || index >= Length) return null;

          NodeDouble<T>*temp = _head;

          if (index < Length/2){
               for (int i = 0; i < index; ++i) {
                    temp = temp->_next;
               }
          } else {
               temp = _tail;
               for (int i = Length - 1; i > index; --i) {
                    temp = temp->_prev;
               }
          }
          return temp;
     }
     
     /**
      * Metodo para buscar un nodo en la lista
      * @param id Identificador del nodo a buscar
      * @return NodeDouble<T> Nodo de la lista
      * @complexity O(n)
      * @precondition Ninguna
      * @postcondition Ninguna
      * @exception Ninguna
      * @test_cases
      */
     public NodeDouble<T>* searchNode(int id)
     {
          NodeDouble<T>* temp = _head;
          
          while (temp != null)
          {
               if (temp->_data is Vehiculo v && v.Id == id)
               {
                    return temp;
               }
               temp = temp->_next;
          }
          
          return null;
     }
     
     // Sort asendente de la Lista Doble
     public void Sort()
     {
          NodeDouble<T>* temp = _head;
          NodeDouble<T>* temp2 = null;
          T data;
          
          while (temp != null)
          {
               temp2 = temp->_next;
               while (temp2 != null)
               {
                    if (temp->_data is Vehiculo v1 && temp2->_data is Vehiculo v2)
                    {
                         if (v1.Modelo > v2.Modelo)
                         {
                              data = temp->_data;
                              temp->_data = temp2->_data;
                              temp2->_data = data;
                         }
                    }
                    temp2 = temp2->_next;
               }
               temp = temp->_next;
          }
     }

     public Double_List<Vehiculo> GetTopVehiculosMasAntiguos(int i)
     {
          Sort();
          Double_List<Vehiculo> topVehiculos = new Double_List<Vehiculo>();
          
          NodeDouble<Vehiculo>* temp = (NodeDouble<Vehiculo>*)_head;
          for (int j = 0; j < i; j++)
          {
               if (temp == null) break;
               topVehiculos.append(temp->_data);
               temp = temp->_next;
          }
          
          return topVehiculos;
     }
}