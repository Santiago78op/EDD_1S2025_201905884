namespace AutoGestPro.Core.Structures;

/// <summary>
/// Interface que define las operaciones de un árbol de Merkle.
/// </summary>
public interface IMerkleTree
{
    /// <summary>
    /// Inserta un nodo en el árbol.
    /// </summary>
    /// <param name="id">El identificador único del nodo.</param>
    /// <param name="value">El valor asociado al nodo.</param>
    void Insert(int id, object value);

    /// <summary>
    /// Elimina un nodo del árbol.
    /// </summary>
    /// <param name="id">El identificador único del nodo a eliminar.</param>
    void Remove(int id);

    /// <summary>
    /// Busca un nodo en el árbol.
    /// </summary>
    /// <param name="id">El identificador único del nodo a buscar.</param>
    /// <returns>El valor asociado al nodo, si se encuentra.</returns>
    object Search(int id);

    /// <summary>
    /// Modifica un nodo en el árbol.
    /// </summary>
    /// <param name="id">El identificador único del nodo a modificar.</param>
    /// <param name="value">El nuevo valor a asignar al nodo.</param>
    /// <returns>True si la modificación fue exitosa; de lo contrario, false.</returns>
    bool Modify(int id, object value);

    /// <summary>
    /// Obtiene el hash raíz del árbol.
    /// </summary>
    /// <returns>El hash raíz como una cadena.</returns>
    string GetRootHash();

    /// <summary>
    /// Verifica la integridad de los datos de un nodo.
    /// </summary>
    /// <param name="id">El identificador único del nodo.</param>
    /// <param name="value">El valor esperado del nodo.</param>
    /// <returns>True si los datos son válidos; de lo contrario, false.</returns>
    bool VerifyData(int id, object value);
}