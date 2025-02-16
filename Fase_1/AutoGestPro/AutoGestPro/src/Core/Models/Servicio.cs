namespace AutoGestPro.Core.Models;

public class Servicio
{
    private int _id;
    private int id_repuesto;
    private int id_vehiculo;
    private string _detalles;
    private double _costo;

    public Servicio(int id, int idRepuesto, int idVehiculo, string detalles, double costo)
    {
        _id = id;
        id_repuesto = idRepuesto;
        id_vehiculo = idVehiculo;
        _detalles = detalles;
        _costo = costo;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public int Id_Repuesto
    {
        get => id_repuesto;
        set => id_repuesto = value;
    }

    public int Id_Vehiculo
    {
        get => id_vehiculo;
        set => id_vehiculo = value;
    }

    public string Detalle
    {
        get => _detalles;
        set => _detalles = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double Costo
    {
        get => _costo;
        set => _costo = value;
    }
}