namespace AutoGestPro.Core.Interfaces;

/// <summary>
/// Interfaz para la implementación de un árbol AVL.
/// </summary>
/// <typeparam name="TValue">Tipo de valor almacenado en el árbol</typeparam>
public interface ITreeAvl<TValue> : IDisposable
{
    /// <summary>
    /// Obtiene el número de elementos en el árbol.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Obtiene la altura del árbol.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Inserta un nuevo elemento en el árbol.
    /// </summary>
    /// <param name="key">Clave única del elemento</param>
    /// <param name="value">Valor a almacenar</param>
    void Insert(int key, TValue value);

    /// <summary>
    /// Elimina un elemento del árbol.
    /// </summary>
    /// <param name="key">Clave del elemento a eliminar</param>
    /// <returns>True si se eliminó correctamente, False si no se encontró la clave</returns>
    bool Remove(int key);

    /// <summary>
    /// Busca un elemento en el árbol por su clave.
    /// </summary>
    /// <param name="key">Clave a buscar</param>
    /// <returns>El valor asociado a la clave o default(TValue) si no se encuentra</returns>
    TValue Search(int key);

    /// <summary>
    /// Verifica si una clave existe en el árbol.
    /// </summary>
    /// <param name="key">Clave a verificar</param>
    /// <returns>True si la clave existe, False en caso contrario</returns>
    bool ContainsKey(int key);

    /// <summary>
    /// Modifica el valor asociado a una clave.
    /// </summary>
    /// <param name="key">Clave del elemento a modificar</param>
    /// <param name="value">Nuevo valor</param>
    /// <returns>True si se modificó correctamente, False si no se encontró la clave</returns>
    bool Modify(int key, TValue value);

    /// <summary>
    /// Recorre el árbol en orden (izquierda, raíz, derecha).
    /// </summary>
    /// <param name="action">Acción a ejecutar con cada valor</param>
    void InOrder(Action<TValue> action);

    /// <summary>
    /// Recorre el árbol en pre-orden (raíz, izquierda, derecha).
    /// </summary>
    /// <param name="action">Acción a ejecutar con cada valor</param>
    void PreOrder(Action<TValue> action);

    /// <summary>
    /// Recorre el árbol en post-orden (izquierda, derecha, raíz).
    /// </summary>
    /// <param name="action">Acción a ejecutar con cada valor</param>
    void PostOrder(Action<TValue> action);

    /// <summary>
    /// Limpia todos los elementos del árbol.
    /// </summary>
    void Clear();

    /// <summary>
    /// Convierte el árbol en una lista ordenada.
    /// </summary>
    /// <returns>Lista con los valores del árbol ordenados por clave</returns>
    List<TValue> ToList();

    /// <summary>
    /// Obtiene un diccionario con las claves y valores del árbol.
    /// </summary>
    /// <returns>Diccionario con las claves y valores del árbol</returns>
    Dictionary<int, TValue> ToDictionary();
}