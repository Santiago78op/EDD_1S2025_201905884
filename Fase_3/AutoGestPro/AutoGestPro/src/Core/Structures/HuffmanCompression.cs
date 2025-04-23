using System.Collections;
using System.Text;
using AutoGestPro.Core.Nodes;

namespace AutoGestPro.Core.Structures;

/// <summary>
/// Implementación del algoritmo de compresión de Huffman
/// </summary>
public class HuffmanCompression
{
    /// <summary>
    /// Comprime una cadena de texto utilizando el algoritmo de Huffman
    /// </summary>
    /// <param name="data">Datos a comprimir</param>
    /// <returns>Tupla con los datos comprimidos y la representación del árbol de Huffman</returns>
    public (byte[] compressedData, string huffmanTree) Compress(string data)
    {
        if (string.IsNullOrEmpty(data))
            return (new byte[0], string.Empty);

        // Calcular frecuencias de cada carácter
        Dictionary<char, int> frequencies = CalculateFrequencies(data);

        // Construir el árbol de Huffman
        HuffmanNode root = BuildHuffmanTree(frequencies);

        // Generar códigos Huffman para cada carácter
        Dictionary<char, string> huffmanCodes = new Dictionary<char, string>();
        GenerateHuffmanCodes(root, "", huffmanCodes);

        // Serializar el árbol de Huffman para almacenarlo
        string serializedTree = SerializeHuffmanTree(root);

        // Calcular el tamaño total en bits que ocuparán los datos comprimidos
        int totalBits = 0;
        foreach (char c in data)
        {
            totalBits += huffmanCodes[c].Length;
        }

        // Calcular el número de bytes necesarios (redondeando hacia arriba)
        int totalBytes = (totalBits + 7) / 8;

        // Inicializar el arreglo de bytes para los datos comprimidos
        byte[] compressedBytes = new byte[totalBytes];

        // Comprimir los datos
        int bitPosition = 0;
        for (int i = 0; i < data.Length; i++)
        {
            string code = huffmanCodes[data[i]];
            for (int j = 0; j < code.Length; j++)
            {
                // Calcular en qué byte y en qué bit del byte estamos
                int byteIndex = bitPosition / 8;
                int bitIndex = 7 - (bitPosition % 8); // Invertimos el índice para MSB primero

                // Asegurarnos de que no nos pasamos del tamaño del arreglo
                if (byteIndex >= compressedBytes.Length)
                    break;

                // Establecer el bit correspondiente
                if (code[j] == '1')
                {
                    compressedBytes[byteIndex] |= (byte)(1 << bitIndex);
                }

                bitPosition++;
            }
        }

        return (compressedBytes, serializedTree);
    }

    /// <summary>
    /// Descomprime datos previamente comprimidos con el algoritmo de Huffman
    /// </summary>
    /// <param name="compressedData">Datos comprimidos</param>
    /// <param name="serializedTree">Representación del árbol de Huffman</param>
    /// <param name="originalLength">Longitud de los datos originales</param>
    /// <returns>Datos descomprimidos</returns>
    public string Decompress(byte[] compressedData, string serializedTree, int originalLength)
    {
        if (compressedData == null || compressedData.Length == 0 || string.IsNullOrEmpty(serializedTree))
            return string.Empty;

        try
        {
            // Reconstruir el árbol de Huffman
            HuffmanNode root = DeserializeHuffmanTree(serializedTree);
            if (root == null)
                return string.Empty;

            StringBuilder result = new StringBuilder(originalLength);
            HuffmanNode current = root;

            // Recorrer bit a bit los datos comprimidos
            for (int byteIndex = 0; byteIndex < compressedData.Length && result.Length < originalLength; byteIndex++)
            {
                byte currentByte = compressedData[byteIndex];
                for (int bitIndex = 7; bitIndex >= 0 && result.Length < originalLength; bitIndex--)
                {
                    // Obtener el bit actual
                    bool bit = (currentByte & (1 << bitIndex)) != 0;

                    // Navegar por el árbol según el bit
                    current = bit ? current.Right : current.Left;

                    // Si llegamos a una hoja, agregar el carácter al resultado y volver a la raíz
                    if (current != null && current.IsLeaf)
                    {
                        result.Append(current.Symbol);
                        current = root;
                    }
                    else if (current == null)
                    {
                        // Si llegamos a un nodo nulo, algo está mal con el árbol
                        current = root;
                    }
                }
            }

            return result.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en la descompresión: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Calcula la frecuencia de cada carácter en los datos
    /// </summary>
    private Dictionary<char, int> CalculateFrequencies(string data)
    {
        Dictionary<char, int> frequencies = new Dictionary<char, int>();

        foreach (char c in data)
        {
            if (frequencies.ContainsKey(c))
                frequencies[c]++;
            else
                frequencies[c] = 1;
        }

        return frequencies;
    }

    /// <summary>
    /// Construye el árbol de Huffman basado en las frecuencias
    /// </summary>
    private HuffmanNode BuildHuffmanTree(Dictionary<char, int> frequencies)
    {
        // Utilizar una cola de prioridad (simulada con lista ordenada)
        var priorityQueue = new List<HuffmanNode>();

        // Inicializar con nodos hoja
        foreach (var pair in frequencies)
        {
            priorityQueue.Add(new HuffmanNode(pair.Key, pair.Value));
        }

        // Construir el árbol
        while (priorityQueue.Count > 1)
        {
            // Ordenar por frecuencia
            priorityQueue.Sort((a, b) => a.Frequency.CompareTo(b.Frequency));

            // Tomar los dos nodos con menor frecuencia
            HuffmanNode left = priorityQueue[0];
            priorityQueue.RemoveAt(0);

            HuffmanNode right = priorityQueue[0];
            priorityQueue.RemoveAt(0);

            // Crear un nuevo nodo interno
            int combinedFreq = left.Frequency + right.Frequency;
            HuffmanNode parent = new HuffmanNode(combinedFreq, left, right);

            // Agregar el nuevo nodo a la cola
            priorityQueue.Add(parent);
        }

        // El único nodo que queda es la raíz
        return priorityQueue[0];
    }

    /// <summary>
    /// Genera los códigos Huffman para cada carácter
    /// </summary>
    private void GenerateHuffmanCodes(HuffmanNode node, string code, Dictionary<char, string> codes)
    {
        if (node == null)
            return;

        // Si es un nodo hoja, guardar el código
        if (node.IsLeaf)
        {
            codes[node.Symbol] = code.Length > 0 ? code : "0"; // Caso especial: un solo carácter
            return;
        }

        // Recorrer subárboles
        GenerateHuffmanCodes(node.Left, code + "0", codes);
        GenerateHuffmanCodes(node.Right, code + "1", codes);
    }

    /// <summary>
    /// Serializa el árbol de Huffman para almacenarlo
    /// </summary>
    private string SerializeHuffmanTree(HuffmanNode root)
    {
        StringBuilder sb = new StringBuilder();
        SerializeNode(root, sb);
        return sb.ToString();
    }

    private void SerializeNode(HuffmanNode node, StringBuilder sb)
    {
        // Formato simple de serialización:
        // 'L' para nodo hoja seguido del carácter
        // 'I' para nodo interno

        if (node.IsLeaf)
        {
            sb.Append('L');
            sb.Append(node.Symbol);
        }
        else
        {
            sb.Append('I');
            SerializeNode(node.Left, sb);
            SerializeNode(node.Right, sb);
        }
    }

    /// <summary>
    /// Deserializa el árbol de Huffman
    /// </summary>
    private HuffmanNode DeserializeHuffmanTree(string serialized)
    {
        int index = 0;
        return DeserializeNode(serialized, ref index);
    }

    private HuffmanNode DeserializeNode(string serialized, ref int index)
    {
        if (index >= serialized.Length)
            return null;

        char nodeType = serialized[index++];

        if (nodeType == 'L')
        {
            char symbol = serialized[index++];
            return new HuffmanNode(symbol, 0); // La frecuencia no importa para descomprimir
        }
        else // nodeType == 'I'
        {
            HuffmanNode left = DeserializeNode(serialized, ref index);
            HuffmanNode right = DeserializeNode(serialized, ref index);
            return new HuffmanNode(0, left, right);
        }
    }
}