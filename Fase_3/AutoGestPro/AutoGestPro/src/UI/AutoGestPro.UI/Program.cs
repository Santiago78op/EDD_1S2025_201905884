using AutoGestPro.Core.Services;
using AutoGestPro.UI;
using Gtk;

/// <summary>
/// Clase principal que contiene el punto de entrada de la aplicación.
/// internal es para que no se pueda acceder desde fuera del ensamblado.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Método principal que inicia la aplicación.
    /// </summary>
    /// <param name="args">Argumentos de línea de comandos (opcional).</param>
    [STAThread]
    public static void Main(string[] args)
    {
        // Inicializar GTK
        Application.Init();
        // Crear instancia de la ventana principal
        var app = new Application("org.AutoGestPro.App", GLib.ApplicationFlags.None);
        app.Register(GLib.Cancellable.Current);
        var mainWindow = new MainWindow();
        app.AddWindow(mainWindow);
        
        // Ejecutar restauración automática de entidades
        var autoRestore = new AutoRestoreService();
        var restoreResult = autoRestore.RestoreAutomatically();
            
        // Si la restauración falló, mostrar mensaje de error
        if (restoreResult.Success)
        {
            // Opcional: Mostrar mensaje de éxito
            var dialog = new MessageDialog(
                mainWindow,
                DialogFlags.Modal,
                MessageType.Info,
                ButtonsType.Ok,
                $"Restauración automática completada con éxito.\n" +
                $"Usuarios: {restoreResult.UsuariosRestored}\n" +
                $"Vehículos: {restoreResult.VehiculosRestored}\n" +
                $"Repuestos: {restoreResult.RepuestosRestored}");
            dialog.Title = "Éxito en la Restauración Automática";
            dialog.Run();
            dialog.Dispose();
        }

        
        // Mostrar la interfaz
        mainWindow.ShowAll();
        // Ejecutar la aplicación
        Application.Run();
    }
}