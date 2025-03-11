using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol AVL
 */
public class TreeAvl
{
    public NodeTreeAvl Root { get; set; }

    public TreeAvl()
    {
        Root = null;
    }

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

}