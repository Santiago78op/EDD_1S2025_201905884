namespace AutoGestPro.Core.Models;

/// <summary>
/// Representa un vehículo.
/// </summary>
public class Vehiculo
{
    private int _id;
    private int _idUsuario;
    private string _marca;
    private int _modelo;
    private string _placa;
    
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Vehiculo"/>.
    /// </summary>
    /// <param name="id">El identificador del vehículo.</param>
    /// <param name="idUsuario">El identificador del usuario.</param>
    /// <param name="marca">La marca del vehículo.</param>
    /// <param name="modelo">El modelo del vehículo.</param>
    /// <param name="placa">La placa del vehículo.</param>
    public Vehiculo(int id, int idUsuario, string marca, int modelo, string placa)
    {
        _id = id;
        _idUsuario = idUsuario;
        _marca = marca ?? throw new ArgumentNullException(nameof(marca));
        _modelo = modelo;
        _placa = placa ?? throw new ArgumentNullException(nameof(placa));
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