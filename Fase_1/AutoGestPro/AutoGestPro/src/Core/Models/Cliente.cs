namespace AutoGestPro.Core.Models;

public class Cliente
{
    private int _id;
    private string _nombres;
    private string _apellidos;
    private string _correo;
    private string _contrasenia;

    public Cliente(int id, string nombres, string apellidos, string correo, string contrasenia)
    {
        _id = id;
        _nombres = nombres;
        _apellidos = apellidos;
        _correo = correo;
        _contrasenia = contrasenia;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Nombre
    {
        get => _nombres;
        set => _nombres = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Apellido
    {
        get => _apellidos;
        set => _apellidos = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Correo
    {
        get => _correo;
        set => _correo = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Contrasenia
    {
        get => _contrasenia;
        set => _contrasenia = value ?? throw new ArgumentNullException(nameof(value));
    }
}