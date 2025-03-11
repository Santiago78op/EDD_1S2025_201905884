namespace AutoGestPro.Core.Nodes;

/*
 * Nodo de un arbol binario
 */
public class NodeTreeBinary
{
    public NodeTreeBinary Left { get; set; }
    public NodeTreeBinary Right { get; set; }
    public int Key { get; set; }
    public object Value { get; set; }

    public NodeTreeBinary(int key, object value)
    {
        Key = key;
        Value = value;
    }
}