namespace AutoGestPro.Core.Nodes;

public unsafe class NodoInterno<T> where T : unmanaged
{
    // Propiedad que representa el id del nodo.
    public int Id { get; set; }
    // Propiedad que representa el nombre del nodo.
    public string Nombre { get; set; }
    
    // Coordenadas del nodo.
    public int coordenadaX { get; set; }
    public int coordenadaY { get; set; }

    // Referencias a los nodos vecinos.
    public NodoInterno<T>* Arriba { get; set; }
    public NodoInterno<T>* Abajo { get; set; }
    public NodoInterno<T>* Derecha { get; set; }
    public NodoInterno<T>* Izquierda { get; set; }

    // Constructor que inicializa el nodo con sus coordenadas y el carácter.
    public NodoInterno(int x, int y, int id, string nombre)
    {
        coordenadaX = x;
        coordenadaY = y;
        Id = id;
        Nombre = nombre;

        Arriba = null;
        Abajo = null;
        Derecha = null;
        Izquierda = null;
    }
}