using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol B, de orden k
 */
public class TreeB : ITreeB, IDisposable
{
    // Orden del arbol
    private int _order;

    // Raiz del arbol
    private NodeTreeB _root;

    /**
     * Constructor de la clase
     * @param order Orden del arbol
     */
    public TreeB(int order)
    {
        _order = order;
        _root = new NodeTreeB(order);
    }

    // Get de la Altura del arbol
    public int Height => _root.Height;

    // Get de la cantidad de llaves
    public int Count => _root.Count;

    // Actualiza la altura del arbol
    private void UpdateHeight(NodeTreeB node)
    {
        if (node == null) return;
        node.Height = 1 + node.Children.Max(x => x?.Height ?? 0);
    }

    // Metodo para insertar un nodo en el arbol
    public void Insert(int key, object value)
    {
        // Si la raiz esta llena, se debe dividir
        if (_root.Count == _order)
        {
            NodeTreeB newRoot = new NodeTreeB(_order);
            newRoot.Children[0] = _root;
            Split(newRoot, 0);
            _root = newRoot;
        }

        // Se inserta la llave en el nodo
        InsertNonFull(_root, key, value);
        // Se actualiza la altura del arbol
        UpdateHeight(_root);
    }

    // Metodo para insertar una llave en un nodo no lleno
    private void InsertNonFull(NodeTreeB node, int key, object value)
    {
        int i = node.Count - 1;

        if (node.IsLeaf)
        {
            // Encuentra la posición donde se debe insertar la nueva llave
            while (i >= 0 && key < node.Keys[i])
            {
                node.Keys[i + 1] = node.Keys[i];
                node.Values[i + 1] = node.Values[i];
                i--;
            }

            node.Keys[i + 1] = key;
            node.Values[i + 1] = value;
            node.Count++;
        }
        else
        {
            // Encuentra el hijo que debe recibir la nueva llave
            while (i >= 0 && key < node.Keys[i])
            {
                i--;
            }

            i++;
            if (node.Children[i].Count == _order)
            {
                Split(node, i);
                if (key > node.Keys[i])
                {
                    i++;
                }
            }

            InsertNonFull(node.Children[i], key, value);
        }
    }

    // Metodo para dividir un nodo
    private void Split(NodeTreeB node, int i)
    {
        NodeTreeB child = node.Children[i];
        NodeTreeB newNode = new NodeTreeB(_order);

        node.Children[i + 1] = newNode;
        newNode.IsLeaf = child.IsLeaf;
        newNode.Count = _order / 2;

        for (int j = 0; j < _order / 2; j++)
        {
            newNode.Keys[j] = child.Keys[j + _order / 2];
            newNode.Values[j] = child.Values[j + _order / 2];
        }

        if (!child.IsLeaf)
        {
            for (int j = 0; j <= _order / 2; j++)
            {
                newNode.Children[j] = child.Children[j + _order / 2];
            }
        }

        child.Count = _order / 2;

        for (int j = node.Count; j >= i + 1; j--)
        {
            node.Children[j + 1] = node.Children[j];
        }

        for (int j = node.Count - 1; j >= i; j--)
        {
            node.Keys[j + 1] = node.Keys[j];
            node.Values[j + 1] = node.Values[j];
        }

        node.Keys[i] = child.Keys[_order / 2 - 1];
        node.Values[i] = child.Values[_order / 2 - 1];
        node.Count++;
    }

    // Metodo para eliminar un nodo del arbol
    public void Remove(int key)
    {
        // Se elimina la llave del arbol
        Remove(_root, key);
        // Si la raiz esta vacia y no es hoja, se elimina
        if (_root.Count == 0 && !_root.IsLeaf)
        {
            _root = _root.Children[0];
        }

        // Se actualiza la altura del arbol
        UpdateHeight(_root);
    }

    // Metodo recursivo para eliminar un nodo del arbol
    /**
     * @param node Nodo actual
     * @param key Llave a eliminar
     */
    private void Remove(NodeTreeB node, int key)
    {
        // Se busca la llave en el nodo
        int idx = FindKey(node, key);

        // Si la llave esta en el nodo
        if (idx < node.Count && node.Keys[idx] == key)
        {
            // Si el nodo es una hoja
            if (node.IsLeaf)
            {
                RemoveFromLeaf(node, idx);
            }
            // Si el nodo no es una hoja
            else
            {
                RemoveFromNonLeaf(node, idx);
            }
        }
        // Si la llave no esta en el nodo
        else
        {
            // Si el nodo es una hoja
            if (node.IsLeaf)
            {
                return; // La clave no está en el árbol
            }

            // Flag indica si la clave está presente en el subárbol que tiene la última clave
            bool flag = (idx == node.Count);

            // Si el hijo donde se espera que esté la clave tiene menos de t claves, lo llenamos
            if (node.Children[idx].Count < _order / 2)
            {
                // Si el hijo anterior tiene al menos t claves, le prestamos una clave
                Fill(node, idx);
            }

            // Si el último hijo ha sido fusionado, debemos seguir con el anterior
            if (flag && idx > node.Count)
            {
                Remove(node.Children[idx - 1], key);
            }
            // Si no, seguimos con el hijo que contiene la clave
            else
            {
                Remove(node.Children[idx], key);
            }
        }
    }

    /**
     * Metodo para buscar la llave en un nodo hoja
     * @param node Nodo hoja
     * @param idx Indice de la llave a eliminar
     */
    private int FindKey(NodeTreeB node, int key)
    {
        int idx = 0;
        // Se busca la posición de la llave
        while (idx < node.Count && node.Keys[idx] < key)
        {
            idx++;
        }

        // Se retorna la posición de la llave
        return idx;
    }

    // Metodo para eliminar de un nodo no hoja
    /**
     * @param node Nodo actual
     * @param idx Indice de la llave a eliminar
     */
    private void RemoveFromLeaf(NodeTreeB node, int idx)
    {
        // Se eliminan las llaves y valores
        for (int i = idx + 1; i < node.Count; i++)
        {
            node.Keys[i - 1] = node.Keys[i]; // Se mueven las llaves
            node.Values[i - 1] = node.Values[i]; // Se mueven los valores
        }

        // Se disminuye la cantidad de llaves
        node.Count--;
    }

    /**
     * Metodo para eliminar de un nodo no hoja
     * @param node Nodo actual
     * @param idx Indice de la llave a eliminar
     */
    private void RemoveFromNonLeaf(NodeTreeB node, int idx)
    {
        // Se obtiene la llave y el hijo
        int key = node.Keys[idx];

        // Si el hijo izquierdo tiene al menos t llaves
        if (node.Children[idx].Count >= _order / 2)
        {
            NodeTreeB pred = GetPredecessor(node, idx);
            node.Keys[idx] = pred.Keys[pred.Count - 1];
            node.Values[idx] = pred.Values[pred.Count - 1];
            Remove(node.Children[idx], pred.Keys[pred.Count - 1]);
        }
        // Si el hijo derecho tiene al menos t llaves
        else if (node.Children[idx + 1].Count >= _order / 2)
        {
            NodeTreeB succ = GetSuccessor(node, idx);
            node.Keys[idx] = succ.Keys[0];
            node.Values[idx] = succ.Values[0];
            Remove(node.Children[idx + 1], succ.Keys[0]);
        }
        // Si ambos hijos tienen menos de t llaves, se fusionan
        else
        {
            Merge(node, idx);
            Remove(node.Children[idx], key);
        }
    }
    
    /**
     * Metodo para obtener el predecesor de un nodo
     * @param node Nodo actual
     * @param idx Indice de la llave
     */
    private NodeTreeB GetPredecessor(NodeTreeB node, int idx)
    {
        NodeTreeB cur = node.Children[idx];
        // Se busca el nodo más a la derecha
        while (!cur.IsLeaf)
        {
            cur = cur.Children[cur.Count];
        }

        return cur;
    }
    
    /**
     * Metodo para obtener el sucesor de un nodo
     * @param node Nodo actual
     * @param idx Indice de la llave
     */
    private NodeTreeB GetSuccessor(NodeTreeB node, int idx)
    {
        NodeTreeB cur = node.Children[idx + 1];
        // Se busca el nodo más a la izquierda
        while (!cur.IsLeaf)
        {
            cur = cur.Children[0];
        }

        return cur;
    }
    
    /**
     * Metodo para llenar un nodo
     * @param node Nodo actual
     * @param idx Indice de la llave
     */
    private void Fill(NodeTreeB node, int idx)
    {
        // Si el hijo anterior tiene al menos t llaves
        if (idx != 0 && node.Children[idx - 1].Count >= _order / 2)
        {
            // Se presta una llave
            BorrowFromPrev(node, idx);
        }
        // Si el siguiente hijo tiene al menos t llaves
        else if (idx != node.Count && node.Children[idx + 1].Count >= _order / 2)
        {
            // Se presta una llave
            BorrowFromNext(node, idx);
        }
        // Si ambos hijos tienen menos de t llaves, se fusionan
        else
        {
            // Se fusionan los nodos
            if (idx != node.Count)
            {
                // Se fusiona con el siguiente hijo
                Merge(node, idx);
            }
            // Si es el último hijo
            else
            {
                // Se fusiona con el hijo anterior
                Merge(node, idx - 1);
            }
        }
    }
    
    /**
     * Metodo para prestar una llave del hijo anterior
     * @param node Nodo actual
     * @param idx Indice de la llave
     */
    private void BorrowFromPrev(NodeTreeB node, int idx)
    {
        NodeTreeB child = node.Children[idx];
        NodeTreeB sibling = node.Children[idx - 1];

        // Se mueven las llaves y valores
        for (int i = child.Count - 1; i >= 0; i--)
        {
            child.Keys[i + 1] = child.Keys[i];
            child.Values[i + 1] = child.Values[i];
        }
        
        // Si no es hoja, se mueven los hijos
        if (!child.IsLeaf)
        {
            // Se mueven los hijos
            for (int i = child.Count; i >= 0; i--)
            {
                child.Children[i + 1] = child.Children[i];
            }
        }

        // Se mueven las llaves y valores
        child.Keys[0] = node.Keys[idx - 1];
        child.Values[0] = node.Values[idx - 1];

        // Si no es hoja, se mueven los hijos
        if (!child.IsLeaf)
        {
            child.Children[0] = sibling.Children[sibling.Count];
        }
        
        // Se mueven las llaves y valores
        node.Keys[idx - 1] = sibling.Keys[sibling.Count - 1];
        node.Values[idx - 1] = sibling.Values[sibling.Count - 1];
        
        // Se disminuye la cantidad de llaves de los nodos
        child.Count++;
        sibling.Count--;
    }

    private void BorrowFromNext(NodeTreeB node, int idx)
    {
        NodeTreeB child = node.Children[idx];
        NodeTreeB sibling = node.Children[idx + 1];

        // Se mueven las llaves y valores
        child.Keys[child.Count] = node.Keys[idx];
        child.Values[child.Count] = node.Values[idx];

        // Si no es hoja, se mueven los hijos
        if (!child.IsLeaf)
        {
            child.Children[child.Count + 1] = sibling.Children[0];
        }

        // Se mueven las llaves y valores
        node.Keys[idx] = sibling.Keys[0];
        node.Values[idx] = sibling.Values[0];

        // Se mueven las llaves y valores
        for (int i = 1; i < sibling.Count; i++)
        {
            sibling.Keys[i - 1] = sibling.Keys[i];
            sibling.Values[i - 1] = sibling.Values[i];
        }
        
        // Si no es hoja, se mueven los hijos
        if (!sibling.IsLeaf)
        {
            // Se mueven los hijos
            for (int i = 1; i <= sibling.Count; i++)
            {
                sibling.Children[i - 1] = sibling.Children[i];
            }
        }
        
        // Se disminuye la cantidad de llaves de los nodos
        child.Count++;
        sibling.Count--;
    }
    
    /**
     * Metodo para fusionar dos nodos
     * @param node Nodo actual
     * @param idx Indice de la llave
     */
    private void Merge(NodeTreeB node, int idx)
    {
        NodeTreeB child = node.Children[idx];
        NodeTreeB sibling = node.Children[idx + 1];

        child.Keys[_order / 2 - 1] = node.Keys[idx];
        child.Values[_order / 2 - 1] = node.Values[idx];

        // Se copian las llaves y valores
        for (int i = 0; i < sibling.Count; i++)
        {
            child.Keys[i + _order / 2] = sibling.Keys[i];
            child.Values[i + _order / 2] = sibling.Values[i];
        }
        
        // Si no es hoja, se copian los hijos
        if (!child.IsLeaf)
        {
            // Se copian los hijos
            for (int i = 0; i <= sibling.Count; i++)
            {
                child.Children[i + _order / 2] = sibling.Children[i];
            }
        }
        
        // Se mueven las llaves y valores
        for (int i = idx + 1; i < node.Count; i++)
        {
            node.Keys[i - 1] = node.Keys[i];
            node.Values[i - 1] = node.Values[i];
        }
        
        // Si no es hoja, se mueven los hijos
        for (int i = idx + 2; i <= node.Count; i++)
        {
            node.Children[i - 1] = node.Children[i];
        }

        child.Count += sibling.Count + 1;
        node.Count--;
    }
    
    // Metodo para buscar una llave en el arbol
    public object Search(int key)
    {
        return Search(_root, key);
    }
    
    // Metodo recursivo para buscar una llave en un nodo
    private object Search(NodeTreeB node, int key)
    {
        int i = 0;
        // Encuentra la primera llave mayor o igual a la clave
        while (i < node.Count && key > node.Keys[i])
        {
            i++;
        }
    
        // Si la clave es igual a la llave en el nodo, retorna el valor
        if (i < node.Count && node.Keys[i] == key)
        {
            return node.Values[i];
        }
    
        // Si el nodo es hoja, la clave no está en el árbol
        if (node.IsLeaf)
        {
            return null;
        }
    
        // Busca en el hijo correspondiente
        return Search(node.Children[i], key);
    }
    
    public void Dispose()
    {
        _root = null;
    }
    
    ~TreeB()
    {
        Dispose();
    }
}