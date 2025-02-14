namespace AutoGestPro.Core.Models;

public class Cliente
{
    private int _id;
    private string _nombre;
    private string _apellido;
    private string _correo;
    private string _contrasenia;

    public Cliente(int id, string nombre, string apellido, string correo, string contrasenia)
    {
        _id = id;
        _nombre = nombre;
        _apellido = apellido;
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
        get => _nombre;
        set => _nombre = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Apellido
    {
        get => _apellido;
        set => _apellido = value ?? throw new ArgumentNullException(nameof(value));
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