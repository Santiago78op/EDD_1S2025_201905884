namespace AutoGestPro.Core.Interfaces;

/// <summary>
/// Interfaz para un árbol binario genérico
/// </summary>
/// <typeparam name="TKey">Tipo de la llave</typeparam>
/// <typeparam name="TValue">Tipo del valor</typeparam>

public interface ITreeBinary
{
    /// <summary>
    /// Inserts a new node into the binary tree.
    /// </summary>
    /// <param name="key">The key of the node to insert.</param>
    /// <param name="value">The value of the node to insert.</param>
    void Insert(int key, object value);
    
    /// <summary>
    /// Deletes a node from the binary tree by its key.
    /// </summary>
    /// <param name="key">The key of the node to delete.</param>
    void Delete(int key);
    
    /// <summary>
    /// Searches for a node in the binary tree by its key.
    /// </summary>
    /// <param name="key">The key of the node to search for.</param>
    /// <returns>The value of the node if found, otherwise null.</returns>
    object Search(int key);
    
    /// <summary>
    /// Checks if a node with the specified key exists in the binary tree.
    /// </summary>
    /// <param name="key">The key to check for.</param>
    /// <returns>True if the key exists, otherwise false.</returns>
    bool Contains(int key);
    
    /// <summary>
    /// Gets the total number of nodes in the binary tree.
    /// </summary>
    /// <returns>The count of nodes.</returns>
    int Count();
    
    /// <summary>
    /// Clears all nodes from the binary tree.
    /// </summary>
    void Clear();
    
    /// <summary>
    /// Performs an in-order traversal of the binary tree.
    /// </summary>
    /// <returns>A list of node values in in-order sequence.</returns>
    List<object> InOrder();
    
    /// <summary>
    /// Performs a pre-order traversal of the binary tree.
    /// </summary>
    /// <returns>A list of node values in pre-order sequence.</returns>
    List<object> PreOrder();
    
    /// <summary>
    /// Performs a post-order traversal of the binary tree.
    /// </summary>
    /// <returns>A list of node values in post-order sequence.</returns>
    List<object> PostOrder();
    
    /// <summary>
    /// Gets the height of the binary tree.
    /// </summary>
    /// <returns>The height of the tree.</returns>
    int Height();
}