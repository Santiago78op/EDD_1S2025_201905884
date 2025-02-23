namespace AutoGestPro.Core.Nodes;

// Estructura genérica NodoEncabezado para manejar enlaces en una lista o matriz
public unsafe class NodoEncabezado<T> where T : unmanaged
{
    public int Id { get; set; }
    public NodoEncabezado<T>* Siguiente { get; set; }
    public NodoEncabezado<T>* Anterior { get; set; }
    public NodoInterno<T>* Acceso { get; set; } // APUNTADOR A NODOS INTERNOS

    public NodoEncabezado(int id)
    {
        Id = id;
        Siguiente = null;
        Anterior = null;
        Acceso = null;
    }
}