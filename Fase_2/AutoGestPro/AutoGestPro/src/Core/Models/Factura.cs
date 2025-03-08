namespace AutoGestPro.Core.Models;

public class Factura
{
    private int _id;
    private int _idServicio;
    private decimal _total;
    
    public Factura(int id, int idServicio, decimal total)
    {
        _id = id;
        _idServicio = idServicio;
        _total = total;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public int IdServicio
    {
        get => _idServicio;
        set => _idServicio = value;
    }

    public decimal Total
    {
        get => _total;
        set => _total = value;
    }
}