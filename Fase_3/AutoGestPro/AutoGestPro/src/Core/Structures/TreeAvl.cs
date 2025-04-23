using System.Text;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/// <summary>
/// Implementación genérica optimizada de un Árbol AVL.
/// </summary>
/// <typeparam name="TValue">Tipo de valor almacenado en el árbol</typeparam>
public class TreeAvl<TValue> : ITreeAvl<TValue>
{
    /// <summary>
    /// Nodo raíz del árbol.
    /// </summary>
    private NodeTreeAvl<TValue> _root;

    /// <summary>
    /// Indica si los recursos han sido liberados.
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// Contador de elementos en el árbol.
    /// </summary>
    private int _count;
    
    /// <summary>
    /// Get de la propiedad Root.
    /// </summary>
    public NodeTreeAvl<TValue> Root => _root;
    
    /// <summary>
    /// Obtiene el número de elementos en el árbol.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// Obtiene la altura del árbol.
    /// </summary>
    public int Height => GetHeight(_root);

    /// <summary>
    /// Inicializa una nueva instancia de TreeAvl.
    /// </summary>
    public TreeAvl()
    {
        _root = null;
        _count = 0;
    }

    /// <summary>
    /// Inserta un nuevo elemento en el árbol.
    /// </summary>
    /// <param name="key">Clave única del elemento</param>
    /// <param name="value">Valor a almacenar</param>
    /// <exception cref="ArgumentNullException">Se lanza si el valor es nulo</exception>
    public void Insert(int key, TValue value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "El valor no puede ser nulo");
        }

        _root = InsertNode(_root, key, value);
        _count++;
    }

    /// <summary>
    /// Elimina un elemento del árbol.
    /// </summary>
    /// <param name="key">Clave del elemento a eliminar</param>
    /// <returns>True si se eliminó correctamente, False si no se encontró la clave</returns>
    public bool Remove(int key)
    {
        int initialCount = _count;
        _root = RemoveNode(_root, key);
        return initialCount != _count;
    }

    /// <summary>
    /// Busca un elemento en el árbol por su clave.
    /// </summary>
    /// <param name="key">Clave a buscar</param>
    /// <returns>El valor asociado a la clave o default(TValue) si no se encuentra</returns>
    public TValue Search(int key)
    {
        NodeTreeAvl<TValue> node = SearchNode(_root, key);
        return node != null ? node.Value : default;
    }

    /// <summary>
    /// Verifica si una clave existe en el árbol.
    /// </summary>
    /// <param name="key">Clave a verificar</param>
    /// <returns>True si la clave existe, False en caso contrario</returns>
    public bool ContainsKey(int key)
    {
        return SearchNode(_root, key) != null;
    }

    /// <summary>
    /// Modifica el valor asociado a una clave.
    /// </summary>
    /// <param name="key">Clave del elemento a modificar</param>
    /// <param name="value">Nuevo valor</param>
    /// <returns>True si se modificó correctamente, False si no se encontró la clave</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el valor es nulo</exception>
    public bool Modify(int key, TValue value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "El valor no puede ser nulo");
        }

        NodeTreeAvl<TValue> node = SearchNode(_root, key);
        if (node == null)
        {
            return false;
        }

        node.Value = value;
        return true;
    }

    /// <summary>
    /// Recorre el árbol en orden (izquierda, raíz, derecha).
    /// </summary>
    /// <param name="action">Acción a ejecutar con cada valor</param>
    public void InOrder(Action<TValue> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), "La acción no puede ser nula");
        }

        InOrderTraversal(_root, action);
    }

    /// <summary>
    /// Recorre el árbol en pre-orden (raíz, izquierda, derecha).
    /// </summary>
    /// <param name="action">Acción a ejecutar con cada valor</param>
    public void PreOrder(Action<TValue> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), "La acción no puede ser nula");
        }

        PreOrderTraversal(_root, action);
    }

    /// <summary>
    /// Recorre el árbol en post-orden (izquierda, derecha, raíz).
    /// </summary>
    /// <param name="action">Acción a ejecutar con cada valor</param>
    public void PostOrder(Action<TValue> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), "La acción no puede ser nula");
        }

        PostOrderTraversal(_root, action);
    }

    /// <summary>
    /// Limpia todos los elementos del árbol.
    /// </summary>
    public void Clear()
    {
        _root = null;
        _count = 0;
    }

    /// <summary>
    /// Convierte el árbol en una lista ordenada.
    /// </summary>
    /// <returns>Lista con los valores del árbol ordenados por clave</returns>
    public List<TValue> ToList()
    {
        List<TValue> result = new List<TValue>(_count);
        InOrder(value => result.Add(value));
        return result;
    }

    /// <summary>
    /// Obtiene un diccionario con las claves y valores del árbol.
    /// </summary>
    /// <returns>Diccionario con las claves y valores del árbol</returns>
    public Dictionary<int, TValue> ToDictionary()
    {
        Dictionary<int, TValue> result = new Dictionary<int, TValue>(_count);
        TraverseDictionary(_root, result);
        return result;
    }

    #region Private Methods

    /// <summary>
    /// Obtiene la altura de un nodo.
    /// </summary>
    /// <param name="node">Nodo a consultar</param>
    /// <returns>Altura del nodo o 0 si es nulo</returns>
    private int GetHeight(NodeTreeAvl<TValue> node)
    {
        return node?.Height ?? 0;
    }

    /// <summary>
    /// Calcula el factor de balance de un nodo.
    /// </summary>
    /// <param name="node">Nodo a calcular</param>
    /// <returns>Factor de balance</returns>
    private int GetBalanceFactor(NodeTreeAvl<TValue> node)
    {
        return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
    }

    /// <summary>
    /// Actualiza la altura de un nodo.
    /// </summary>
    /// <param name="node">Nodo a actualizar</param>
    private void UpdateHeight(NodeTreeAvl<TValue> node)
    {
        if (node != null)
        {
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }
    }

    /// <summary>
    /// Realiza una rotación a la derecha.
    /// </summary>
    /// <param name="y">Nodo a rotar</param>
    /// <returns>Nuevo nodo raíz después de la rotación</returns>
    private NodeTreeAvl<TValue> RightRotate(NodeTreeAvl<TValue> y)
    {
        NodeTreeAvl<TValue> x = y.Left;
        NodeTreeAvl<TValue> T2 = x.Right;

        // Realizar rotación
        x.Right = y;
        y.Left = T2;

        // Actualizar alturas
        UpdateHeight(y);
        UpdateHeight(x);

        return x;
    }

    /// <summary>
    /// Realiza una rotación a la izquierda.
    /// </summary>
    /// <param name="x">Nodo a rotar</param>
    /// <returns>Nuevo nodo raíz después de la rotación</returns>
    private NodeTreeAvl<TValue> LeftRotate(NodeTreeAvl<TValue> x)
    {
        NodeTreeAvl<TValue> y = x.Right;
        NodeTreeAvl<TValue> T2 = y.Left;

        // Realizar rotación
        y.Left = x;
        x.Right = T2;

        // Actualizar alturas
        UpdateHeight(x);
        UpdateHeight(y);

        return y;
    }

    /// <summary>
    /// Inserta un nodo en el árbol.
    /// </summary>
    /// <param name="node">Nodo actual</param>
    /// <param name="key">Clave a insertar</param>
    /// <param name="value">Valor a insertar</param>
    /// <returns>Nodo raíz actualizado</returns>
    private NodeTreeAvl<TValue> InsertNode(NodeTreeAvl<TValue> node, int key, TValue value)
    {
        // Realizar inserción estándar BST
        if (node == null)
        {
            return new NodeTreeAvl<TValue>(key, value);
        }

        if (key < node.Key)
        {
            node.Left = InsertNode(node.Left, key, value);
        }
        else if (key > node.Key)
        {
            node.Right = InsertNode(node.Right, key, value);
        }
        else // Clave duplicada, actualizar valor
        {
            /*node.Value = value;
            _count--; // Compensar el incremento posterior*/
            return node;
        }

        // Actualizar altura
        UpdateHeight(node);

        // Obtener factor de balance
        int balance = GetBalanceFactor(node);

        // Casos de rotación

        // Caso Izquierda-Izquierda
        if (balance > 1 && key < node.Left.Key)
        {
            return RightRotate(node);
        }

        // Caso Derecha-Derecha
        if (balance < -1 && key > node.Right.Key)
        {
            return LeftRotate(node);
        }

        // Caso Izquierda-Derecha
        if (balance > 1 && key > node.Left.Key)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        // Caso Derecha-Izquierda
        if (balance < -1 && key < node.Right.Key)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }

    /// <summary>
    /// Encuentra el nodo con el valor mínimo en un subárbol.
    /// </summary>
    /// <param name="node">Raíz del subárbol</param>
    /// <returns>Nodo con el valor mínimo</returns>
    private NodeTreeAvl<TValue> MinValueNode(NodeTreeAvl<TValue> node)
    {
        NodeTreeAvl<TValue> current = node;
        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }

    /// <summary>
    /// Elimina un nodo del árbol.
    /// </summary>
    /// <param name="node">Nodo actual</param>
    /// <param name="key">Clave a eliminar</param>
    /// <returns>Nodo raíz actualizado</returns>
    private NodeTreeAvl<TValue> RemoveNode(NodeTreeAvl<TValue> node, int key)
    {
        if (node == null)
        {
            return null;
        }

        // Búsqueda del nodo a eliminar
        if (key < node.Key)
        {
            node.Left = RemoveNode(node.Left, key);
        }
        else if (key > node.Key)
        {
            node.Right = RemoveNode(node.Right, key);
        }
        else
        {
            // Nodo encontrado, proceder con la eliminación
            _count--; // Decrementar contador

            // Caso 1: Nodo con 0 o 1 hijo
            if (node.Left == null)
            {
                return node.Right;
            }
            else if (node.Right == null)
            {
                return node.Left;
            }

            // Caso 2: Nodo con 2 hijos
            NodeTreeAvl<TValue> temp = MinValueNode(node.Right);
            node.Key = temp.Key;
            node.Value = temp.Value;
            node.Right = RemoveNode(node.Right, temp.Key);
            _count++; // Compensar el decremento adicional
        }

        // Si era un nodo hoja, retornar
        if (node == null)
        {
            return null;
        }

        // Actualizar altura
        UpdateHeight(node);

        // Verificar balance
        int balance = GetBalanceFactor(node);

        // Casos de rotación

        // Caso Izquierda-Izquierda
        if (balance > 1 && GetBalanceFactor(node.Left) >= 0)
        {
            return RightRotate(node);
        }

        // Caso Izquierda-Derecha
        if (balance > 1 && GetBalanceFactor(node.Left) < 0)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        // Caso Derecha-Derecha
        if (balance < -1 && GetBalanceFactor(node.Right) <= 0)
        {
            return LeftRotate(node);
        }

        // Caso Derecha-Izquierda
        if (balance < -1 && GetBalanceFactor(node.Right) > 0)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }

    /// <summary>
    /// Busca un nodo en el árbol.
    /// </summary>
    /// <param name="node">Nodo raíz</param>
    /// <param name="key">Clave a buscar</param>
    /// <returns>Nodo encontrado o null</returns>
    private NodeTreeAvl<TValue> SearchNode(NodeTreeAvl<TValue> node, int key)
    {
        while (node != null)
        {
            if (key < node.Key)
            {
                node = node.Left;
            }
            else if (key > node.Key)
            {
                node = node.Right;
            }
            else
            {
                return node;
            }
        }

        return null;
    }

    /// <summary>
    /// Recorre el árbol en in-orden.
    /// </summary>
    /// <param name="node">Nodo actual</param>
    /// <param name="action">Acción a ejecutar</param>
    private void InOrderTraversal(NodeTreeAvl<TValue> node, Action<TValue> action)
    {
        if (node != null)
        {
            InOrderTraversal(node.Left, action);
            action(node.Value);
            InOrderTraversal(node.Right, action);
        }
    }

    /// <summary>
    /// Recorre el árbol en pre-orden.
    /// </summary>
    /// <param name="node">Nodo actual</param>
    /// <param name="action">Acción a ejecutar</param>
    private void PreOrderTraversal(NodeTreeAvl<TValue> node, Action<TValue> action)
    {
        if (node != null)
        {
            action(node.Value);
            PreOrderTraversal(node.Left, action);
            PreOrderTraversal(node.Right, action);
        }
    }

    /// <summary>
    /// Recorre el árbol en post-orden.
    /// </summary>
    /// <param name="node">Nodo actual</param>
    /// <param name="action">Acción a ejecutar</param>
    private void PostOrderTraversal(NodeTreeAvl<TValue> node, Action<TValue> action)
    {
        if (node != null)
        {
            PostOrderTraversal(node.Left, action);
            PostOrderTraversal(node.Right, action);
            action(node.Value);
        }
    }

    /// <summary>
    /// Recorre el árbol y construye un diccionario.
    /// </summary>
    /// <param name="node">Nodo actual</param>
    /// <param name="dictionary">Diccionario a construir</param>
    private void TraverseDictionary(NodeTreeAvl<TValue> node, Dictionary<int, TValue> dictionary)
    {
        if (node != null)
        {
            TraverseDictionary(node.Left, dictionary);
            dictionary[node.Key] = node.Value;
            TraverseDictionary(node.Right, dictionary);
        }
    }

    /// <summary>
    /// Genera una representación del grafo en formato DOT para visualización con Graphviz.
    /// </summary>
    /// <returns>String en formato DOT</returns>
    public string GenerarDotRepuestos()
    {
        StringBuilder dot = new StringBuilder();
        dot.AppendLine("digraph Repuestos {");
        dot.AppendLine("node [shape=record];");
        dot.AppendLine("    subgraph cluster_0 {");
        dot.AppendLine("        label=\"AVL Tree de Repuestos\";");

        if (_root == null)
        {
            dot.AppendLine("        empty [label=\"Arbol AVL vacío\"];");
        }
        else
        {
            // Ordena el arbol AVL en inorden
           
            // Genera el grafo
            GenerarDotAvl(_root,dot);
        }
        
        dot.AppendLine("    }");
        dot.AppendLine("}");
        return dot.ToString();
    }
    
    /// <summary>
    /// Genera una representación del grafo en formato DOT para visualización con Graphviz.
    /// </summary>
    public void GenerarDotAvl(NodeTreeAvl<TValue> repuesto,StringBuilder dot)
    {
        if (repuesto != null)
        {
            Repuesto r = repuesto.Value as Repuesto;
            dot.AppendLine($"R{r.Id} [label=\"{{<izq> | ID: {r.Id} | Nombre: {r.Repuesto1} | Detalles: {r.Detalles} |Costo: {r.Costo} | <der>}}\"]");
            if (repuesto.Left != null)
            {
                Repuesto rLeft = repuesto.Left.Value as Repuesto;
                dot.AppendLine($"R{r.Id} -> R{rLeft.Id}");
                GenerarDotAvl(repuesto.Left,dot);
            }
            if (repuesto.Right != null)
            {
                Repuesto rRight = repuesto.Right.Value as Repuesto;
                dot.AppendLine($"R{r.Id} -> R{rRight.Id}");
                GenerarDotAvl(repuesto.Right, dot);
            }
        }
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Libera los recursos utilizados por el árbol.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Libera los recursos utilizados por el árbol.
    /// </summary>
    /// <param name="disposing">Indica si se está realizando una liberación explícita</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Clear();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Destructor de la clase.
    /// </summary>
    ~TreeAvl()
    {
        Dispose(false);
    }

    #endregion
}