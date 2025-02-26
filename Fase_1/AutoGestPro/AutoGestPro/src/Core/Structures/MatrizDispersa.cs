using System.Runtime.InteropServices;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

public unsafe class MatrizDispersa<T> where T : unmanaged
{
    public int capa; // Representa una capa adicional que puede ser utilizada en la visualización de la matriz.
    public ListaEncabezado<int> filas; // Lista de encabezados para las filas
    public ListaEncabezado<int> columnas; // Lista de encabezados para las columnas

    // Constructor de la clase MatrizDispersa
    public MatrizDispersa(int capa)
    {
        this.capa = capa; // Inicializa la capa
        filas = new ListaEncabezado<int>("Fila"); // Inicializa la lista de encabezados de filas
        columnas = new ListaEncabezado<int>("Columna"); // Inicializa la lista de encabezados de columnas
    }

    // Método para insertar un nuevo nodo en la matriz dispersa
    public void insert(int pos_x, int pos_y, string nombre)
    {
        // Creación de un nuevo nodo interno que será insertado en la matriz
        NodoInterno<int>* nodoInterno = (NodoInterno<int>*)Marshal.AllocHGlobal(sizeof(NodoInterno<int>));
        if (nodoInterno == null) throw new OutOfMemoryException("Error al asignar memoria.");
        
        // Inicializar el nuevo nodo con los valores proporcionados
        *nodoInterno = new NodoInterno<int>(1, nombre, pos_x, pos_y);

        // Verificar si ya existen los encabezados para la fila y columna en la matriz
        NodoEncabezado<int>* encabezadoX  = filas.getEncabezado(nodoInterno->coordenadaX); // Obtener el encabezado de la fila
        NodoEncabezado<int>* encabezadoY  = columnas.getEncabezado(nodoInterno->coordenadaY); // Obtener el encabezado de la columna

        // Si el encabezado de la fila no existe, crearlo
        if (encabezadoX == null)
        {
            encabezadoX = (NodoEncabezado<int>*)Marshal.AllocHGlobal(sizeof(NodoEncabezado<int>));
            if (encabezadoX == null) throw new InvalidOperationException("No se pudo asignar memoria para el nuevo encabezado.");

            *encabezadoX = new NodoEncabezado<int>(nodoInterno->coordenadaX);
            filas.insertar_nodoEncabezado(encabezadoX->id);
            encabezadoX = filas.getEncabezado(nodoInterno->coordenadaX);
        }

        // Si el encabezado de la columna no existe, crearlo
        if (encabezadoY == null)
        {
            encabezadoY = (NodoEncabezado<int>*)Marshal.AllocHGlobal(sizeof(NodoEncabezado<int>));
            if (encabezadoY == null) throw new InvalidOperationException("No se pudo asignar memoria para el nuevo encabezado.");
            
            *encabezadoY = new NodoEncabezado<int>(nodoInterno->coordenadaY); // Inicializar el encabezado de la columna
            
            columnas.insertar_nodoEncabezado(encabezadoY->id); // Insertar el nuevo encabezado en la lista de columnas
            encabezadoY = columnas.getEncabezado(nodoInterno->coordenadaY); // Obtener el encabezado de la columna
        }

        // Verificar que ambos encabezados hayan sido creados correctamente
        if (encabezadoX == null || encabezadoY == null)
        {
            throw new InvalidOperationException("Error al crear los encabezados.");
        }

        // Insertar el nuevo nodo en la fila correspondiente
        if (encabezadoX->acceso == null)
        {
            encabezadoX->acceso = nodoInterno; // Si la fila está vacía, asignamos el nuevo nodo como el primer acceso
        }
        else
        {
            if (nodoInterno->coordenadaY < encabezadoX->acceso->coordenadaY)
            {
                nodoInterno->derecha = encabezadoX->acceso;
                encabezadoX->acceso->izquierda = nodoInterno;
                encabezadoX->acceso = nodoInterno;
            }
            else
            {
                NodoInterno<int>* tmp1 = encabezadoX->acceso;
                while (tmp1 != null)
                {
                    if (nodoInterno->coordenadaY < tmp1->coordenadaY)
                    {
                        nodoInterno->derecha = tmp1;
                        nodoInterno->izquierda = tmp1->izquierda;
                        tmp1->izquierda->derecha = nodoInterno;
                        tmp1->izquierda = nodoInterno;
                        break;
                    }
                    else
                    {
                        if (tmp1->derecha == null)
                        {
                            tmp1->derecha = nodoInterno;
                            nodoInterno->izquierda = tmp1;
                            break;
                        }
                        else
                        {
                            tmp1 = tmp1->derecha;
                        }
                    }
                }
            }
        }

        // Insertar el nuevo nodo en la columna correspondiente
        if (encabezadoY->acceso == null)
        {
            encabezadoY->acceso = nodoInterno; // Si la columna está vacía, asignamos el nuevo nodo como el primer acceso
        }
        else
        {
            if (nodoInterno->coordenadaX < encabezadoY->acceso->coordenadaX)
            {
                nodoInterno->abajo = encabezadoY->acceso;
                encabezadoY->acceso->arriba = nodoInterno;
                encabezadoY->acceso = nodoInterno;
            }
            else
            {
                NodoInterno<int>* tmp2 = encabezadoY->acceso;
                while (tmp2 != null)
                {
                    if (nodoInterno->coordenadaX < tmp2->coordenadaX)
                    {
                        nodoInterno->abajo = tmp2;
                        nodoInterno->arriba = tmp2->arriba;
                        tmp2->arriba->abajo = nodoInterno;
                        tmp2->arriba = nodoInterno;
                        break;
                    }
                    else
                    {
                        if (tmp2->abajo == null)
                        {
                            tmp2->abajo = nodoInterno;
                            nodoInterno->arriba = tmp2;
                            break;
                        }
                        else
                        {
                            tmp2 = tmp2->abajo;
                        }
                    }
                }
            }
        }
    }

    // Método para mostrar la matriz dispersa
    public void mostrar()
    {
        // Imprimir los encabezados de columnas
        NodoEncabezado<int>* y_columna = columnas.primero;
        Console.Write("\t"); // Espacio inicial para alinear las columnas

        // Imprimir los IDs de las columnas
        while (y_columna != null)
        {
            Console.Write(y_columna->id + "\t"); // Imprimir el encabezado de cada columna
            y_columna = y_columna->siguiente;
        }

        Console.WriteLine(); // Salto de línea después de las cabeceras de las columnas

        // Imprimir los nodos de cada fila
        NodoEncabezado<int>* x_fila = filas.primero;
        while (x_fila != null)
        {
            // Imprimir el encabezado de la fila
            Console.Write(x_fila->id + "\t");

            // Imprimir los valores de la fila
            NodoInterno<int>* interno = x_fila->acceso;
            NodoEncabezado<int>* y_columna_iter = columnas.primero;

            // Imprimir los valores de las columnas de la fila
            while (y_columna_iter != null)
            {
                if (interno != null && interno->coordenadaY == y_columna_iter->id)
                {
                    Console.Write(interno->nombre + "\t"); // Si el nodo interno existe, mostrar su nombre
                    interno = interno->derecha; // Mover al siguiente nodo en la fila
                }
                else
                {
                    Console.Write(
                        "0\t"); // Si no hay nodo, mostrar 0 (representa la ausencia de un valor en esa posición)
                }

                y_columna_iter = y_columna_iter->siguiente; // Avanzar a la siguiente columna
            }

            Console.WriteLine(); // Salto de línea después de imprimir una fila
            x_fila = x_fila->siguiente; // Avanzar a la siguiente fila
        }
    }
    
    // Top 5 de Vehiculos con mas Servicios
    public Dictionary<int,int> GetTopVehiculosConMasServicios()
    {
        // Diccionario para los objetos Vehiculo
        Dictionary<int, int> diccionario = new Dictionary<int, int>();
        
        // Recorrer la lista de encabezados de filas
        NodoEncabezado<int>* fila = filas.primero;
        
        // Numero de Servicios
        int numServicios = 0;

        while (fila != null)
        {
            NodoInterno<int>* listaInterna = fila->acceso;
            NodoEncabezado<int>* columna = columnas.primero;

            while (columna != null)
            {
                if (listaInterna != null && listaInterna->coordenadaY == listaInterna->id)
                {
                    numServicios += 1;
                }
                columna = columna->siguiente;
            }
            
            // Agregar el numero de servicios al diccionario
            diccionario.Add(fila->id, numServicios);
            fila = fila->siguiente;
        }
        
        // Ordenar el diccionario por el numero de servicios del mas alto al mas pequeño
        diccionario = diccionario.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        
        // Retornar el diccionario
        return diccionario;
        
    }

    // Destructor para liberar la memoria de los nodos internos y encabezados
    ~MatrizDispersa()
    {
        // Liberar memoria de los nodos internos y encabezados de filas
        NodoEncabezado<int>* x_fila = filas.primero;
        while (x_fila != null)
        {
            // Liberar los nodos internos de la fila
            NodoInterno<int>* interno = x_fila->acceso;
            while (interno != null)
            {
                NodoInterno<int>* tmp = interno;
                interno = interno->derecha;
                if (tmp != null)
                {
                    Marshal.FreeHGlobal((IntPtr)tmp);
                }
            }

            // Liberar el encabezado de fila
            NodoEncabezado<int>* tmp_fila = x_fila;
            x_fila = x_fila->siguiente;
            if (tmp_fila != null)
            {
                Marshal.FreeHGlobal((IntPtr)tmp_fila);
            }
        }

        // Liberar memoria de los nodos internos y encabezados de columnas
        NodoEncabezado<int>* x_columna = columnas.primero;
        while (x_columna != null)
        {
            // Liberar los nodos internos de la columna
            NodoInterno<int>* interno = x_columna->acceso;
            while (interno != null)
            {
                NodoInterno<int>* tmp = interno;
                interno = interno->abajo;
                if (tmp != null)
                {
                    Marshal.FreeHGlobal((IntPtr)tmp);
                }
            }

            // Liberar el encabezado de columna
            NodoEncabezado<int>* tmp_columna = x_columna;
            x_columna = x_columna->siguiente;
            if (tmp_columna != null)
            {
                Marshal.FreeHGlobal((IntPtr)tmp_columna);
            }
        }
    }
}