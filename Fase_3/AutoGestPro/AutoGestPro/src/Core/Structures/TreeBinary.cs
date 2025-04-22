using System.Text;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Models;

namespace AutoGestPro.Core.Structures;

/// <summary>
/// Implementación de un árbol binario de búsqueda
/// </summary>
public class TreeBinary : ITreeBinary, IDisposable
{
    private NodeTreeBinary _root;
    private int _count;
    private bool _disposed;

    /// <summary>
    /// Constructor del árbol binario
    /// </summary>
    public TreeBinary()
    {
        _root = null;
        _count = 0;
        _disposed = false;
    }

    /// <summary>
    /// Obtiene la raíz del árbol
    /// </summary>
    public NodeTreeBinary Root
    {
        get
        {
            ThrowIfDisposed();
            return _root;
        }
        private set => _root = value;
    }

    /// <summary>
    /// Inserta un nuevo elemento en el árbol
    /// </summary>
    /// <param name="key">Clave única</param>
    /// <param name="value">Valor a almacenar</param>
    /// <exception cref="ArgumentNullException">Se lanza si el valor es nulo</exception>
    public void Insert(int key, object value)
    {
        ThrowIfDisposed();
        
        if (value == null)
            throw new ArgumentNullException(nameof(value), "El valor a insertar no puede ser nulo");

        _root = InsertRecursive(_root, key, value);
        _count++;
    }

    /// <summary>
    /// Método recursivo para insertar un nodo en el árbol
    /// </summary>
    private NodeTreeBinary InsertRecursive(NodeTreeBinary node, int key, object value)
    {
        // Caso base: si el nodo es nulo, creamos un nuevo nodo
        if (node == null)
            return new NodeTreeBinary(key, value);

        // Si la clave ya existe, actualizamos el valor
        if (key == node.Key)
        {
            /*node.Value = value;
            _count--; */ // Compensamos el incremento que se hará después
            return node;
        }

        // Navegamos hacia el subárbol correspondiente
        if (key < node.Key)
            node.Left = InsertRecursive(node.Left, key, value);
        else
            node.Right = InsertRecursive(node.Right, key, value);

        // Actualizamos la altura del nodo actual
        node.UpdateHeight();
        
        return node;
    }

    /// <summary>
    /// Elimina un elemento del árbol con la clave especificada
    /// </summary>
    /// <param name="key">Clave a eliminar</param>
    public void Delete(int key)
    {
        ThrowIfDisposed();
        
        bool found = Contains(key);
        _root = DeleteRecursive(_root, key);
        
        if (found)
            _count--;
    }

    /// <summary>
    /// Método recursivo para eliminar un nodo del árbol
    /// </summary>
    private NodeTreeBinary DeleteRecursive(NodeTreeBinary node, int key)
    {
        if (node == null)
            return null;

        // Buscamos el nodo a eliminar
        if (key < node.Key)
            node.Left = DeleteRecursive(node.Left, key);
        else if (key > node.Key)
            node.Right = DeleteRecursive(node.Right, key);
        else
        {
            // Caso 1: Nodo es una hoja (sin hijos)
            if (node.Left == null && node.Right == null)
                return null;
                
            // Caso 2: Nodo tiene solo un hijo
            if (node.Left == null)
                return node.Right;
                
            if (node.Right == null)
                return node.Left;
                
            // Caso 3: Nodo tiene dos hijos
            // Encontramos el sucesor inmediato (el más pequeño en el subárbol derecho)
            NodeTreeBinary successor = FindMinNode(node.Right);
            
            // Copiamos los datos del sucesor al nodo actual
            node.Key = successor.Key;
            node.Value = successor.Value;
            
            // Eliminamos el sucesor
            node.Right = DeleteRecursive(node.Right, successor.Key);
        }

        // Actualizamos la altura del nodo actual
        node.UpdateHeight();
        
        return node;
    }

    /// <summary>
    /// Encuentra el nodo con el valor mínimo en un subárbol
    /// </summary>
    private NodeTreeBinary FindMinNode(NodeTreeBinary node)
    {
        NodeTreeBinary current = node;
        
        // El nodo más a la izquierda tiene el valor mínimo
        while (current.Left != null)
            current = current.Left;
            
        return current;
    }

    /// <summary>
    /// Busca un elemento en el árbol por su clave
    /// </summary>
    /// <param name="key">Clave a buscar</param>
    /// <returns>El valor asociado a la clave o null si no se encuentra</returns>
    public object Search(int key)
    {
        ThrowIfDisposed();
        return SearchRecursive(_root, key);
    }

    /// <summary>
    /// Método recursivo para buscar un elemento en el árbol
    /// </summary>
    private object SearchRecursive(NodeTreeBinary node, int key)
    {
        if (node == null)
            return null;
            
        if (key == node.Key)
            return node.Value;
            
        return key < node.Key 
            ? SearchRecursive(node.Left, key) 
            : SearchRecursive(node.Right, key);
    }

    /// <summary>
    /// Verifica si una clave existe en el árbol
    /// </summary>
    /// <param name="key">Clave a verificar</param>
    /// <returns>true si la clave existe, false en caso contrario</returns>
    public bool Contains(int key)
    {
        ThrowIfDisposed();
        return ContainsRecursive(_root, key);
    }

    /// <summary>
    /// Método recursivo para verificar si una clave existe en el árbol
    /// </summary>
    private bool ContainsRecursive(NodeTreeBinary node, int key)
    {
        if (node == null)
            return false;
            
        if (key == node.Key)
            return true;
            
        return key < node.Key 
            ? ContainsRecursive(node.Left, key) 
            : ContainsRecursive(node.Right, key);
    }

    /// <summary>
    /// Obtiene la cantidad de elementos en el árbol
    /// </summary>
    /// <returns>Número de elementos</returns>
    public int Count()
    {
        ThrowIfDisposed();
        return _count;
    }

    /// <summary>
    /// Obtiene la altura del árbol
    /// </summary>
    /// <returns>Altura del árbol</returns>
    public int Height()
    {
        ThrowIfDisposed();
        return _root?.Height ?? 0;
    }

    /// <summary>
    /// Limpia todos los elementos del árbol
    /// </summary>
    public void Clear()
    {
        ThrowIfDisposed();
        _root = null;
        _count = 0;
    }

    /// <summary>
    /// Obtiene los valores del árbol en recorrido inorden (izquierda-raíz-derecha)
    /// </summary>
    /// <returns>Lista de valores en orden</returns>
    public List<object> InOrder()
    {
        ThrowIfDisposed();
        return InOrderRecursive(_root);
    }

    /// <summary>
    /// Método recursivo para el recorrido inorden
    /// </summary>
    private List<object> InOrderRecursive(NodeTreeBinary node)
    {
        var result = new List<object>();
        
        if (node == null)
            return result;
            
        // Primero visitamos el subárbol izquierdo
        result.AddRange(InOrderRecursive(node.Left));
        
        // Luego visitamos la raíz
        result.Add(node.Value);
        
        // Finalmente visitamos el subárbol derecho
        result.AddRange(InOrderRecursive(node.Right));
        
        return result;
    }

    /// <summary>
    /// Obtiene los valores del árbol en recorrido preorden (raíz-izquierda-derecha)
    /// </summary>
    /// <returns>Lista de valores en preorden</returns>
    public List<object> PreOrder()
    {
        ThrowIfDisposed();
        return PreOrderRecursive(_root);
    }

    /// <summary>
    /// Método recursivo para el recorrido preorden
    /// </summary>
    private List<object> PreOrderRecursive(NodeTreeBinary node)
    {
        var result = new List<object>();
        
        if (node == null)
            return result;
            
        // Primero visitamos la raíz
        result.Add(node.Value);
        
        // Luego visitamos el subárbol izquierdo
        result.AddRange(PreOrderRecursive(node.Left));
        
        // Finalmente visitamos el subárbol derecho
        result.AddRange(PreOrderRecursive(node.Right));
        
        return result;
    }

    /// <summary>
    /// Obtiene los valores del árbol en recorrido postorden (izquierda-derecha-raíz)
    /// </summary>
    /// <returns>Lista de valores en postorden</returns>
    public List<object> PostOrder()
    {
        ThrowIfDisposed();
        return PostOrderRecursive(_root);
    }

    /// <summary>
    /// Método recursivo para el recorrido postorden
    /// </summary>
    private List<object> PostOrderRecursive(NodeTreeBinary node)
    {
        var result = new List<object>();
        
        if (node == null)
            return result;
            
        // Primero visitamos el subárbol izquierdo
        result.AddRange(PostOrderRecursive(node.Left));
        
        // Luego visitamos el subárbol derecho
        result.AddRange(PostOrderRecursive(node.Right));
        
        // Finalmente visitamos la raíz
        result.Add(node.Value);
        
        return result;
    }

    /// <summary>
    /// Verifica si el objeto ya ha sido eliminado
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(TreeBinary));
    }

    /// <summary>
    /// Genera una representación del grafo en formato DOT para visualización con Graphviz.
    /// </summary>
    /// <returns>String en formato DOT</returns>
    public string GenerarDotServicios()
    {
        StringBuilder dot = new StringBuilder();
        dot.AppendLine("digraph Servicios {");
        dot.AppendLine("node [shape=ellipse];");
        dot.AppendLine("    subgraph cluster_0 {");
        dot.AppendLine("        label=\"Binary Tree de Servicios\";");

        // Generar el DOT para el árbol
        GenerateDot(_root, dot);

        dot.AppendLine("}");
        return dot.ToString();
    }
    
    /// <summary>
    /// Método recursivo para generar el DOT del árbol
    /// </summary>
    public void GenerateDot(NodeTreeBinary node, StringBuilder dot)
    {
        if (node == null)
            return;

        // Agregar el nodo actual
        dot.AppendLine($"    {node.Key} [label=\"{node.Value}\"];");
        Servicio s = (Servicio)node.Value;
        dot.AppendLine($"{node.Key} [label=\"ID: {s.Id}\\nRepuesto: {s.IdRepuesto} | Vehículo: {s.IdVehiculo}\\n {s.Detalles}\\nCosto: Q{s.Costo} \"]");

        // Agregar la relación con el hijo izquierdo
        if (node.Left != null)
            dot.AppendLine($"    {node.Key} -> {node.Left.Key};");

        // Agregar la relación con el hijo derecho
        if (node.Right != null)
            dot.AppendLine($"    {node.Key} -> {node.Right.Key};");

        // Llamar recursivamente para los hijos
        GenerateDot(node.Left, dot);
        GenerateDot(node.Right, dot);
    }

    /// <summary>
    /// Libera los recursos utilizados por el árbol
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _root = null;
            _count = 0;
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~TreeBinary()
    {
        Dispose();
    }
}