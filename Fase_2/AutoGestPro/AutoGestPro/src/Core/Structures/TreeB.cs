using System.Text;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol B, de orden k
 */
public class TreeB : ITreeB, IDisposable
{
    // Raiz del arbol
    private NodeTreeB _root;
    // ORDER -> Orden del arbol
    private const int ORDER = 5;
    // MAX_KEYS -> Cantidad maxima de llaves
    private const int MAX_KEYS = ORDER - 1;
    // MIN_KEYS -> Cantidad minima de llaves
    private const int MIN_KEYS = (ORDER / 2) - 1;
    
    /**
     * Constructor de la clase
     */
    public TreeB()
    {
        _root = new NodeTreeB();
    }
    
    /**
     * Metodo para insertar una llave en el arbol
     * @param key -> Llave a insertar
     * @param value -> Valor a insertar
     * @return void
     * @complexity O(log n)
     * @precondition La llave no debe existir en el arbol
     * @postcondition La llave es insertada en el arbol
     */
    public void Insert(int key, object value)
    {
        // Si la raíz está llena, necesitamos dividirla
        if (_root.IsFull())
        {
            NodeTreeB newRoot = new NodeTreeB(); // Creamos un nuevo nodo
            newRoot.IsLeaf = false; // Indicamos que no es hoja
            newRoot.Children[0] = _root; // Asignamos la raíz actual como hijo
            SplitChild(newRoot, 0, _root); // Dividimos la raíz
            _root = newRoot; // Asignamos el nuevo nodo como raíz
        }
        // Insertamos la llave en la raíz
        InsertNonFull(_root, key, value);
    }
    
    /**
     * Metodo InsertNonFull para insertar una llave en un nodo no lleno
     * @param node -> Nodo en el que se insertara la llave
     * @param key -> Llave a insertar
     * @param value -> Valor a insertar
     * @return void
     * @complexity O(log n)
     * @precondition El nodo no debe estar lleno
     * @postcondition La llave es insertada en el nodo
     */
    private void InsertNonFull(NodeTreeB node, int key, object value)
    {
        int i = node.Count - 1; // Inicializamos el contador
        
        // Si el nodo es una hoja
        if (node.IsLeaf)
        {
            // Encuentra la posición donde insertar la nueva llave
            while (i >= 0 && key < node.Keys[i])
            {
                node.Keys[i + 1] = node.Keys[i];
                node.Values[i + 1] = node.Values[i];
                i--;
            }
            
            node.Keys[i + 1] = key; // Inserta la llave
            node.Values[i + 1] = value; // Inserta el valor
            node.Count++; // Incrementa el contador
        }
        else
        {
            // Encuentra el hijo que debe recibir la nueva llave
            while (i >= 0 && key < node.Keys[i])
            {
                /* Si la llave es menor que la llave actual, movemos una posición
                a la derecha la llave y el valor actual*/
                i--;
            }
            // Incrementamos el contador
            i++;
            
            // Verificamos si el hijo está lleno
            if (node.Children[i].IsFull())
            {
                // Dividimos el hijo
                SplitChild(node, i, node.Children[i]);
                // Verificamos si la nueva llave es mayor que la llave actual
                if (key > node.Keys[i])
                {
                    i++; // Incrementamos el contador
                }
            }
            // Insertamos la llave en el hijo
            InsertNonFull(node.Children[i], key, value);
        }
    }
    
    /**
     * Metodo para dividir un hijo de un nodo
     * @param parent -> Nodo padre
     * @param index -> Indice del hijo a dividir
     * @param fullChild -> Hijo a dividir
     * @return void
     * @complexity O(1)
     * @precondition El nodo hijo debe estar lleno
     * @postcondition El nodo hijo es dividido
     */ 
    private void SplitChild(NodeTreeB parent, int index, NodeTreeB fullChild)
    {
        NodeTreeB newChild = new NodeTreeB(); // Creamos un nuevo nodo
        newChild.IsLeaf = fullChild.IsLeaf; // Indicamos si es hoja o no
        newChild.Count = MIN_KEYS; // Asignamos la cantidad de llaves
        
        // Recorremos las llaves y valores del hijo, para asignarlos al nuevo nodo hijo creado.
        for (int j = 0; j < MIN_KEYS; j++)
        {
            newChild.Keys[j] = fullChild.Keys[j + MIN_KEYS + 1]; // Asignamos las llaves
            newChild.Values[j] = fullChild.Values[j + MIN_KEYS + 1]; // Asignamos los valores
        }
        
        // Si el nodo no es una hoja, asignamos los hijos al nuevo nodo hijo creado.
        if (!fullChild.IsLeaf)
        {
            // Recorremos los hijos del nodo, para asignarlos al nuevo nodo hijo creado.
            for (int j = 0; j <= MIN_KEYS; j++)
            {
                newChild.Children[j] = fullChild.Children[j + MIN_KEYS + 1]; // Asignamos los hijos
            }
        }
    
        fullChild.Count = MIN_KEYS; // Asignamos la cantidad de llaves al nodo hijo original
        
        // Recorremos las llaves y valores del nodo padre, para asignarlos al nuevo nodo hijo creado.
        for (int j = parent.Count; j > index; j--)
        {
            parent.Children[j + 1] = parent.Children[j]; // Asignamos los hijos
            parent.Keys[j] = parent.Keys[j - 1]; // Asignamos las llaves
            parent.Values[j] = parent.Values[j - 1]; // Asignamos los valores
        }
    
        parent.Children[index + 1] = newChild; // Asignamos el nuevo nodo hijo creado
        parent.Keys[index] = fullChild.Keys[MIN_KEYS]; // Asignamos la llave
        parent.Values[index] = fullChild.Values[MIN_KEYS]; // Asignamos el valor
        parent.Count++; // Incrementamos el contador
    }
    
    /**
     * Metodo para buscar una llave en el arbol
     * @param key -> Llave a buscar
     * @return object
     * @complexity O(log n)
     * @precondition La llave debe existir en el arbol
     * @postcondition La llave es encontrada en el arbol
     */
    public object Search(int key)
    {
        return Search(_root, key);
    }
    
    /**
     * Metodo para buscar una llave en un nodo
     * @param node -> Nodo en el que se buscara la llave
     * @param key -> Llave a buscar
     * @return object
     * @complexity O(log n)
     * @precondition La llave debe existir en el nodo
     * @postcondition La llave es encontrada en el nodo
     */
    private object Search(NodeTreeB node, int key)
    {
        int i = 0; // Inicializamos el contador
        while (i < node.Count && key > node.Keys[i])
        {
            i++;
        }
    
        if (i < node.Count && key == node.Keys[i])
        {
            return node.Values[i];
        }
    
        if (node.IsLeaf)
        {
            return null; // La llave no se encuentra en el árbol
        }
        else
        {
            return Search(node.Children[i], key);
        }
    }
}