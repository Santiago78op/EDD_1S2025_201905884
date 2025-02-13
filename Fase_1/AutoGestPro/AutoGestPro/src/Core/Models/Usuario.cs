namespace AutoGestPro.Core.Models;

/**
 * Clase que representa un usuario del sistema.
 */
public class Usuario
{
    private String _nombreUsuario;
    private String _contrasenia;

    public Usuario(string nombreUsuario, string contrasenia)
    {
        this._nombreUsuario = nombreUsuario;
        this._contrasenia = contrasenia;
    }

    public string NombreUsuario
    {
        get => _nombreUsuario;
        set => _nombreUsuario = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Contrasenia
    {
        get => _contrasenia;
        set => _contrasenia = value ?? throw new ArgumentNullException(nameof(value));
    }
}