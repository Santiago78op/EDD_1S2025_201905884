using AutoGestPro.Core.Models;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

public unsafe class FacturaService
{
    // Acceso a la lista
    public static StackList<Factura> pilaFacturas = new StackList<Factura>();
    private int contadorID = 1;
    public BitacoraService bitacoraService;
    
    public FacturaService()
    {
        bitacoraService = new BitacoraService();
    }

    public void GenerarFactura(int idOrden, double costoServicio, double costoRepuesto, int idVehiculo, int idRepuesto, string detalle)
    {
        double total = costoServicio + costoRepuesto;

        Factura nuevaFactura = new Factura(contadorID++, idOrden, total);
        
        // 🔥 Insertar relación en la Bitácora
        Repuesto repuesto = CargaMasivaService.repuestos.searchNode(idRepuesto)->_data;
        Vehiculo vehiculo = CargaMasivaService.vehiculos.searchNode(idVehiculo)->_data;
        if (repuesto != null && vehiculo != null)
        {
            bitacoraService.InsertarRelacion(idVehiculo, idRepuesto, detalle);
        }
        else
        {
            System.Console.WriteLine($"❌ Error al generar Bitacora: No se encontró el repuesto o vehículo.");
        }

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