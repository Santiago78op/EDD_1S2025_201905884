namespace AutoGestPro.Core.Nodes;

/// <summary>
/// Clase que representa un nodo en el árbol de Huffman.
/// </summary>
public class HuffmanNode
{
    public char Symbol { get; set; }
    public int Frequency { get; set; }
    public HuffmanNode Left { get; set; }
    public HuffmanNode Right { get; set; }
    public bool IsLeaf => Left == null && Right == null;
            
    public HuffmanNode(char symbol, int frequency)
    {
        Symbol = symbol;
        Frequency = frequency;
        Left = null;
        Right = null;
    }
            
    public HuffmanNode(int frequency, HuffmanNode left, HuffmanNode right)
    {
        Symbol = '\0'; // Nodo interno, no tiene símbolo
        Frequency = frequency;
        Left = left;
        Right = right;
    }
}