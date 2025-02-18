namespace AutoGestPro.Core.Models;

public class Factura
{
    private int _id;
    private int _id_orden;
    private double _total;
    
    public Factura(int id, int id_orden, double total)
    {
        _id = id;
        _id_orden = id_orden;
        _total = total;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public int IdOrden
    {
        get => _id_orden;
        set => _id_orden = value;
    }

    public double Total
    {
        get => _total;
        set => _total = value;
    }
}