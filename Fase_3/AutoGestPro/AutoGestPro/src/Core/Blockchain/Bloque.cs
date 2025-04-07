using System.Security.Cryptography;
using System.Text;

namespace AutoGestPro.Core.Blockchain;

/// <summary>
/// Representa un bloque en la cadena de blockchain
/// </summary>
public class Bloque
{
    private int _indice; // Índice del bloque en la cadena
    private DateTime _timestamp ; // Fecha de creación del bloque
    private string _hash; // Hash del bloque
    private string _hashPrevio ; // Hash del bloque anterior
    private string _datos; // Datos del bloque
    private int _nonce; // Número aleatorio utilizado en el proceso de minería

    public int Indice
    {
        get => _indice;
        set => _indice = value;
    }

    public DateTime Timestamp
    {
        get => _timestamp;
        set => _timestamp = value;
    }

    public string Hash
    {
        get => _hash;
        set => _hash = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string HashPrevio
    {
        get => _hashPrevio;
        set => _hashPrevio = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Datos
    {
        get => _datos;
        set => _datos = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Nonce
    {
        get => _nonce;
        set => _nonce = value;
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase Bloque con los datos proporcionados.
    /// </summary>
    /// <param name="indice">Índice del bloque en la cadena.</param>
    /// <param name="fechaCreacion">Fecha de creación del bloque.</param>
    /// <param name="hashPrevio">Hash del bloque anterior.</param>
    /// <param name="datos">Datos del bloque.</param>
    public Bloque(int indice, DateTime timestamp, string hashPrevio, string datos)
    {
        _indice = indice;
        _timestamp = timestamp;
        _hashPrevio = hashPrevio;
        _datos = datos;
        _nonce = 0;
        _hash = CalcularHash();
    }
    
    // Calcula el hash del bloque (SHA-256)
    public string CalcularHash()
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string str = $"{_indice}{_timestamp.ToString("yyyy-MM-ddTHH:mm:ss")}{_hashPrevio}{_datos}{_nonce}";
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
    
    // Implementación básica de proof of work (minado)
    public void MinarBloque(int dificultad)
    {
        string objetivo = new string('0', dificultad);
        while (!_hash.Substring(0, dificultad).Equals(objetivo))
        {
            _nonce++;
            _hash = CalcularHash();
        }
    }
}