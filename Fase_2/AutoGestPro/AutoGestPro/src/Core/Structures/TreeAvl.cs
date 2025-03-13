using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol AVL
 */
public class TreeAvl : ITreeAvl, IDisposable
{
    // Nodo raiz del arbol
    public NodeTreeAvl Root { get; set; }

    /**
     * Constructor de la clase
     */
    public TreeAvl()
    {
        Root = null;
    }
    
    // Get de la altura del Arbol AVL
    /**
     * Get de la altura del Arbol AVL
     * @return Altura del arbol
     */
    public int GetHeight()
    {
        return Height(Root);
    }

    // Obtiene la altura de un nodo, si el nodo es nulo devuelve 0
    /**
     * Obtiene la altura de un nodo, si el nodo es nulo devuelve 0
     * @param node Nodo a obtener la altura
     * @return Altura del nodo
     */
    private int Height(NodeTreeAvl node)
    {
        return node?.Height ?? 0;
    }

    // Calcula el factor de balance de un nodo
    /**
     * Calcula el factor de balance de un nodo
     * @param node Nodo a calcular el factor de balance
     * @return Factor de balance
     */
    private int GetBalance(NodeTreeAvl node)
    {
        return node == null ? 0 : Height(node.Left) - Height(node.Right);
    }

    // Actualiza la altura de un nodo, la altura es la maxima entre la altura de los hijos + 1
    /**
     * Actualiza la altura de un nodo, la altura es la maxima entre la altura de los hijos + 1
     * @param node Nodo a actualizar la altura
     * @return void
     */
    private void UpdateHeight(NodeTreeAvl node)
    {
        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    // Rotacion a la derecha
    /**
     * Rotacion a la derecha
     * @param node Nodo a rotar
     * @return Nodo rotado
     */
    private NodeTreeAvl RightRotate(NodeTreeAvl node)
    {
        NodeTreeAvl left = node.Left; // Nodo izquierdo
        NodeTreeAvl right = left.Right; // Hijo derecho del nodo izquierdo

        /*
         * Realiza la rotacion
         * El nodo izquierdo se convierte en la raiz
         * El nodo se convierte en el hijo derecho del nodo izquierdo
         * El hijo derecho del nodo izquierdo se convierte en el hijo izquierdo del nodo
         */
        left.Right = node;
        node.Left = right;

        // * Se actualiza la altura de los nodos
        UpdateHeight(node);
        UpdateHeight(left);

        // * Se retorna el nodo rotado
        return left;
    }

    // Rotacion a la izquierda
    /**
     * Rotacion a la izquierda
     * @param node Nodo a rotar
     * @return Nodo rotado
     */
    private NodeTreeAvl LeftRotate(NodeTreeAvl node)
    {
        NodeTreeAvl right = node.Right; // Nodo derecho
        NodeTreeAvl left = right.Left; // Hijo izquierdo del nodo derecho

        /*
         * Realiza la rotacion
         * El nodo derecho se convierte en la raiz
         * El nodo se convierte en el hijo izquierdo del nodo derecho
         * El hijo izquierdo del nodo derecho se convierte en el hijo derecho del nodo
         */
        right.Left = node;
        node.Right = left;

        // * Se actualiza la altura de los nodos
        UpdateHeight(node);
        UpdateHeight(right);

        // * Se retorna el nodo rotado
        return right;
    }

    // Metodo para insertar un nodo en el arbol
    /**
     * Metodo para insertar un nodo en el arbol
     * @param key Llave del nodo
     * @param value Valor del nodo
     * @return void
     */
    public void Insert(int key, object value)
    {
        /*
         * Se inserta el nodo en el arbol
         * La raiz del arbol se actualiza con el nodo retornado
         * La funcion Insert retorna el nodo actualizado
         */
        Root = Insert(Root, key, value);
    }

    // Metodo recursivo para insertar un nodo en el arbol
    /**
     * Metodo recursivo para insertar un nodo en el arbol
     * Key: Llave del nodo; debe ser unica
     * @param node Nodo actual
     * @param key Llave del nodo
     * @param value Valor del nodo
     * @return Nodo actualizado
     */
    private NodeTreeAvl Insert(NodeTreeAvl node, int key, object value)
    {
        // Si el nodo es nulo, se crea un nuevo nodo
        if (node == null)
        {
            return new NodeTreeAvl(key, value);
        }

        // Si la llave es menor que la llave del nodo actual, se inserta en el subarbol izquierdo
        if (key < node.Key)
        {
            node.Left = Insert(node.Left, key, value);
        }
        // Si la llave es mayor que la llave del nodo actual, se inserta en el subarbol derecho
        else if (key > node.Key)
        {
            node.Right = Insert(node.Right, key, value);
        }
        // Si la llave es igual que la llave del nodo actual, no se inserta en el arbol
        else
        {
            return node;
        }

        // Se actualiza la altura del nodo
        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));

        // Se calcula el factor de balance del nodo
        int balance = GetBalance(node);

        // Si el factor de balance es mayor que 1 y el factor de balance del subarbol izquierdo es mayor o igual que 0
        if (balance > 1 && GetBalance(node.Left) >= 0)
        {
            return RightRotate(node);
        }

        // Si el factor de balance es mayor que 1 y el factor de balance del subarbol izquierdo es menor que 0
        if (balance > 1 && GetBalance(node.Left) < 0)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        // Si el factor de balance es menor que -1 y el factor de balance del subarbol derecho es menor o igual que 0
        if (balance < -1 && GetBalance(node.Right) <= 0)
        {
            return LeftRotate(node);
        }

        // Si el factor de balance es menor que -1 y el factor de balance del subarbol derecho es mayor que 0
        if (balance < -1 && GetBalance(node.Right) > 0)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        // Se retorna el nodo actualizado
        return node;
    }

    // Metodo para eliminar un nodo del arbol
    /**
     * Metodo para eliminar un nodo del arbol
     * @param key Llave del nodo a eliminar
     * @return void
     */
    public void Remove(int key)
    {
        // Se elimina el nodo del arbol
        Root = Remove(Root, key);
    }

    // Metodo para obtener el nodo con el valor minimo
    /**
     * Metodo para obtener el nodo con el valor minimo
     * @param node Nodo actual
     * @return Nodo con el valor minimo
     */
    private NodeTreeAvl MinValueNode(NodeTreeAvl node)
    {
        NodeTreeAvl current = node; // Nodo actual

        // Se recorre el arbol hasta encontrar el nodo con el valor minimo
        while (current.Left != null)
        {
            current = current.Left;
        }

        // Se retorna el nodo con el valor minimo
        return current;
    }

    // Metodo recursivo para eliminar un nodo del arbol
    /**
     * Metodo recursivo para eliminar un nodo del arbol
     * @param node Nodo actual
     * @param key Llave del nodo a eliminar
     * @return Nodo actualizado
     */
    private NodeTreeAvl Remove(NodeTreeAvl node, int key)
    {
        // Si el nodo es nulo, se retorna nulo
        if (node == null)
        {
            return null;
        }

        // Si la llave es menor que la llave del nodo actual, se elimina en el subarbol izquierdo
        if (key < node.Key)
        {
            node.Left = Remove(node.Left, key);
        }
        // Si la llave es mayor que la llave del nodo actual, se elimina en el subarbol derecho
        else if (key > node.Key)
        {
            node.Right = Remove(node.Right, key);
        }
        // Si la llave es igual que la llave del nodo actual, se elimina el nodo actual
        else
        {
            // Si el nodo tiene un solo hijo o no tiene hijos
            if (node.Left == null || node.Right == null)
            {
                node = node.Left ?? node.Right;
            }
            // Si el nodo tiene dos hijos
            else
            {
                NodeTreeAvl temp = MinValueNode(node.Right); // Nodo con el valor minimo
                node.Key = temp.Key; // Se actualiza la llave del nodo
                node.Value = temp.Value; // Se actualiza el valor del nodo
                node.Right = Remove(node.Right, temp.Key); // Se elimina el nodo con el valor minimo
            }
        }

        // Si el nodo es nulo, se retorna nulo
        if (node == null)
        {
            return null;
        }

        // Se actualiza la altura del nodo
        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));

        // Se calcula el factor de balance del nodo
        int balance = GetBalance(node);

        // Si el factor de balance es mayor que 1
        if (balance > 1)
        {
            if (GetBalance(node.Left) < 0)
            {
                node.Left = LeftRotate(node.Left);
            }

            return RightRotate(node);
        }

        // Si el factor de balance es menor que -1
        if (balance < -1)
        {
            if (GetBalance(node.Right) > 0)
            {
                node.Right = RightRotate(node.Right);
            }

            return LeftRotate(node);
        }

        // Se retorna el nodo actualizado
        return node;
    }

    // Metodo para buscar un nodo en el arbol
    /**
     * Metodo para buscar un nodo en el arbol
     * @param key Llave del nodo a buscar
     * @return Valor del nodo
     */
    public object Search(int key)
    {
        // Se busca el nodo en el arbol
        return Search(Root, key);
    }

    // Metodo recursivo para buscar un nodo en el arbol
    /**
     * Metodo recursivo para buscar un nodo en el arbol
     * @param node Nodo actual
     * @param key Llave del nodo a buscar
     * @return Valor del nodo
     */
    private object Search(NodeTreeAvl node, int key)
    {
        // Se recorre el arbol hasta encontrar el nodo con la llave
        while (node != null)
        {
            // Si la llave es menor que la llave del nodo actual, se busca en el subarbol izquierdo
            if (key < node.Key)
            {
                node = node.Left;
            }
            // Si la llave es mayor que la llave del nodo actual, se busca en el subarbol derecho
            else if (key > node.Key)
            {
                node = node.Right;
            }
            // Si la llave es igual que la llave del nodo actual, se retorna el valor del nodo
            else
            {
                return node.Value;
            }
        }

        // Si no se encuentra el nodo, se retorna nulo
        return null;
    }
    
    /*
     * Dispose
     */
    public void Dispose()
    {
        Root = null;
    }

    /*
     * Destructor
     */
    ~TreeAvl()
    {
        Dispose();
    }
    
    // Metodo para recorrer el arbol en PreOrden
    /**
     * Metodo para recorrer el arbol en PreOrden
     * @param action Accion a realizar
     * @return void
     */
    public void PreOrder(Action<object> action)
    {
        // Se recorre el arbol en PreOrden
        PreOrder(Root, action);
    }
    
    // Metodo recursivo para recorrer el arbol en PreOrden
    /**
     * Metodo recursivo para recorrer el arbol en PreOrden
     * @param node Nodo actual
     * @param action Accion a realizar
     * @return void
     */
    private void PreOrder(NodeTreeAvl node, Action<object> action)
    {
        if (node != null)
        {
            action(node.Value);
            PreOrder(node.Left, action);
            PreOrder(node.Right, action);
        }
    }

    // Metodo para recorrer el arbol en InOrden
    /**
     * Metodo para recorrer el arbol en InOrden
     * @param action Accion a realizar
     * @return void
     */
    public void InOrder(Action<object> action)
    {
        // Se recorre el arbol en InOrden
        InOrder(Root, action);
    }
    
    // Metodo recursivo para recorrer el arbol en InOrden
    /**
     * Metodo recursivo para recorrer el arbol en InOrden
     * @param node Nodo actual
     * @param action Accion a realizar
     * @return void
     */
    private void InOrder(NodeTreeAvl node, Action<object> action)
    {
        if (node != null)
        {
            InOrder(node.Left, action);
            action(node.Value);
            InOrder(node.Right, action);
        }
    }
    
}