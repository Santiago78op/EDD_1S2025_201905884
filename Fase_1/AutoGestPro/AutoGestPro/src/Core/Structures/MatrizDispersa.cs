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
    
    // Generar Matriz
    public String GeneraDotMatrizDispersa()
    {
        // -- lo primero es settear los valores que nos preocupan
        string grafo = "digraph T{ \nnode[shape=box fontname=\"Arial\" fillcolor=\"white\" style=filled ];";
        grafo += $"\nroot[label = \"capa: {capa}\", group=1];\n";
        grafo += "label = \"MATRIZ DISPERSA\" \nfontname=\"Arial Black\" \nfontsize=\"15pt\" \n\n";

// --- lo siguiente es escribir los nodos encabezados, empezamos con las filas, los nodos tendran el foramto Fn
        var x_fila = filas.primero;
        while (x_fila != null)
        {
            grafo += $"F{x_fila->id}[label=\"F{x_fila->id}\",fillcolor=\"plum\",group=1];\n";
            x_fila = x_fila->siguiente;
        }

// --- apuntamos los nodos F entre ellos
        x_fila = filas.primero;
        while (x_fila != null)
        {
            if (x_fila->siguiente != null)
            {
                grafo += $"F{x_fila->id}->F{x_fila->siguiente->id};\n";
                grafo += $"F{x_fila->siguiente->id}->F{x_fila->id};\n";
            }

            x_fila = x_fila->siguiente;
        }

// --- Luego de los nodos encabezados fila, seguimos con las columnas, los nodos tendran el foramto Cn
        var y_columna = columnas.primero;
        while (y_columna != null)
        {
            int group = y_columna->id + 1;
            grafo += $"C{y_columna->id}[label=\"C{y_columna->id}\",fillcolor=\"powderblue\",group={group.ToString()}];\n";
            y_columna = y_columna->siguiente;
        }

// --- apuntamos los nodos C entre ellos
        int cont = 0;
        y_columna = columnas.primero;
        while (y_columna != null)
        {
            if (y_columna->siguiente != null)
            {
                grafo += $"C{y_columna->id}->C{y_columna->siguiente->id};\n";
                grafo += $"C{y_columna->siguiente->id}->C{y_columna->id};\n";
            }

            cont++;
            y_columna = y_columna->siguiente;
        }

// --- luego que hemos escrito todos los nodos encabezado, apuntamos el nodo root hacua ellos
        y_columna = columnas.primero;
        x_fila = filas.primero;
        grafo += $"root->F{x_fila->id};\n root->C{y_columna->id};\n";
        grafo += "{rank=same;root;";
        cont = 0;
        y_columna = columnas.primero;
        while (y_columna != null)
        {
            grafo += $"C{y_columna->id};";
            cont++;
            y_columna = y_columna->siguiente;
        }

        grafo += "}\n";
        var aux = filas.primero;
        var aux2 = aux->acceso;
        cont = 0;
        while (aux != null)
        {
            cont++;
            while (aux2 != null)
            {
                // if (aux2.caracter == '-')
                //    grafo += $"N{aux2.x}_{aux2.y}[label=\" \",group=\"{int.Parse(aux2.y) + 1}\"];\n";
                // else
                int group = aux2->coordenadaY + 1;
                grafo +=
                    $"N{aux2->coordenadaX}_{aux2->coordenadaY}[label=\"{aux2->nombre}\",group=\"{group}\", fillcolor=\"yellow\"];\n";

                aux2 = aux2->derecha;
            }

            aux = aux->siguiente;
            if (aux != null)
            {
                aux2 = aux->acceso;
            }
        }

        aux = filas.primero;
        aux2 = aux->acceso;
        cont = 0;
        while (aux != null)
        {
            string rank = $"{{rank = same;F{aux->id};";
            cont = 0;
            while (aux2 != null)
            {
                if (cont == 0)
                {
                    grafo += $"F{aux->id}->N{aux2->coordenadaX}_{aux2->coordenadaY};\n";
                    grafo += $"N{aux2->coordenadaX}_{aux2->coordenadaY}->F{aux->id};\n";
                    cont++;
                }

                if (aux2->derecha != null)
                {
                    grafo += $"N{aux2->coordenadaX}_{aux2->coordenadaY}->N{aux2->derecha->coordenadaX}_{aux2->derecha->coordenadaY};\n";
                    grafo += $"N{aux2->derecha->coordenadaX}_{aux2->derecha->coordenadaY}->N{aux2->coordenadaX}_{aux2->coordenadaY};\n";
                }

                rank += $"N{aux2->coordenadaX}_{aux2->coordenadaY};";
                aux2 = aux2->derecha;
            }

            aux = aux->siguiente;
            if (aux != null)
            {
                aux2 = aux->acceso;
            }

            grafo += rank + "}\n";
        }

        aux = columnas.primero;
        aux2 = aux->acceso;
        cont = 0;
        while (aux != null)
        {
            cont = 0;
            grafo += "";
            while (aux2 != null)
            {
                if (cont == 0)
                {
                    grafo += $"C{aux->id}->N{aux2->coordenadaX}_{aux2->coordenadaY};\n";
                    grafo += $"N{aux2->coordenadaX}_{aux2->coordenadaY}->C{aux->id};\n";
                    cont++;
                }

                if (aux2->abajo != null)
                {
                    grafo += $"N{aux2->abajo->coordenadaX}_{aux2->abajo->coordenadaY}->N{aux2->coordenadaX}_{aux2->coordenadaY};\n";
                    grafo += $"N{aux2->coordenadaX}_{aux2->coordenadaY}->N{aux2->abajo->coordenadaX}_{aux2->abajo->coordenadaY};\n";
                }

                aux2 = aux2->abajo;
            }

            aux = aux->siguiente;
            if (aux != null)
            {
                aux2 = aux->acceso;
            }
        }
        return grafo;
    }
}