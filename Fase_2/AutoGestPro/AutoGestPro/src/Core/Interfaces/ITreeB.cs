namespace AutoGestPro.Core.Interfaces;

public interface ITreeB
{
    void Insert(int key, object value); // Inserta un nodo en el arbol
    void Remove(int key); // Elimina un node en el arbol
    Object Search(int key); // Busca un nodo en el arbol
}