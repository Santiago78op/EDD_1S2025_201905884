using System.Runtime.InteropServices;
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
        NodoInterno<int>* nuevo = (NodoInterno<int>*)Marshal.AllocHGlobal(sizeof(NodoInterno<int>));
        nuevo->Id = 1; // Asigna un ID al nodo
        nuevo->Nombre = nombre; // Asigna el nombre proporcionado al nodo
        nuevo->coordenadaX = pos_x; // Asigna la coordenada X (fila)
        nuevo->coordenadaY = pos_y; // Asigna la coordenada Y (columna)
        nuevo->Arriba = null;
        nuevo->Abajo = null;
        nuevo->Derecha = null;
        nuevo->Izquierda = null;

        // Verificar si ya existen los encabezados para la fila y columna en la matriz
        NodoEncabezado<int>* nodo_X = filas.getEncabezado(pos_x); // Obtener el encabezado de la fila
        NodoEncabezado<int>* nodo_Y = columnas.getEncabezado(pos_y); // Obtener el encabezado de la columna

        // Si el encabezado de la fila no existe, crearlo
        if (nodo_X == null)
        {
            filas.insertar_nodoEncabezado(pos_x); // Crear encabezado para la fila
            nodo_X = filas.getEncabezado(pos_x); // Obtener el encabezado de la fila recién creado
        }

        // Si el encabezado de la columna no existe, crearlo
        if (nodo_Y == null)
        {
            columnas.insertar_nodoEncabezado(pos_y); // Crear encabezado para la columna
            nodo_Y = columnas.getEncabezado(pos_y); // Obtener el encabezado de la columna recién creado
        }

        // Verificar que ambos encabezados hayan sido creados correctamente
        if (nodo_X == null || nodo_Y == null)
        {
            throw new InvalidOperationException("Error al crear los encabezados.");
        }

        // Insertar el nuevo nodo en la fila correspondiente
        if (nodo_X->Acceso == null)
        {
            nodo_X->Acceso = nuevo; // Si la fila está vacía, asignamos el nuevo nodo como el primer acceso
        }
        else
        {
            // Si ya hay nodos en la fila, buscamos el lugar adecuado para insertar el nuevo nodo
            NodoInterno<int>* tmp = nodo_X->Acceso;
            while (tmp != null)
            {
                // Si la columna del nuevo nodo es menor que la columna del nodo actual, insertamos el nuevo nodo antes
                if (nuevo->coordenadaY < tmp->coordenadaY)
                {
                    nuevo->Derecha = tmp;
                    nuevo->Izquierda = tmp->Izquierda;
                    tmp->Izquierda->Derecha = nuevo;
                    tmp->Izquierda = nuevo;
                    break;
                }
                else if (nuevo->coordenadaX == tmp->coordenadaX && nuevo->coordenadaY == tmp->coordenadaY)
                {
                    // Verificar que no haya nodos duplicados (con las mismas coordenadas)
                    break;
                }
                else
                {
                    // Si no hemos encontrado el lugar, seguimos buscando
                    if (tmp->Derecha == null)
                    {
                        tmp->Derecha = nuevo; // Insertamos el nodo al final de la fila
                        nuevo->Izquierda = tmp;
                        break;
                    }
                    else
                    {
                        tmp = tmp->Derecha; // Avanzamos al siguiente nodo en la fila
                    }
                }
            }
        }

        // Insertar el nuevo nodo en la columna correspondiente
        if (nodo_Y->Acceso == null)
        {
            nodo_Y->Acceso = nuevo; // Si la columna está vacía, asignamos el nuevo nodo como el primer acceso
        }
        else
        {
            // Si ya hay nodos en la columna, buscamos el lugar adecuado para insertar el nuevo nodo
            NodoInterno<int>* tmp2 = nodo_Y->Acceso;
            while (tmp2 != null)
            {
                // Si la fila del nuevo nodo es menor que la fila del nodo actual, insertamos el nuevo nodo antes
                if (nuevo->coordenadaX < tmp2->coordenadaX)
                {
                    nuevo->Abajo = tmp2;
                    nuevo->Arriba = tmp2->Arriba;
                    tmp2->Arriba->Abajo = nuevo;
                    tmp2->Arriba = nuevo;
                    break;
                }
                else if (nuevo->coordenadaX == tmp2->coordenadaX && nuevo->coordenadaY == tmp2->coordenadaY)
                {
                    // Verificar que no haya nodos duplicados (con las mismas coordenadas)
                    break;
                }
                else
                {
                    // Si no hemos encontrado el lugar, seguimos buscando
                    if (tmp2->Abajo == null)
                    {
                        tmp2->Abajo = nuevo; // Insertamos el nodo al final de la columna
                        nuevo->Arriba = tmp2;
                        break;
                    }
                    else
                    {
                        tmp2 = tmp2->Abajo; // Avanzamos al siguiente nodo en la columna
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
            Console.Write(y_columna->Id + "\t"); // Imprimir el encabezado de cada columna
            y_columna = y_columna->Siguiente;
        }

        Console.WriteLine(); // Salto de línea después de las cabeceras de las columnas

        // Imprimir los nodos de cada fila
        NodoEncabezado<int>* x_fila = filas.primero;
        while (x_fila != null)
        {
            // Imprimir el encabezado de la fila
            Console.Write(x_fila->Id + "\t");

            // Imprimir los valores de la fila
            NodoInterno<int>* interno = x_fila->Acceso;
            NodoEncabezado<int>* y_columna_iter = columnas.primero;

            // Imprimir los valores de las columnas de la fila
            while (y_columna_iter != null)
            {
                if (interno != null && interno->coordenadaY == y_columna_iter->Id)
                {
                    Console.Write(interno->Nombre + "\t"); // Si el nodo interno existe, mostrar su nombre
                    interno = interno->Derecha; // Mover al siguiente nodo en la fila
                }
                else
                {
                    Console.Write(
                        "0\t"); // Si no hay nodo, mostrar 0 (representa la ausencia de un valor en esa posición)
                }

                y_columna_iter = y_columna_iter->Siguiente; // Avanzar a la siguiente columna
            }

            Console.WriteLine(); // Salto de línea después de imprimir una fila
            x_fila = x_fila->Siguiente; // Avanzar a la siguiente fila
        }
    }

    // Destructor para liberar la memoria de los nodos internos y encabezados
    ~MatrizDispersa()
    {
        // Liberar memoria de los nodos internos y encabezados de filas
        NodoEncabezado<int>* x_fila = filas.primero;
        while (x_fila != null)
        {
            // Liberar los nodos internos de la fila
            NodoInterno<int>* interno = x_fila->Acceso;
            while (interno != null)
            {
                NodoInterno<int>* tmp = interno;
                interno = interno->Derecha;
                if (tmp != null)
                {
                    Marshal.FreeHGlobal((IntPtr)tmp);
                }
            }

            // Liberar el encabezado de fila
            NodoEncabezado<int>* tmp_fila = x_fila;
            x_fila = x_fila->Siguiente;
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
            NodoInterno<int>* interno = x_columna->Acceso;
            while (interno != null)
            {
                NodoInterno<int>* tmp = interno;
                interno = interno->Abajo;
                if (tmp != null)
                {
                    Marshal.FreeHGlobal((IntPtr)tmp);
                }
            }

            // Liberar el encabezado de columna
            NodoEncabezado<int>* tmp_columna = x_columna;
            x_columna = x_columna->Siguiente;
            if (tmp_columna != null)
            {
                Marshal.FreeHGlobal((IntPtr)tmp_columna);
            }
        }
    }
}