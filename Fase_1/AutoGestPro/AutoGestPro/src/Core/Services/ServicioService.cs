using AutoGestPro.Core.Models;

namespace AutoGestPro.Core.Services;

public class ServicioService
{
    private List<Servicio> listaServicios = new List<Servicio>();
    private int contadorID = 1;
    private FacturaService facturaService;

    public ServicioService()
    {
        facturaService = new FacturaService();
    }

    public int ObtenerNuevoId()
    {
        return contadorID++;
    }

    public void RegistrarServicio(Servicio servicio, decimal costoRepuesto)
    {
        listaServicios.Add(servicio);

        // 🔥 Genera la Factura Automáticamente
        facturaService.GenerarFactura(servicio.ID, 100.00m, costoRepuesto); // Simulación del costo del servicio
    }
}