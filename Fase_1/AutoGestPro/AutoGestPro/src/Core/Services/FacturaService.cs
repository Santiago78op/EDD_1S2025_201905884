namespace AutoGestPro.Core.Services;

public class FacturaService
{
    private Stack<Factura> pilaFacturas = new Stack<Factura>();
    private int contadorID = 1;

    public void GenerarFactura(int idOrden, decimal costoServicio, decimal costoRepuesto)
    {
        decimal total = costoServicio + costoRepuesto;

        Factura nuevaFactura = new Factura
        {
            ID = contadorID++,
            ID_Orden = idOrden,
            Total = total
        };

        pilaFacturas.Push(nuevaFactura);
        System.Console.WriteLine($"✅ Factura Generada: {nuevaFactura}");
    }

    public Factura ObtenerUltimaFactura()
    {
        return pilaFacturas.Count > 0 ? pilaFacturas.Peek() : null;
    }

    public Factura RetirarFactura()
    {
        return pilaFacturas.Count > 0 ? pilaFacturas.Pop() : null;
    }

    public int ObtenerTotalFacturas()
    {
        return pilaFacturas.Count;
    }
}