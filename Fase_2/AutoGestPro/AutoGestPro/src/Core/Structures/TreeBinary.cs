using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol binario
 */
public class TreeBinary
{
    public NodeTreeBinary Root { get; set; }

    public TreeBinary()
    {
        Root = null;
    }

    public void Insert(int key, object value)
    {
        Root = Insert(Root, key, value);
    }

    private NodeTreeBinary Insert(NodeTreeBinary node, int key, object value)
    {
        if (node == null)
        {
            return new NodeTreeBinary(key, value);
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

        return node;
    }
    
}