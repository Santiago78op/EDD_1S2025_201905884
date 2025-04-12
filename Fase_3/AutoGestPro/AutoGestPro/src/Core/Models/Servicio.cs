namespace AutoGestPro.Core.Models;

/// <summary>
/// Representa un servicio realizado a un vehículo.
/// </summary>
public class Servicio
{
    private int _id;
    private int _idUsuario;
    private int _idRepuesto;
    private int _idVehiculo;
    private string _detalles;
    private decimal _costo;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Servicio"/>.
    /// </summary>
    /// <param name="id">El identificador del servicio.</param>
    /// <param name="idUsuario">El identificador del usuario.</param>
    /// <param name="idRepuesto">El identificador del repuesto.</param>
    /// <param name="idVehiculo">El identificador del vehículo.</param>
    /// <param name="detalles">Los detalles del servicio.</param>
    /// <param name="costo">El costo del servicio.</param>
    public Servicio(int id, int idRepuesto, int idVehiculo, string detalles, decimal costo)
    {
        _id = id;
        _idUsuario = 0;
        _idRepuesto = idRepuesto;
        _idVehiculo = idVehiculo;
        _detalles = detalles ?? throw new ArgumentNullException(nameof(detalles));
        _costo = costo;
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

    public int IdRepuesto
    {
        get => _idRepuesto;
        set => _idRepuesto = value;
    }

    public int IdVehiculo
    {
        get => _idVehiculo;
        set => _idVehiculo = value;
    }

    public string Detalles
    {
        get => _detalles;
        set => _detalles = value ?? throw new ArgumentNullException(nameof(value));
    }

    public decimal Costo
    {
        get => _costo;
        set => _costo = value;
    }
}