using AutoGestPro.Core.Interfaces;

namespace AutoGestPro.Core.Nodes;
/*
 * Defincion del nodo de un arbol B, de orden K
 */
public class NodeTreeB
{
    // Puntero al hijo izquierdo
    public NodeTreeB[] Children { get; set; }
    // Puntero al padre
    public NodeTreeB Parent { get; set; }
    // Llave del nodo
    public int[] Keys { get; set; }
    // Valor del nodo
    public object[] Values { get; set; }
    // Cantidad de llaves
    public int Count { get; set; }
    // Indica si el nodo es hoja
    public bool IsLeaf { get; set; }
    // Altura del nodo
    public int Height { get; set; }
    
    /**
     * Constructor de la clase
     * @param order Orden del arbol
     */
    public NodeTreeB(int order)
    {
        Children = new NodeTreeB[order + 1];
        Keys = new int[order];
        Values = new object[order];
        Count = 0;
        IsLeaf = true;
        Height = 1;
    }
}