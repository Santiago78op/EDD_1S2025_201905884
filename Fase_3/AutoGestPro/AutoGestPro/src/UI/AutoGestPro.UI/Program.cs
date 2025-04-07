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
        // Mostrar la interfaz
        mainWindow.ShowAll();
        // Ejecutar la aplicación
        Application.Run();
    }
}