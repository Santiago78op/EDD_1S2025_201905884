using AutoGestPro.Core.Interfaces;

namespace AutoGestPro.Core.Nodes;
/*
 * Defincion del nodo de un arbol B, de orden K
 */
public class NodeTreeB
{
    // Keys -> Arreglo de llaves
    public int[] Keys { get; set; }
    // Values -> Arreglo de valores
    public object[] Values { get; set; }
    // Children -> Arreglo de hijos
    public NodeTreeB[] Children { get; set; }
    // Count -> Cantidad de llaves
    public int Count { get; set; }
    // IsLeaf -> Indica si es una hoja
    public bool IsLeaf { get; set; }
    // Height -> Altura del nodo
    public int Height { get; set; }
    
    /*+
     * Constructor de la clase
     * @param order -> Orden del arbol
     */
    public NodeTreeB(int order, bool isLeaf)
    {
        Keys = new int[order - 1];
        Values = new object[order - 1];
        Children = new NodeTreeB[order];
        Count = 0;
        IsLeaf = isLeaf;
        Height = 1;
    }
}