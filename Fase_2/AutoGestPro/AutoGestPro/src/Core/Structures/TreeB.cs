using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/*
 * Arbol B, de orden 5
 */
public class TreeB
{
    public NodeTreeB Root { get; set; }

    public TreeB()
    {
        Root = null;
    }

    public void Insert(int key, object value)
    {
        Root = Insert(Root, key, value);
    }

    private NodeTreeB Insert(NodeTreeB node, int key, object value)
    {
        if (node == null)
        {
            return new NodeTreeB(key, value);
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