namespace AutoGestPro.UI.Views.Admin;

using Gtk;

/// <summary>
/// Ventana de menú para el usuario administrador.
/// Desde aquí se puede acceder a todas las funcionalidades administrativas.
/// </summary>
public class MenuAdministrador
{
    
    public void OnCargaMaisva(object sender, EventArgs e)
    {
        // Lógica para abrir la gestión de Cargas Masivas
        var ventana = new GestionCargaMasiva();
        ventana.ShowAll();
    }

    public void OnInsercionUsuario(object sender, EventArgs e)
    {
        // Lógica para abrir la gestión de Cargas Masivas
        var ventana = new GestionUsuariosInsercion();
        ventana.ShowAll();
    }

    public void OnVisualizarUsuario(object sender, EventArgs e)
    {
        var ventana = new VisualizacionUsuarios();
        ventana.ShowAll();
    }

    public void OnGestionServicios(object sender, EventArgs e)
    {
        Console.WriteLine("Abrir Servicios");
    }

    public void OnGestionFacturas(object sender, EventArgs e)
    {
        Console.WriteLine("Abrir Facturas");
    }

    public void OnGenerarReportes(object sender, EventArgs e)
    {
        Console.WriteLine("Generar Reportes");
    }

    public void OnMostrarBitacora(object sender, EventArgs e)
    {
        Console.WriteLine("Mostrar Bitácora");
    }
    /*
     *ntana = new GestionUsuarios();
           ventana.ShowAll();
       }
       public void OnGestionVehiculos(object sender, EventArgs e)
       {
           var ventana = new GestionVehiculos();
           ventana.ShowAll();
       }
       public void OnGestionRepuestos(object sender, EventArgs e)
       {
           var ventana = new GestionRepuestos();
           ventana.ShowAll();
       }
       public void OnGestionServicios(object sender, EventArgs e)
       {
           var ventana = new GestionServicios();
           ventana.ShowAll();
       }
       public void OnGestionFacturas(object sender, EventArgs e)
       {
           var ventana = new GestionFacturas();
           ventana.ShowAll();
       }
       public void OnGenerarReportes(object sender, EventArgs e)
       {
           var ventana = new GeneradorReportes();
           ventana.ShowAll();
       }
       public void OnMostrarBitacora(object sender, EventArgs e)
       {
           var ventana = new BitacoraView();
           ventana.ShowAll();
       }* /
     * 
     */
}