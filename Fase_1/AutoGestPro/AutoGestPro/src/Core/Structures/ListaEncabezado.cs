using System.Runtime.InteropServices;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

public unsafe class ListaEncabezado<T> : IDisposable where T : unmanaged
{
    public NodoEncabezado<int>* primero; // Apunta al primer nodo de la lista
    public NodoEncabezado<int>* ultimo; // Apunta al último nodo de la lista
    public string tipo; // Tipo de la lista (por ejemplo, "Lista de Enteros")
    public int size; // Tamaño actual de la lista (número de nodos)

    // Constructor que inicializa la lista vacía y asigna el tipo
    public ListaEncabezado(string tipo)
    {
        primero = null; // Lista vacía al inicio
        ultimo = null; // Lista vacía al inicio
        this.tipo = tipo; // Asigna el tipo de lista
        size = 0; // La lista comienza con tamaño 0
    }

    // Método para insertar un nuevo nodo con un ID en la lista
    public void insertar_nodoEncabezado(int id)
    {
        // Crear un nuevo nodo de tipo NodoEncabezado en la memoria
        NodoEncabezado<int>* nuevo = (NodoEncabezado<int>*)Marshal.AllocHGlobal(sizeof(NodoEncabezado<int>));
        if (nuevo == null) throw new InvalidOperationException("No se pudo asignar memoria para el nuevo nodo.");

        // Asignar valores iniciales al nuevo nodo
        nuevo->Id = id; // Asigna el ID al nodo
        nuevo->Siguiente = null; // Inicializa el puntero siguiente a null
        nuevo->Anterior = null; // Inicializa el puntero anterior a null
        nuevo->Acceso = null; // Inicializa el puntero acceso a null

        size++; // Incrementa el tamaño de la lista

        // Si la lista está vacía, el nuevo nodo es tanto el primero como el último
        if (primero == null)
        {
            primero = nuevo; // El primer nodo es el nuevo
            ultimo = nuevo; // El último nodo también es el nuevo
        }
        else
        {
            // Insertar el nodo de manera ordenada
            if (nuevo->Id < primero->Id) // Si el ID es menor que el primero
            {
                nuevo->Siguiente = primero; // El siguiente del nuevo es el primero
                primero->Anterior = nuevo; // El anterior del primero es el nuevo
                primero = nuevo; // El nuevo nodo ahora es el primero
            }
            else if (nuevo->Id > ultimo->Id) // Si el ID es mayor que el último
            {
                ultimo->Siguiente = nuevo; // El siguiente del último es el nuevo
                nuevo->Anterior = ultimo; // El anterior del nuevo es el último
                ultimo = nuevo; // El nuevo nodo ahora es el último
            }
            else
            {
                // Si el ID está en medio, buscar el lugar adecuado
                NodoEncabezado<int>* tmp = primero;
                while (tmp != null)
                {
                    // Insertar entre nodos si el ID es menor que el actual
                    if (nuevo->Id < tmp->Id)
                    {
                        nuevo->Siguiente = tmp; // El siguiente del nuevo es el actual
                        nuevo->Anterior = tmp->Anterior; // El anterior del nuevo es el anterior del actual
                        tmp->Anterior->Siguiente = nuevo; // El siguiente del anterior apunta al nuevo
                        tmp->Anterior = nuevo; // El anterior del actual apunta al nuevo
                        break; // Terminamos la inserción
                    }
                    else
                    {
                        tmp = tmp->Siguiente; // Continuar buscando
                    }
                }
            }
        }
    }

    // Método para mostrar todos los nodos de la lista
    public void Mostrar()
    {
        if (primero == null)
        {
            Console.WriteLine("Lista vacía."); // Si la lista está vacía, mostrar un mensaje
            return;
        }

        // Recorremos la lista y mostramos los IDs de los nodos
        NodoEncabezado<int>* tmp = primero;
        while (tmp != null)
        {
            Console.WriteLine("Encabezado " + tipo + " " + Convert.ToString(tmp->Id)); // Mostrar ID
            tmp = tmp->Siguiente; // Avanzar al siguiente nodo
        }
    }

    // Método para obtener un nodo por su ID
    public NodoEncabezado<int>* getEncabezado(int id)
    {
        NodoEncabezado<int>* tmp = primero;
        while (tmp != null)
        {
            if (id == tmp->Id) return tmp; // Si encontramos el nodo, lo retornamos
            tmp = tmp->Siguiente; // Si no, seguimos al siguiente nodo
        }

        return null; // Si no encontramos el nodo, retornamos null
    }
    
    public void Dispose()
    {
        if (primero == null) return; // Si la lista ya está vacía, no hacemos nada

        // Liberar cada nodo de la lista
        while (primero != null)
        {
            NodoEncabezado<int>* tmp = primero;  // Apuntar al primer nodo
            primero = primero->Siguiente;  // Avanzar al siguiente nodo
            Marshal.FreeHGlobal((IntPtr)tmp); // Liberar la memoria del nodo actual
        }
    }

    // Destructor que libera la memoria de todos los nodos al destruir la lista
    ~ListaEncabezado()
    {
        Dispose();
    }
}