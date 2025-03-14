namespace AutoGestPro.Core.Nodes;

/*
 * Nodo de un arbol binario
 */
public class NodeTreeBinary
{
    // Llave del nodo
    public int Key { get; set; }
    // Valor del nodo
    public object Value { get; set; }
    // Puntero al hijo izquierdo
    public NodeTreeBinary Left { get; set; }
    // Puntero al hijo derecho
    public NodeTreeBinary Right { get; set; }
    // Altura del nodo
    public int Height { get; set; }

    /**
     * Constructor de la clase
     * @param key Llave del nodo
     * @param value Valor del nodo
     */
    public NodeTreeBinary(int key, object value)
    {
        Key = key;
        Value = value;
        Left = null;
        Right = null;
        Height = 1;
    }
}