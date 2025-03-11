namespace AutoGestPro.Core.Nodes;
/*
 * Nodo de un arbol B, de orden 5
 */
public class NodeTreeB
{
    public NodeTreeB Left { get; set; }
    public NodeTreeB Right { get; set; }
    public int Key { get; set; }
    public object Value { get; set; }

    public NodeTreeB(int key, object value)
    {
        Key = key;
        Value = value;
    }
}