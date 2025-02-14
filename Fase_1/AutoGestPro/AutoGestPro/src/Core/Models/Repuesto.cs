namespace AutoGestPro.Core.Models;

public class Repuesto
{
    private int _id;
    private string _repuesto;
    private string _detalle;
    private double _costo;

    public Repuesto(int id, string repuesto, string detalle, double costo)
    {
        _id = id;
        _repuesto = repuesto;
        _detalle = detalle;
        _costo = costo;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Repuesto1
    {
        get => _repuesto;
        set => _repuesto = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Detalle
    {
        get => _detalle;
        set => _detalle = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double Costo
    {
        get => _costo;
        set => _costo = value;
    }
}