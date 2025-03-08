namespace AutoGestPro.Core.Models;

public class Servicio
{
    private int _id;
    private int _idRepuesto;
    private int _idVehiculo;
    private string _detalles;
    private decimal _costo;

    public Servicio(int id, int idRepuesto, int idVehiculo, string detalles, decimal costo)
    {
        _id = id;
        _idRepuesto = idRepuesto;
        _idVehiculo = idVehiculo;
        _detalles = detalles;
        _costo = costo;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
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