namespace AutoGestPro.Core.Models;

public class Vehiculo
{
    private int _id;
    private int _id_usuario;
    private string _marca;
    private int _modelo;
    private string _placa;

    public Vehiculo(int id, int idUsuario, string marca, int modelo, string placa)
    {
        _id = id;
        _id_usuario = idUsuario;
        _marca = marca;
        _modelo = modelo;
        _placa = placa;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public int Id_Usuario
    {
        get => _id_usuario;
        set => _id_usuario = value;
    }

    public string Marca
    {
        get => _marca;
        set => _marca = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Modelo
    {
        get => _modelo;
        set => _modelo = value;
    }

    public string Placa
    {
        get => _placa;
        set => _placa = value ?? throw new ArgumentNullException(nameof(value));
    }
}