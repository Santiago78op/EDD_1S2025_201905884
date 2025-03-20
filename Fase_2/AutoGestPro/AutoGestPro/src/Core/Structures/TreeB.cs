using System.Text;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol B, de orden k
 */
public class TreeB : ITreeB, IDisposable
{
    private int _order;
    private NodeTreeB _root;

    public TreeB(int order)
    {
        _order = order;
        _root = new NodeTreeB(order, true);
    }

    public NodeTreeB Root
    {
        get => _root;
        set => _root = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Height => _root.Height;
    public int Count => _root.Count;

    private void UpdateHeight(NodeTreeB node)
    {
        if (node == null) return;
        node.Height = 1 + node.Children.Max(x => x?.Height ?? 0);
    }

    public void Insert(int key, object value)
    {
        if (Search(key) != null)
        {
            Console.WriteLine($"Error: La clave con ID {key} ya existe.");
            return;
        }

        if (_root.Count == _order - 1)
        {
            NodeTreeB newRoot = new NodeTreeB(_order, false);
            newRoot.Children[0] = _root;
            SplitChild(newRoot, 0, _root);
            _root = newRoot;
        }

        InsertNonFull(_root, key, value);
    }

    private void InsertNonFull(NodeTreeB node, int key, object value)
    {
        int i = node.Count - 1;

        if (node.IsLeaf)
        {
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
            while (i >= 0 && key < node.Keys[i])
            {
                i--;
            }

            i++;

            if (node.Children[i].Count == _order - 1)
            {
                SplitChild(node, i, node.Children[i]);
                if (key > node.Keys[i])
                {
                    i++;
                }
            }

            InsertNonFull(node.Children[i], key, value);
        }
    }

    private void SplitChild(NodeTreeB parent, int index, NodeTreeB child)
    {
        NodeTreeB newNode = new NodeTreeB(_order, child.IsLeaf);
        int mid = _order / 2;
    
        // Move the second half of the child's keys and values to the new node
        for (int j = 0; j < mid - 1; j++)
        {
            newNode.Keys[j] = child.Keys[mid + 1 + j];
            newNode.Values[j] = child.Values[mid + 1 + j];
        }
    
        if (!child.IsLeaf)
        {
            for (int j = 0; j < mid; j++)
            {
                newNode.Children[j] = child.Children[mid + 1 + j];
            }
        }
    
        child.Count = mid;
        newNode.Count = mid - 1;
    
        // Move the parent's children to make space for the new node
        for (int j = parent.Count; j >= index + 1; j--)
        {
            parent.Children[j + 1] = parent.Children[j];
        }
        parent.Children[index + 1] = newNode;
    
        // Move the parent's keys and values to make space for the middle key of the child
        for (int j = parent.Count - 1; j >= index; j--)
        {
            parent.Keys[j + 1] = parent.Keys[j];
            parent.Values[j + 1] = parent.Values[j];
        }
        parent.Keys[index] = child.Keys[mid];
        parent.Values[index] = child.Values[mid];
        parent.Count++;
    }

    public void Remove(int key)
    {
        Remove(_root, key);
        if (_root.Count == 0 && !_root.IsLeaf)
        {
            _root = _root.Children[0];
        }

        UpdateHeight(_root);
    }

    private void Remove(NodeTreeB node, int key)
    {
        int idx = FindKey(node, key);

        if (idx < node.Count && node.Keys[idx] == key)
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

            if (node.Children[idx].Count < _order / 2)
            {
                Fill(node, idx);
            }

            if (flag && idx > node.Count)
            {
                Remove(node.Children[idx - 1], key);
            }
            else
            {
                Remove(node.Children[idx], key);
            }
        }
    }

    private int FindKey(NodeTreeB node, int key)
    {
        int idx = 0;
        while (idx < node.Count && node.Keys[idx] < key)
        {
            idx++;
        }

        return idx;
    }

    private void RemoveFromLeaf(NodeTreeB node, int idx)
    {
        for (int i = idx + 1; i < node.Count; i++)
        {
            node.Keys[i - 1] = node.Keys[i];
            node.Values[i - 1] = node.Values[i];
        }

        node.Count--;
    }

    private void RemoveFromNonLeaf(NodeTreeB node, int idx)
    {
        int key = node.Keys[idx];

        if (node.Children[idx].Count >= _order / 2)
        {
            NodeTreeB pred = GetPredecessor(node, idx);
            node.Keys[idx] = pred.Keys[pred.Count - 1];
            node.Values[idx] = pred.Values[pred.Count - 1];
            Remove(node.Children[idx], pred.Keys[pred.Count - 1]);
        }
        else if (node.Children[idx + 1].Count >= _order / 2)
        {
            NodeTreeB succ = GetSuccessor(node, idx);
            node.Keys[idx] = succ.Keys[0];
            node.Values[idx] = succ.Values[0];
            Remove(node.Children[idx + 1], succ.Keys[0]);
        }
        else
        {
            Merge(node, idx);
            Remove(node.Children[idx], key);
        }
    }

    private NodeTreeB GetPredecessor(NodeTreeB node, int idx)
    {
        NodeTreeB cur = node.Children[idx];
        while (!cur.IsLeaf)
        {
            cur = cur.Children[cur.Count];
        }

        return cur;
    }

    private NodeTreeB GetSuccessor(NodeTreeB node, int idx)
    {
        NodeTreeB cur = node.Children[idx + 1];
        while (!cur.IsLeaf)
        {
            cur = cur.Children[0];
        }

        return cur;
    }

    private void Fill(NodeTreeB node, int idx)
    {
        if (idx != 0 && node.Children[idx - 1].Count >= _order / 2)
        {
            BorrowFromPrev(node, idx);
        }
        else if (idx != node.Count && node.Children[idx + 1].Count >= _order / 2)
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

        for (int i = child.Count - 1; i >= 0; i--)
        {
            child.Keys[i + 1] = child.Keys[i];
            child.Values[i + 1] = child.Values[i];
        }

        if (!child.IsLeaf)
        {
            for (int i = child.Count; i >= 0; i--)
            {
                child.Children[i + 1] = child.Children[i];
            }
        }

        child.Keys[0] = node.Keys[idx - 1];
        child.Values[0] = node.Values[idx - 1];

        if (!child.IsLeaf)
        {
            child.Children[0] = sibling.Children[sibling.Count];
        }

        node.Keys[idx - 1] = sibling.Keys[sibling.Count - 1];
        node.Values[idx - 1] = sibling.Values[sibling.Count - 1];

        child.Count++;
        sibling.Count--;
    }

    private void BorrowFromNext(NodeTreeB node, int idx)
    {
        NodeTreeB child = node.Children[idx];
        NodeTreeB sibling = node.Children[idx + 1];

        child.Keys[child.Count] = node.Keys[idx];
        child.Values[child.Count] = node.Values[idx];

        if (!child.IsLeaf)
        {
            child.Children[child.Count + 1] = sibling.Children[0];
        }

        node.Keys[idx] = sibling.Keys[0];
        node.Values[idx] = sibling.Values[0];

        for (int i = 1; i < sibling.Count; i++)
        {
            sibling.Keys[i - 1] = sibling.Keys[i];
            sibling.Values[i - 1] = sibling.Values[i];
        }

        if (!sibling.IsLeaf)
        {
            for (int i = 1; i <= sibling.Count; i++)
            {
                sibling.Children[i - 1] = sibling.Children[i];
            }
        }

        child.Count++;
        sibling.Count--;
    }

    private void Merge(NodeTreeB node, int idx)
    {
        NodeTreeB child = node.Children[idx];
        NodeTreeB sibling = node.Children[idx + 1];

        child.Keys[_order / 2 - 1] = node.Keys[idx];
        child.Values[_order / 2 - 1] = node.Values[idx];

        for (int i = 0; i < sibling.Count; i++)
        {
            child.Keys[i + _order / 2] = sibling.Keys[i];
            child.Values[i + _order / 2] = sibling.Values[i];
        }

        if (!child.IsLeaf)
        {
            for (int i = 0; i <= sibling.Count; i++)
            {
                child.Children[i + _order / 2] = sibling.Children[i];
            }
        }

        for (int i = idx + 1; i < node.Count; i++)
        {
            node.Keys[i - 1] = node.Keys[i];
            node.Values[i - 1] = node.Values[i];
        }

        for (int i = idx + 2; i <= node.Count; i++)
        {
            node.Children[i - 1] = node.Children[i];
        }

        child.Count += sibling.Count + 1;
        node.Count--;
    }

    public object Search(int key)
    {
        return Search(_root, key);
    }

    private object Search(NodeTreeB node, int key)
    {
        int i = 0;
        while (i < node.Count && key > node.Keys[i])
        {
            i++;
        }

        if (i < node.Count && node.Keys[i] == key)
        {
            return node.Values[i];
        }

        if (node.IsLeaf)
        {
            return null;
        }

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

    public List<object> InOrder()
    {
        List<object> list = new();
        InOrder(_root, list);
        return list;
    }

    private void InOrder(NodeTreeB node, List<object> list)
    {
        if (node == null) return;
        for (int i = 0; i < node.Count; i++)
        {
            InOrder(node.Children[i], list);
            list.Add(node.Values[i]);
        }

        InOrder(node.Children[node.Count], list);
    }

    public List<object> PreOrder()
    {
        List<object> list = new();
        PreOrder(_root, list);
        return list;
    }

    private void PreOrder(NodeTreeB node, List<object> list)
    {
        if (node == null) return;
        for (int i = 0; i < node.Count; i++)
        {
            list.Add(node.Values[i]);
            PreOrder(node.Children[i], list);
        }

        PreOrder(node.Children[node.Count], list);
    }

    public List<object> PostOrder()
    {
        List<object> list = new();
        PostOrder(_root, list);
        return list;
    }

    private void PostOrder(NodeTreeB node, List<object> list)
    {
        if (node == null) return;
        for (int i = 0; i < node.Count; i++)
        {
            PostOrder(node.Children[i], list);
        }

        PostOrder(node.Children[node.Count], list);
        for (int i = 0; i < node.Count; i++)
        {
            list.Add(node.Values[i]);
        }
    }

    public string Print()
    {
        StringBuilder sb = new();
        Print(_root, sb, 0);
        return sb.ToString();
    }

    private void Print(NodeTreeB node, StringBuilder sb, int level)
    {
        if (node == null) return;
        for (int i = 0; i < node.Count; i++)
        {
            Print(node.Children[i], sb, level + 1);
            sb.Append($"{new string(' ', level * 4)}{node.Keys[i]}\n");
        }

        Print(node.Children[node.Count], sb, level + 1);
    }
}