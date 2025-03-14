using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol binario
 */
public class TreeBinary : ITreeBinary, IDisposable
{
    // Raiz del arbol
    private NodeTreeBinary _root;

    /**
     * Constructor de la clase
     */
    public TreeBinary()
    {
        _root = null;
    }

    // Get de la Altura del arbol
    public int Height => _root?.Height ?? 0;
    
    // Update de la altura del arbol
    private void UpdateHeight(NodeTreeBinary node)
    {
        if (node == null) return;
        node.Height = 1 + Math.Max(node.Left?.Height ?? 0, node.Right?.Height ?? 0);
    }

    // Metodo para insertar un nodo en el arbol
    public void Insert(int key, object value)
    {
        // Se inserta la llave en el nodo
        _root = Insert(_root, key, value);
    }
    
    // Metodo recursivo para insertar un nodo en el arbol
    /**
     * @param node Nodo actual
     * @param key Llave del nodo
     * @param value Valor del nodo
     * @return Nodo actualizado
     */
    private NodeTreeBinary Insert(NodeTreeBinary node, int key, object value)
    {
        // Si el nodo es nulo
        if (node == null)
        {
            return new NodeTreeBinary(key, value);
        }

        // Si la llave es menor que la llave del nodo
        if (key < node.Key)
        {
            node.Left = Insert(node.Left, key, value);
        }
        // Si la llave es mayor que la llave del nodo
        else if (key > node.Key)
        {
            node.Right = Insert(node.Right, key, value);
        }
        // Si la llave es igual a la llave del nodo
        else
        {
            node.Value = value;
        }

        // Se actualiza la altura del nodo
        UpdateHeight(node);

        return node;
    }
    
    // Metodo para eliminar un nodo del arbol
    public void Delete(int key)
    {
        // Se elimina la llave del nodo
        _root = Delete(_root, key);
    }
    
    // Metodo para obtener el nodo con el menor valor
    /**
     * @param node Nodo actual
     * @return Nodo con el menor valor
     */
    private NodeTreeBinary MinValueNode(NodeTreeBinary node)
    {
        NodeTreeBinary current = node;

        // Se recorre el arbol hasta encontrar el nodo con el menor valor
        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }
    
    // Metodo recursivo para eliminar un nodo del arbol
    /**
     * @param node Nodo actual
     * @param key Llave del nodo
     * @return Nodo actualizado
     */
    private NodeTreeBinary Delete(NodeTreeBinary node, int key)
    {
        // Si el nodo es nulo
        if (node == null)
        {
            return null;
        }

        // Si la llave es menor que la llave del nodo
        if (key < node.Key)
        {
            node.Left = Delete(node.Left, key);
        }
        // Si la llave es mayor que la llave del nodo
        else if (key > node.Key)
        {
            node.Right = Delete(node.Right, key);
        }
        // Si la llave es igual a la llave del nodo
        else
        {
            // Si el nodo tiene un solo hijo
            if (node.Left == null || node.Right == null)
            {
                // Se obtiene el hijo del nodo
                NodeTreeBinary temp = node.Left ?? node.Right;

                // Si el nodo no tiene hijos
                if (temp == null)
                {
                    return null;
                }
                else
                {
                    return temp;
                }
            }
            else
            {
                // Se obtiene el nodo sucesor
                NodeTreeBinary temp = MinValueNode(node.Right);

                // Se copian los valores del nodo sucesor al nodo actual
                node.Key = temp.Key;
                node.Value = temp.Value;

                // Se elimina el nodo sucesor
                node.Right = Delete(node.Right, temp.Key);
            }
        }

        // Se actualiza la altura del nodo
        UpdateHeight(node);

        return node;
    }
    
    // Metodo para buscar un nodo en el arbol
    public object Search(int key)
    {
        // Se busca la llave en el nodo
        return Search(_root, key);
    }
    
    // Metodo recursivo para buscar un nodo en el arbol
    /**
     * @param node Nodo actual
     * @param key Llave del nodo
     * @return Valor del nodo
     */
    private object Search(NodeTreeBinary node, int key)
    {
        // Si el nodo es nulo
        if (node == null)
        {
            return null;
        }

        // Si la llave es menor que la llave del nodo
        if (key < node.Key)
        {
            return Search(node.Left, key);
        }
        // Si la llave es mayor que la llave del nodo
        else if (key > node.Key)
        {
            return Search(node.Right, key);
        }
        // Si la llave es igual a la llave del nodo
        else
        {
            return node.Value;
        }
    }
    
    // Metodo para eliminar el arbol
    public void Dispose()
    {
        // Se elimina el arbol
        _root = null;
    }
    
    /**
     * Destructor de la clase
     */
    ~TreeBinary()
    {
        Dispose();
    }
    
    // Metodo para recorrer el arbol en orden
    public List<object> InOrder()
    {
        // Se recorre el arbol en orden
        return InOrder(_root);
    }
    
    // Metodo recursivo para recorrer el arbol en orden
    /**
     * @param node Nodo actual
     * @return Lista de valores
     */
    private List<object> InOrder(NodeTreeBinary node)
    {
        List<object> list = new();

        // Si el nodo es nulo
        if (node == null)
        {
            return list;
        }

        // Se recorre el arbol en orden
        list.AddRange(InOrder(node.Left));
        list.Add(node.Value);
        list.AddRange(InOrder(node.Right));

        return list;
    }
    
    // Metodo para recorrer el arbol en preorden
    public List<object> PreOrder()
    {
        // Se recorre el arbol en preorden
        return PreOrder(_root);
    }
    
    // Metodo recursivo para recorrer el arbol en preorden
    /**
     * @param node Nodo actual
     * @return Lista de valores
     */
    private List<object> PreOrder(NodeTreeBinary node)
    {
        List<object> list = new();

        // Si el nodo es nulo
        if (node == null)
        {
            return list;
        }

        // Se recorre el arbol en preorden
        list.Add(node.Value);
        list.AddRange(PreOrder(node.Left));
        list.AddRange(PreOrder(node.Right));

        return list;
    }
    
    // Metodo para recorrer el arbol en postorden
    public List<object> PostOrder()
    {
        // Se recorre el arbol en postorden
        return PostOrder(_root);
    }
    
    // Metodo recursivo para recorrer el arbol en postorden
    /**
     * @param node Nodo actual
     * @return Lista de valores
     */
    private List<object> PostOrder(NodeTreeBinary node)
    {
        List<object> list = new();

        // Si el nodo es nulo
        if (node == null)
        {
            return list;
        }

        // Se recorre el arbol en postorden
        list.AddRange(PostOrder(node.Left));
        list.AddRange(PostOrder(node.Right));
        list.Add(node.Value);

        return list;
    }
}