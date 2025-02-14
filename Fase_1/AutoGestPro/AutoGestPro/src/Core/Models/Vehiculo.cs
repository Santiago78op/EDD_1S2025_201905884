namespace AutoGestPro.Core.Models;

public class Vehiculo
{
    private int _id;
    private int _idUsuario;
    private string _marca;
    private string _modelo;
    private String _placa;

    public Vehiculo(int id, int idUsuario, string marca, string modelo, string placa)
    {
        _id = id;
        _idUsuario = idUsuario;
        _marca = marca;
        _modelo = modelo;
        _placa = placa;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public int IdUsuario
    {
        get => _idUsuario;
        set => _idUsuario = value;
    }

    public string Marca
    {
        get => _marca;
        set => _marca = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Modelo
    {
        get => _modelo;
        set => _modelo = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Placa
    {
        get => _placa;
        set => _placa = value ?? throw new ArgumentNullException(nameof(value));
    }
}