using AutoGestPro.Core.Global;

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

    public void OnVisualizarRepuesto(object sender, EventArgs e)
    {
        var ventana = new VisualizacionRepuestos();
        ventana.ShowAll();
    }

    public void OnInsertarServicio(object sender, EventArgs e)
    {
        var ventana = new GestionServicios();
        ventana.ShowAll();
    }

    public void OnGenerarReportes(object sender, EventArgs e)
    {
        var ventana = new GenerarReportes();
        ventana.ShowAll();
    }

    public void OnGenerarBackup(object sender, EventArgs e)
    {
        // Lógica para abrir la gestión de Cargas Masivas
        var ventana = new GestionBackups();
        ventana.ShowAll();
    }
    
    public void OnVisualizarLogs(object sender, EventArgs e)
    {
        // Lógica para abrir la visualización de logs de usuarios
        var ventana = new VisualizacionLogs();
        ventana.ShowAll();
    }

    /* Función comentada para cargar backups, se puede desarrollar a futuro
    public void OnCargarBackup(object sender, EventArgs e)
    {
        // Lógica para abrir la gestión de Cargas Masivas
        var ventana = new RestaurarBackups();
        ventana.ShowAll();
    }
    */
}