namespace AutoGestPro.Core.Nodes;

/*
 * Defincion del nodo de un arbol AVL
 */
public class NodeTreeAvl
{
    // Puntero al hijo izquierdo
    public NodeTreeAvl Left { get; set; }
    // Puntero al hijo derecho
    public NodeTreeAvl Right { get; set; }
    // Altura del nodo
    public int Height { get; set; }
    // Llave del nodo
    public int Key { get; set; }
    // Valor del nodo
    public object Value { get; set; }
    
    /**
     * Constructor de la clase
     * @param key Llave del nodo
     * @param value Valor del nodo
     */
    public NodeTreeAvl(int key, object value)
    {
        Key = key;
        Value = value;
        Left = null;
        Right = null;
        Height = 1;
    }
}