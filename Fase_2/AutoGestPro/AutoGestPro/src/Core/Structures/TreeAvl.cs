using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol AVL
 */
public class TreeAvl : ITreeAvl, IDisposable
{
    /*
     * Raiz del arbol
     * @return NodeTreeAvl
     * @param NodeTreeAvl
     * @param int
     * @param object
     */
    public NodeTreeAvl Root { get; set; }

    /**
     * Constructor de la clase
     */
    public TreeAvl()
    {
        Root = null;
    }

    /**
     * Inserta un nodo en el arbol
     * @param key Llave del nodo
     * @param value Valor del nodo
     * complexity O(log n)
     * @return void
     */
    public void Insert(int key, object value)
    {
        Root = Insert(Root, key, value);
    }
    

    private NodeTreeAvl Insert(NodeTreeAvl node, int key, object value)
    {
        if (node == null)
        {
            return new NodeTreeAvl(key, value);
        }

        if (key < node.Key)
        {
            node.Left = Insert(node.Left, key, value);
        }
        else if (key > node.Key)
        {
            node.Right = Insert(node.Right, key, value);
        }
        else
        {
            node.Value = value;
            return node;
        }

        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));

        int balance = GetBalance(node);

        if (balance > 1 && key < node.Left.Key)
        {
            return RightRotate(node);
        }

        if (balance < -1 && key > node.Right.Key)
        {
            return LeftRotate(node);
        }

        if (balance > 1 && key > node.Left.Key)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        if (balance < -1 && key < node.Right.Key)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }
    

    private NodeTreeAvl MinValueNode(NodeTreeAvl node)
    {
        NodeTreeAvl current = node;

        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }
    
    public void Remove(int key)
    {
        Root = Remove(Root, key);
    }
    
    private NodeTreeAvl Remove(NodeTreeAvl node, int key)
    {
        if (node == null)
        {
            return null;
        }

        if (key < node.Key)
        {
            node.Left = Remove(node.Left, key);
        }
        else if (key > node.Key)
        {
            node.Right = Remove(node.Right, key);
        }
        else
        {
            if (node.Left == null || node.Right == null)
            {
                NodeTreeAvl temp = node.Left ?? node.Right;

                if (temp == null)
                {
                    temp = node;
                    node = null;
                }
                else
                {
                    node = temp;
                }
            }
            else
            {
                NodeTreeAvl temp = MinValueNode(node.Right);

                node.Key = temp.Key;
                node.Value = temp.Value;

                node.Right = Remove(node.Right, temp.Key);
            }
        }

        if (node == null)
        {
            return null;
        }

        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));

        int balance = GetBalance(node);

        if (balance > 1 && GetBalance(node.Left) >= 0)
        {
            return RightRotate(node);
        }

        if (balance > 1 && GetBalance(node.Left) < 0)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        if (balance < -1 && GetBalance(node.Right) <= 0)
        {
            return LeftRotate(node);
        }

        if (balance < -1 && GetBalance(node.Right) > 0)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }
    
    public object Search(int key)
    {
        return Search(Root, key);
    }
    
    private object Search(NodeTreeAvl node, int key)
    {
        if (node == null)
        {
            return null;
        }

        if (key < node.Key)
        {
            return Search(node.Left, key);
        }
        else if (key > node.Key)
        {
            return Search(node.Right, key);
        }
        else
        {
            return node.Value;
        }
    }

    private int Height(NodeTreeAvl node)
    {
        return node == null ? 0 : node.Height;
    }

    private int GetBalance(NodeTreeAvl node)
    {
        return node == null ? 0 : Height(node.Left) - Height(node.Right);
    }

    private NodeTreeAvl RightRotate(NodeTreeAvl node)
    {
        NodeTreeAvl left = node.Left;
        NodeTreeAvl right = left.Right;

        left.Right = node;
        node.Left = right;

        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
        left.Height = 1 + Math.Max(Height(left.Left), Height(left.Right));

        return left;
    }

    private NodeTreeAvl LeftRotate(NodeTreeAvl node)
    {
        NodeTreeAvl right = node.Right;
        NodeTreeAvl left = right.Left;

        right.Left = node;
        node.Right = left;

        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
        right.Height = 1 + Math.Max(Height(right.Left), Height(right.Right));

        return right;
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

}