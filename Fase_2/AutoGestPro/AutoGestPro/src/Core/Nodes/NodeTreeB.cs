using AutoGestPro.Core.Interfaces;

namespace AutoGestPro.Core.Nodes;
/*
 * Defincion del nodo de un arbol B, de orden K
 */
public class NodeTreeB
{
    // Keys -> Arreglo de llaves
    public int[] Keys { get; set; }
    // ORDER -> Orden del arbol
    private const int ORDER = 5;
    // MAX_KEYS -> Cantidad maxima de llaves
    private const int MAX_KEYS = ORDER - 1;
    // MIN_KEYS -> Cantidad minima de llaves
    private const int MIN_KEYS = (ORDER / 2) - 1;
    
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
    
    /**
     * Constructor de la clase
     */
    public NodeTreeB()
    {
        Keys = new int[MAX_KEYS];
        Values = new object[MAX_KEYS];
        Children = new NodeTreeB[ORDER];
        Count = 0;
        IsLeaf = true;
        Height = 0;
    }
    
    // Valida si el nodo esta lleno
    public bool IsFull()
    {
        return Count == MAX_KEYS;
    }
    
    // Valida si el nodo tiene el minimo de llaves requerido
    public bool IsMin()
    {
        return Count == MIN_KEYS;
    }
}