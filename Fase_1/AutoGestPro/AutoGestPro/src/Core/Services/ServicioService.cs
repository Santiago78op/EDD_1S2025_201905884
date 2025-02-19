using AutoGestPro.Core.Models;

namespace AutoGestPro.Core.Services;

public class ServicioService
{
    private int contadorID = 1;
    private FacturaService facturaService;

    public ServicioService()
    {
        facturaService = new FacturaService();
    }

    public void RegistrarServicio(Servicio servicio, double costoRepuesto)
    {
        CargaMasivaService.servicios.enqueue(servicio);

        // 🔥 Genera la Factura Automáticamente
        facturaService.GenerarFactura(servicio.Id, servicio.Costo, costoRepuesto); // Simulación del costo del servicio
    }
}