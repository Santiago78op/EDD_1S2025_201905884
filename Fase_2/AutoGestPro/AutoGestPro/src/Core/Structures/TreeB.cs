using System.Collections.Generic;
using System.Text;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol B, de orden k
 */
public class TreeB : ITreeB
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

    // Gett de la raiz
    public NodeTreeB Root
    {
        get => _root;
    }

    /**
     * Metodo para insertar un valor en el arbol
     * @param value -> Valor a insertar
     * @return void
     * @complexity O(log n)
     */
    public void Insert(int key, object value)
    {
        NodeTreeB root = _root;
        if (root.IsFull())
        {
            NodeTreeB newRoot = new NodeTreeB();
            newRoot.Children[0] = root;
            Split(newRoot, 0, root);
            _root = newRoot;
            InsertNonFull(newRoot, key, value);
        }
        InsertNonFull(root, key, value);
    }
    
    /**
     * Metodo para insertar un valor en el arbol
     * @param node -> Nodo a insertar
     * @param key -> Valor a insertar
     * @param value -> Valor a insertar
     * @return void
     * @complexity O(log n)
     */
    private void InsertNonFull(NodeTreeB node, int key, object value)
    {
        int i = node.Count - 1;
        if (node.IsLeaf)
        {
            while (i >= 0 && key < (int)node.Values[i])
            {
                node.Values[i + 1] = node.Values[i];
                i--;
            }
            node.Values[i + 1] = key;
            node.Count++;
        }
        else
        {
            while (i >= 0 && key < (int)node.Values[i])
            {
                i--;
            }
            i++;
            if (node.Children[i].Count == MAX_KEYS)
            {
                Split(node, i, node.Children[i]);
                if (key > (int)node.Values[i])
                {
                    i++;
                }
            }
            InsertNonFull(node.Children[i], key, value);
        }
    }
    
    /**
     * Metodo para dividir un nodo
     * @param parent -> Nodo padre
     * @param index -> Indice del nodo
     * @param node -> Nodo a dividir
     * @return void
     * @complexity O(log n)
     */
    private void Split(NodeTreeB parent, int index, NodeTreeB node)
    {
        NodeTreeB newNode = new NodeTreeB();
        newNode.IsLeaf = node.IsLeaf;
        newNode.Count = MIN_KEYS;
        for (int i = 0; i < MIN_KEYS; i++)
        {
            newNode.Values[i] = node.Values[i + MIN_KEYS + 1];
        }
        if (!node.IsLeaf)
        {
            for (int i = 0; i < MIN_KEYS + 1; i++)
            {
                newNode.Children[i] = node.Children[i + MIN_KEYS + 1];
            }
        }
        for (int i = parent.Count; i > index; i--)
        {
            parent.Children[i + 1] = parent.Children[i];
        }
        parent.Children[index + 1] = newNode;
        for (int i = parent.Count - 1; i >= index; i--)
        {
            parent.Values[i + 1] = parent.Values[i];
        }
        parent.Values[index] = node.Values[MIN_KEYS];
        node.Count = MIN_KEYS;
        parent.Count++;
    }
    
    /**
     * Metodo para buscar un valor en el arbol
     * @param key -> Valor a buscar
     * @return object
     * @complexity O(log n)
     */
    public object Search(int key)
    {
        return Search(_root, key);
    }
    
    /**
     * Metodo para buscar un valor en el arbol
     * @param node -> Nodo a buscar
     * @param key -> Valor a buscar
     * @return object
     * @complexity O(log n)
     */
    private object Search(NodeTreeB node, int key)
    {
        int i = 0;
        while (i < node.Count && key > (int)node.Values[i])
        {
            i++;
        }
        if (i < node.Count && key == (int)node.Values[i])
        {
            return node.Values[i];
        }
        if (node.IsLeaf)
        {
            return null;
        }
        return Search(node.Children[i], key);
    }
    
    /**
     * Metodo para eliminar un valor en el arbol
     * @param key -> Valor a eliminar
     * @return void
     * @complexity O(log n)
     */
    public void Delete(int key)
    {
        Delete(_root, key);
        if (_root.Count == 0)
        {
            if (!_root.IsLeaf)
            {
                _root = _root.Children[0];
            }
            else
            {
                _root = null;
            }
        }
    }
    
    /**
     * Metodo para eliminar un valor en el arbol
     * @param node -> Nodo a eliminar
     * @param key -> Valor a eliminar
     * @return void
     * @complexity O(log n)
     */
    private void Delete(NodeTreeB node, int key)
    {
        int idx = FindKey(node, key);
    
        if (idx < node.Count && (int)node.Values[idx] == key)
        {
            if (node.IsLeaf)
            {
                RemoveFromLeaf(node, idx);
            }
            else
            {
                RemoveFromNonLeaf(node, idx);
            }
        }
        else
        {
            if (node.IsLeaf)
            {
                return;
            }
    
            bool flag = (idx == node.Count);
    
            if (node.Children[idx].Count < MIN_KEYS + 1)
            {
                Fill(node, idx);
            }
    
            if (flag && idx > node.Count)
            {
                Delete(node.Children[idx - 1], key);
            }
            else
            {
                Delete(node.Children[idx], key);
            }
        }
    }
    
    private int FindKey(NodeTreeB node, int key)
    {
        int idx = 0;
        while (idx < node.Count && (int)node.Values[idx] < key)
        {
            idx++;
        }
        return idx;
    }
    
    private void RemoveFromLeaf(NodeTreeB node, int idx)
    {
        for (int i = idx + 1; i < node.Count; ++i)
        {
            node.Values[i - 1] = node.Values[i];
        }
        node.Count--;
    }
    
    private void RemoveFromNonLeaf(NodeTreeB node, int idx)
    {
        int key = (int)node.Values[idx];
    
        if (node.Children[idx].Count >= MIN_KEYS + 1)
        {
            int pred = GetPredecessor(node, idx);
            node.Values[idx] = pred;
            Delete(node.Children[idx], pred);
        }
        else if (node.Children[idx + 1].Count >= MIN_KEYS + 1)
        {
            int succ = GetSuccessor(node, idx);
            node.Values[idx] = succ;
            Delete(node.Children[idx + 1], succ);
        }
        else
        {
            Merge(node, idx);
            Delete(node.Children[idx], key);
        }
    }
    
    private int GetPredecessor(NodeTreeB node, int idx)
    {
        NodeTreeB cur = node.Children[idx];
        while (!cur.IsLeaf)
        {
            cur = cur.Children[cur.Count];
        }
        return (int)cur.Values[cur.Count - 1];
    }
    
    private int GetSuccessor(NodeTreeB node, int idx)
    {
        NodeTreeB cur = node.Children[idx + 1];
        while (!cur.IsLeaf)
        {
            cur = cur.Children[0];
        }
        return (int)cur.Values[0];
    }
    
    private void Fill(NodeTreeB node, int idx)
    {
        if (idx != 0 && node.Children[idx - 1].Count >= MIN_KEYS + 1)
        {
            BorrowFromPrev(node, idx);
        }
        else if (idx != node.Count && node.Children[idx + 1].Count >= MIN_KEYS + 1)
        {
            BorrowFromNext(node, idx);
        }
        else
        {
            if (idx != node.Count)
            {
                Merge(node, idx);
            }
            else
            {
                Merge(node, idx - 1);
            }
        }
    }
    
    private void BorrowFromPrev(NodeTreeB node, int idx)
    {
        NodeTreeB child = node.Children[idx];
        NodeTreeB sibling = node.Children[idx - 1];
    
        for (int i = child.Count - 1; i >= 0; --i)
        {
            child.Values[i + 1] = child.Values[i];
        }
    
        if (!child.IsLeaf)
        {
            for (int i = child.Count; i >= 0; --i)
            {
                child.Children[i + 1] = child.Children[i];
            }
        }
    
        child.Values[0] = node.Values[idx - 1];
    
        if (!child.IsLeaf)
        {
            child.Children[0] = sibling.Children[sibling.Count];
        }
    
        node.Values[idx - 1] = sibling.Values[sibling.Count - 1];
    
        child.Count += 1;
        sibling.Count -= 1;
    }
    
    private void BorrowFromNext(NodeTreeB node, int idx)
    {
        NodeTreeB child = node.Children[idx];
        NodeTreeB sibling = node.Children[idx + 1];
    
        child.Values[child.Count] = node.Values[idx];
    
        if (!child.IsLeaf)
        {
            child.Children[child.Count + 1] = sibling.Children[0];
        }
    
        node.Values[idx] = sibling.Values[0];
    
        for (int i = 1; i < sibling.Count; ++i)
        {
            sibling.Values[i - 1] = sibling.Values[i];
        }
    
        if (!sibling.IsLeaf)
        {
            for (int i = 1; i <= sibling.Count; ++i)
            {
                sibling.Children[i - 1] = sibling.Children[i];
            }
        }
    
        child.Count += 1;
        sibling.Count -= 1;
    }
    
    private void Merge(NodeTreeB node, int idx)
    {
        NodeTreeB child = node.Children[idx];
        NodeTreeB sibling = node.Children[idx + 1];
    
        child.Values[MIN_KEYS] = node.Values[idx];
    
        for (int i = 0; i < sibling.Count; ++i)
        {
            child.Values[i + MIN_KEYS + 1] = sibling.Values[i];
        }
    
        if (!child.IsLeaf)
        {
            for (int i = 0; i <= sibling.Count; ++i)
            {
                child.Children[i + MIN_KEYS + 1] = sibling.Children[i];
            }
        }
    
        for (int i = idx + 1; i < node.Count; ++i)
        {
            node.Values[i - 1] = node.Values[i];
        }
    
        for (int i = idx + 2; i <= node.Count; ++i)
        {
            node.Children[i - 1] = node.Children[i];
        }
    
        child.Count += sibling.Count + 1;
        node.Count--;
    }

    /**
     * Metodo de Ordenamiento Preorden
     */
    public List<object> PreOrder()
    {
        List<object> result = new List<object>();
        PreOrderTraversal(_root, result);
        return result;
    }

    /**
     * Metodo para recorrer el arbol en preorden
     * @param node -> Nodo a recorrer
     * @param result -> Lista de resultados
     * @return void
     * @complexity O(n)
     */
    private void PreOrderTraversal(NodeTreeB node, List<object> result)
    {
        if (node == null)
            return;

        for (int i = 0; i < node.Count; i++)
        {
            result.Add(node.Values[i]);
        }

        if (!node.IsLeaf)
        {
            for (int i = 0; i <= node.Count; i++)
            {
                PreOrderTraversal(node.Children[i], result);
            }
        }
    }

    /**
     * Metodo de Ordenamiento Inorden
     */
    public List<object> InOrder()
    {
        List<object> result = new List<object>();
        InOrderTraversal(_root, result);
        return result;
    }

    /**
     * Metodo para recorrer el arbol en inorden
     * @param node -> Nodo a recorrer
     * @param result -> Lista de resultados
     * @return void
     * @complexity O(n)
     */
    private void InOrderTraversal(NodeTreeB node, List<object> result)
    {
        if (node == null)
            return;

        for (int i = 0; i < node.Count; i++)
        {
            if (!node.IsLeaf)
            {
                InOrderTraversal(node.Children[i], result);
            }
            result.Add(node.Values[i]);
        }

        if (!node.IsLeaf)
        {
            InOrderTraversal(node.Children[node.Count], result);
        }
    }

    /**
     * Metodo de Ordenamiento Postorden
     */
    public List<object> PostOrder()
    {
        List<object> result = new List<object>();
        PostOrderTraversal(_root, result);
        return result;
    }

    /**
     * Metodo para recorrer el arbol en postorden
     * @param node -> Nodo a recorrer
     * @param result -> Lista de resultados
     * @return void
     * @complexity O(n)
     */
    private void PostOrderTraversal(NodeTreeB node, List<object> result)
    {
        if (node == null)
            return;

        if (!node.IsLeaf)
        {
            for (int i = 0; i <= node.Count; i++)
            {
                PostOrderTraversal(node.Children[i], result);
            }
        }

        for (int i = 0; i < node.Count; i++)
        {
            result.Add(node.Values[i]);
        }
    }
}