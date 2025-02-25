namespace AutoGestPro.Core.Nodes;

// Definición de una estructura genérica NodoInterno
// Esta estructura usa el tipo de dato T, que debe ser un tipo no administrado (unmanaged)
public unsafe struct NodoInterno<T> where T : unmanaged
{
    public T id;               // Identificador de tipo T. Al ser genérico, puede ser cualquier tipo no administrado.
    public string nombre;       // Nombre del nodo, un valor de tipo string.
    public int coordenadaX;     
    public int coordenadaY;     
        
    // Punteros a otros nodos que forman la estructura de la matriz
    public NodoInterno<T>* arriba;      // Puntero al nodo superior 
    public NodoInterno<T>* abajo;       // Puntero al nodo inferior 
    public NodoInterno<T>* derecha;     // Puntero al nodo siguiente
    public NodoInterno<T>* izquierda;   // Puntero al nodo anterior 
    
    // Constructor de la estructura NodoInterno
    public NodoInterno(T id, string nombre, int x, int y)
    {
        this.id = id;           // Asigna el ID proporcionado al nodo
        this.nombre = nombre;   // Asigna el nombre proporcionado al nodo
        this.coordenadaX = x;   // Asigna la coordenada X proporcionada al nodo
        this.coordenadaY = y;   // Asigna la coordenada Y proporcionada al nodo
        this.arriba = null;     // Inicializa el puntero arriba a null
        this.abajo = null;      // Inicializa el puntero abajo a null
        this.derecha = null;    // Inicializa el puntero derecha a null
        this.izquierda = null;  // Inicializa el puntero izquierda a null
    }
}