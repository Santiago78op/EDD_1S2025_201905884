namespace AutoGestPro.UI.Views.User;

/// <summary>
/// Clase que contiene los manejadores de eventos para el menú del usuario.
/// </summary>
public class MenuUsuario
{
    /*/// <summary>
    /// Abre la ventana con la lista de vehículos del usuario logueado.
    /// </summary>
    public void OnMisVehiculos(object sender, EventArgs e)
    {
        var ventana = new MisVehiculosView();
        ventana.ShowAll();
    }
    /// <summary>
    /// Abre la ventana que muestra los servicios aplicados a los vehículos del usuario.
    /// </summary>
    public void OnMisServicios(object sender, EventArgs e)
    {
        var ventana = new MisServiciosView();
        ventana.ShowAll();
    }
    /// <summary>
    /// Abre la ventana con las facturas pendientes o históricas del usuario.
    /// </summary>
    public void OnMisFacturas(object sender, EventArgs e)
    {
        var ventana = new MisFacturasView();
        ventana.ShowAll();
    }*/
    public void OnMisVehiculos(object? sender, EventArgs e)
    {
        // Lógica para abrir la gestión de visualización de vehículos
        var ventana = new VisualizacionVehiculos();
        ventana.ShowAll();
    }

    public void OnMisServicios(object? sender, EventArgs e)
    {
        // Lógica para abrir la gestión de servicios
        var ventana = new VisualizacionServicios();
        ventana.ShowAll();
    }

    public void OnMisFacturas(object? sender, EventArgs e)
    {
        // Lógica para abrir la gestión de facturas
        var ventana = new VisualizacionFacturas();
        ventana.ShowAll();
    }
}