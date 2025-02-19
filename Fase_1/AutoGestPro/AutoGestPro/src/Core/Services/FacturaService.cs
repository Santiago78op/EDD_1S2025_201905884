using AutoGestPro.Core.Models;

namespace AutoGestPro.Core.Services;

public class FacturaService
{
    private Stack<Factura> pilaFacturas = new Stack<Factura>();
    private int contadorID = 1;

    public void GenerarFactura(int idOrden, double costoServicio, double costoRepuesto)
    {
        double total = costoServicio + costoRepuesto;

        Factura nuevaFactura = new Factura(contadorID++, idOrden, total);

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