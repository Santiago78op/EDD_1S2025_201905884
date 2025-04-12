using System.Security.Cryptography;
using System.Text;

namespace AutoGestPro.Core.Nodes;

/// <summary>
/// Definición del nodo de un Árbol de Merkle
/// </summary>
public class NodeMerkleTree
{
    // Hash del nodo
    public string Hash { get; set; }
    // Hash del hijo izquierdo
    public NodeMerkleTree Left { get; set; }
    // Hash del hijo derecho
    public NodeMerkleTree Right { get; set; }
    // ID del comprobante
    public int ID { get; set; }
    // Datos del comprobante
    public object Value { get; set; }
            
    /// <summary>
    /// Constructor de la clase
    /// </summary>
    /// <param name="id">ID del comprobante</param>
    /// <param name="value">Datos del comprobante</param>
    public NodeMerkleTree(int id, object value)
    {
        ID = id;
        Value = value;
        Left = null;
        Right = null;
        Hash = ComputeHash(id, value);
    }
            
    /// <summary>
    /// Calcula el hash de los datos
    /// </summary>
    /// <param name="id">ID del comprobante</param>
    /// <param name="value">Datos del comprobante</param>
    /// <returns>Hash calculado</returns>
    private string ComputeHash(int id, object value)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string data = id.ToString() + value.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}