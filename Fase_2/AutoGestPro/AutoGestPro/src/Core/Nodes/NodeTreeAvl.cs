namespace AutoGestPro.Core.Nodes;

/*
 * Nodo de un arbol AVL
 */
public class NodeTreeAvl
{
    public NodeTreeAvl Left { get; set; }
    public NodeTreeAvl Right { get; set; }
    public int Height { get; set; }
    public int Key { get; set; }
    public object Value { get; set; }

    public NodeTreeAvl(int key, object value)
    {
        Key = key;
        Value = value;
        Height = 1;
    }
}