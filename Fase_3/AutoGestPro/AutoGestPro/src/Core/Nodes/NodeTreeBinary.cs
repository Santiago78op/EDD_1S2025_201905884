/// <summary>
/// Representa un nodo en un árbol binario
/// </summary>
public class NodeTreeBinary
{
    /// <summary>
    /// Clave única del nodo
    /// </summary>
    public int Key { get; set; }
    
    /// <summary>
    /// Valor almacenado en el nodo
    /// </summary>
    public object Value { get; set; }
    
    /// <summary>
    /// Referencia al hijo izquierdo
    /// </summary>
    public NodeTreeBinary Left { get; set; }
    
    /// <summary>
    /// Referencia al hijo derecho
    /// </summary>
    public NodeTreeBinary Right { get; set; }
    
    /// <summary>
    /// Altura del nodo en el árbol
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Constructor del nodo
    /// </summary>
    /// <param name="key">Clave única</param>
    /// <param name="value">Valor a almacenar</param>
    /// <exception cref="ArgumentNullException">Se lanza si el valor es nulo</exception>
    public NodeTreeBinary(int key, object value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "El valor del nodo no puede ser nulo");
            
        Key = key;
        Value = value;
        Left = null;
        Right = null;
        Height = 1;
    }
    
    /// <summary>
    /// Obtiene el factor de balance del nodo
    /// </summary>
    /// <returns>Factor de balance</returns>
    public int GetBalanceFactor()
    {
        int leftHeight = Left?.Height ?? 0;
        int rightHeight = Right?.Height ?? 0;
        return leftHeight - rightHeight;
    }
    
    /// <summary>
    /// Actualiza la altura del nodo basado en la altura de sus hijos
    /// </summary>
    public void UpdateHeight()
    {
        int leftHeight = Left?.Height ?? 0;
        int rightHeight = Right?.Height ?? 0;
        Height = 1 + Math.Max(leftHeight, rightHeight);
    }
}