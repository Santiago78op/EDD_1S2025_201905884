using AutoGestPro.Core.Models;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

public unsafe class FacturaService
{
    // Acceso a la lista
    public static StackList<Factura> pilaFacturas = new StackList<Factura>();
    private int contadorID = 1;

    public void GenerarFactura(int idOrden, double costoServicio, double costoRepuesto)
    {
        double total = costoServicio + costoRepuesto;

        Factura nuevaFactura = new Factura(contadorID++, idOrden, total);

        pilaFacturas.push(nuevaFactura);
        System.Console.WriteLine($"✅ Factura Generada: {nuevaFactura}");
    }

    public Factura ObtenerUltimaFactura()
    {
        return pilaFacturas.Height > 0 ? pilaFacturas.peek()->_data : null;
    }

    public Factura RetirarFactura()
    {
        return pilaFacturas.Height > 0 ? pilaFacturas.pop()->_data : null;
    }

    public int ObtenerTotalFacturas()
    {
        return pilaFacturas.Height;
    }
}