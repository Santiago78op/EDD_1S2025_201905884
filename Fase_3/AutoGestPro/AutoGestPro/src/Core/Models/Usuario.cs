using System.Security.Cryptography;
using System.Text;

namespace AutoGestPro.Core.Models;

/// <summary>
/// Representa un usuario del sistema (cliente del taller)
/// </summary>
public class Usuario
{
    private int _id; // Identificador único del cliente
    private string _nombres; // Nombres del cliente
    private string _apellidos; // Apellidos del cliente
    private string _correo; // Correo electrónico del cliente
    private int _edad; // Edad del cliente
    
    // La contraseña no se almacena directamente, solo su hash
    private string _contraseniaHash;
    private string _salt;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Nombres
    {
        get => _nombres;
        set => _nombres = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Apellidos
    {
        get => _apellidos;
        set => _apellidos = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Correo
    {
        get => _correo;
        set => _correo = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Edad
    {
        get => _edad;
        set => _edad = value;
    }

    public string ContraseniaHash
    {
        get => _contraseniaHash;
        set => _contraseniaHash = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase Cliente con los datos proporcionados.
    /// </summary>
    /// <param name="id">Identificador único del cliente.</param>
    /// <param name="nombres">Nombres del cliente.</param>
    /// <param name="apellidos">Apellidos del cliente.</param>
    /// <param name="correo">Correo electrónico del cliente.</param>
    /// <param name="edad">Edad del cliente.</param>
    /// <param name="contraseniaHash">Hash de la contraseña del cliente.</param>
    public Usuario(int id, string nombres, string apellidos, string correo, int edad, string contraseniaHash)
    {
        _id = id;
        _nombres = nombres;
        _apellidos = apellidos;
        _correo = correo;
        _edad = edad;
        _contraseniaHash = contraseniaHash;
        
        // Generar un salt aleatorio para el hash de la contraseña
        _salt = GenerarSalt();
        _contraseniaHash = HashContrasenia(contraseniaHash, _salt);
    }
    
    // Método para verificar contraseña
    public bool VerificarContrasenia(string contrasenia)
    {
        string hash = HashContrasenia(contrasenia, _salt);
        return hash == _contraseniaHash;
    }
    
    // Genera un salt aleatorio para aumentar seguridad
    private string GenerarSalt()
    {
        byte[] saltBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }
    
    // Implementa el hash SHA-256 con salt
    private string HashContrasenia(string contrasenia, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            // Combina contraseña con salt antes de aplicar hash
            string contraseniaConSalt = contrasenia + salt;
            byte[] bytes = Encoding.UTF8.GetBytes(contraseniaConSalt);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
    
    // Valida si el Usuario es administrador
    /*
     * El nombre del usuario administrador será admin@usac.com y
     * su contraseña será admint123
     */
    public bool EsAdmin()
    {
        return Correo == "admin@usac.com" && VerificarContrasenia("admint123");
    }
    
    // Método para mostrar información del usuario
    public override string ToString()
    {
        return $"ID: {_id}, Nombres: {_nombres}, Apellidos: {_apellidos}, Correo: {_correo}, Edad: {_edad}";
    }
}