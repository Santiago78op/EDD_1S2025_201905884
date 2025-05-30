using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Models;

public class Cliente
{
    private int _id;
    private string _nombres;
    private string _apellidos;
    private string _correo;
    private int _edad;
    private string _contrasenia;

    private TreeBinary _Servicios;
    private TreeB _Facturas;

public Cliente(int id, string nombres, string apellidos, string correo, int edad, string contrasenia)
    {
        _id = id;
        _nombres = nombres;
        _apellidos = apellidos;
        _correo = correo;
        _edad = edad;
        _contrasenia = contrasenia;
        _Servicios = new TreeBinary();
        _Facturas = new TreeB(5);
    }

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

    public string Contrasenia
    {
        get => _contrasenia;
        set => _contrasenia = value ?? throw new ArgumentNullException(nameof(value));
    }

    public TreeBinary Servicios
    {
        get => _Servicios;
        set => _Servicios = value ?? throw new ArgumentNullException(nameof(value));
    }

    public TreeB Facturas
    {
        get => _Facturas;
        set => _Facturas = value ?? throw new ArgumentNullException(nameof(value));
    }
}