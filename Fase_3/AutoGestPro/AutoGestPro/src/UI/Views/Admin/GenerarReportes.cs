using System.Diagnostics;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Graphs;
using Gtk;

namespace AutoGestPro.UI.Views.Admin;

public class GenerarReportes : Window
{
    private Button _btnReporteUsuarios, _btnReporteVehiculos, _btnReporteRepuestos, _btnReporteServicios, _btnReporteFacturas, _btnReporteGrafo;
    
    // Instancia de la clase Graphviz para generar reportes
    private readonly Graphviz _graphviz;
    
    // Barra de progreso y etiqueta de estado
    private ProgressBar _progressBar;
    private Label _lblStatus;
    
    // Contadores para reportes que no usa Graphviz directamente
    private int _contadorReportesRepuesto, _contadorReportesServicio, _contadorReportesFactura, _contadorReportesGrafo;
    
    public GenerarReportes() : base("Generación de Reportes")
    {
        // Inicializar la instancia de Graphviz
        _graphviz = new Graphviz();
        
        SetDefaultSize(500, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide();
        
        // Asegurarse de que la carpeta de reportes existe
        string reportPath = Graphviz.GetReportPath();

        // Crear la interfaz de usuario
        VBox vbox = new VBox(false, 10) { BorderWidth = 15 };
        
        // Título con estilo
        Label lblTitulo = new Label();
        lblTitulo.Markup = "<span size='large' weight='bold'>Seleccione el reporte a generar:</span>";
        lblTitulo.SetAlignment(0, 0.5f);
        vbox.PackStart(lblTitulo, false, false, 10);
        
        // Crear botones con íconos
        _btnReporteUsuarios = CreateStyledButton("👤 Generar Reporte de Usuarios");
        _btnReporteUsuarios.Clicked += (sender, e) => GenerarReporteUsuarios();
        
        _btnReporteVehiculos = CreateStyledButton("🚗 Generar Reporte de Vehículos");
        _btnReporteVehiculos.Clicked += (sender, e) => GenerarReporteVehiculos();
        
        _btnReporteRepuestos = CreateStyledButton("🔧 Generar Reporte de Repuestos");
        _btnReporteRepuestos.Clicked += (sender, e) => GenerarReporteRepuestos();
        
        _btnReporteServicios = CreateStyledButton("🛠️ Generar Reporte de Servicios");
        _btnReporteServicios.Clicked += (sender, e) => GenerarReporteServicios();
        
        _btnReporteFacturas = CreateStyledButton("📝 Generar Reporte de Facturas");
        _btnReporteFacturas.Clicked += (sender, e) => GenerarReporteFacturas();
        
        _btnReporteGrafo = CreateStyledButton("🔄 Generar Reporte de Compatibilidad");
        _btnReporteGrafo.Clicked += (sender, e) => GenerarReporteGrafo();
        
        // Añadir botones al contenedor
        vbox.PackStart(_btnReporteUsuarios, false, false, 5);
        vbox.PackStart(_btnReporteVehiculos, false, false, 5);
        vbox.PackStart(_btnReporteRepuestos, false, false, 5);
        vbox.PackStart(_btnReporteServicios, false, false, 5);
        vbox.PackStart(_btnReporteFacturas, false, false, 5);
        vbox.PackStart(_btnReporteGrafo, false, false, 5);
        
        // Añadir barra de progreso
        _progressBar = new ProgressBar();
        _progressBar.Fraction = 0.0;
        _progressBar.NoShowAll = true;
        vbox.PackStart(_progressBar, false, false, 10);
        
        // Añadir etiqueta de estado
        _lblStatus = new Label("");
        _lblStatus.SetAlignment(0, 0.5f);
        vbox.PackStart(_lblStatus, false, false, 5);
        
        Add(vbox);
        ShowAll();
    }
    
    private Button CreateStyledButton(string text)
    {
        var button = new Button(text);
        button.HeightRequest = 40;
        return button;
    }

    // ✅ Generar reporte de Usuarios
    private async void GenerarReporteUsuarios()
    {
        try
        {
            // Deshabilitar botones y mostrar progreso
            SetButtonsEnabled(false);
            _progressBar.Show();
            UpdateStatus("Generando reporte de usuarios...");
            
            // Animar barra de progreso
            await AnimateProgress();
            
            // Usar la clase Graphviz para generar el reporte
            await Task.Run(() => _graphviz.GeneraReporteUsuario());
            
            UpdateStatus("Reporte de usuarios generado correctamente");
            MostrarMensaje("Éxito", "Reporte de usuarios generado correctamente");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}");
            MostrarMensaje("Error", $"Error al generar reporte: {ex.Message}");
        }
        finally
        {
            // Restaurar UI
            _progressBar.Fraction = 1.0;
            await Task.Delay(500);
            _progressBar.Hide();
            SetButtonsEnabled(true);
        }
    }

    // ✅ Generar reporte de Vehículos
    private async void GenerarReporteVehiculos()
    {
        try
        {
            // Deshabilitar botones y mostrar progreso
            SetButtonsEnabled(false);
            _progressBar.Show();
            UpdateStatus("Generando reporte de vehículos...");
            
            // Animar barra de progreso
            await AnimateProgress();
            
            // Usar la clase Graphviz para generar el reporte
            await Task.Run(() => _graphviz.GeneraReporteVehiculo());
            
            UpdateStatus("Reporte de vehículos generado correctamente");
            MostrarMensaje("Éxito", "Reporte de vehículos generado correctamente");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}");
            MostrarMensaje("Error", $"Error al generar reporte: {ex.Message}");
        }
        finally
        {
            // Restaurar UI
            _progressBar.Fraction = 1.0;
            await Task.Delay(500);
            _progressBar.Hide();
            SetButtonsEnabled(true);
        }
    }

    // ✅ Generar reporte de Repuestos
    private async void GenerarReporteRepuestos()
    {
        try
        {
            // Deshabilitar botones y mostrar progreso
            SetButtonsEnabled(false);
            _progressBar.Show();
            UpdateStatus("Generando reporte de repuestos...");
            
            // Animar barra de progreso
            await AnimateProgress();
            
            // Usar la clase Graphviz para generar el reporte
            await Task.Run(() => _graphviz.GeneraReporteRepuesto());
            
            UpdateStatus("Reporte de repuestos generado correctamente");
            MostrarMensaje("Éxito", "Reporte de repuestos generado correctamente");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}");
            MostrarMensaje("Error", $"Error al generar reporte: {ex.Message}");
        }
        finally
        {
            // Restaurar UI
            _progressBar.Fraction = 1.0;
            await Task.Delay(500);
            _progressBar.Hide();
            SetButtonsEnabled(true);
        }
    }
    
    // ✅ Generar reporte de Servicios
    private async void GenerarReporteServicios()
    {
        try
        {
            // Deshabilitar botones y mostrar progreso
            SetButtonsEnabled(false);
            _progressBar.Show();
            UpdateStatus("Generando reporte de servicios...");
            
            // Animar barra de progreso
            await AnimateProgress();
            
            // Usar la clase Graphviz para generar el reporte
            await Task.Run(() => _graphviz.GeneraReporteServicio());
            
            UpdateStatus("Reporte de servicios generado correctamente");
            MostrarMensaje("Éxito", "Reporte de servicios generado correctamente");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}");
            MostrarMensaje("Error", $"Error al generar reporte: {ex.Message}");
        }
        finally
        {
            // Restaurar UI
            _progressBar.Fraction = 1.0;
            await Task.Delay(500);
            _progressBar.Hide();
            SetButtonsEnabled(true);
        }
    }

    // ✅ Generar reporte de Facturas
    private async void GenerarReporteFacturas()
    {
        await GenerarReporteGenerico("facturas", _contadorReportesFactura, ReporteService.GenerarDotFacturas);
    }
    
    // ✅ Generar reporte de grafo de compatibilidad
    private async void GenerarReporteGrafo()
    {
        await GenerarReporteGenerico("grafo", _contadorReportesGrafo, () => Estructuras.Grafo.GenerarDot());
    }
    
    // Método genérico para generar reportes
    private async Task GenerarReporteGenerico(string tipo, int contador, Func<string> generarDot)
    {
        try
        {
            // Deshabilitar botones y mostrar progreso
            SetButtonsEnabled(false);
            _progressBar.Show();
            UpdateStatus($"Generando reporte de {tipo}...");
            
            // Animar barra de progreso
            await AnimateProgress();
            
            // Generar el reporte
            contador++;
            string reportPath = Graphviz.GetReportPath();
            string dotFilePath = System.IO.Path.Combine(reportPath, $"{tipo}_{contador}.dot");
            string outputImagePath = System.IO.Path.Combine(reportPath, $"{tipo}_{contador}.png");
            
            // Generar el contenido DOT
            string dotContent = generarDot();
            
            // Generar la imagen de forma asíncrona
            await Task.Run(() => _graphviz.GenerarDotImg(dotFilePath, outputImagePath, dotContent));
            
            // Verificar si la imagen se generó correctamente
            if (File.Exists(outputImagePath))
            {
                MostrarMensaje("Éxito", $"Reporte generado correctamente: {outputImagePath}");
                UpdateStatus($"Reporte de {tipo} generado: {outputImagePath}");
                
                // Intentar abrir la imagen generada
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = outputImagePath,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No se pudo abrir la imagen: {ex.Message}");
                }
            }
            else
            {
                MostrarMensaje("Error", "No se pudo generar la imagen con Graphviz.");
                UpdateStatus("Error al generar la imagen.");
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error", $"Error al generar reporte: {ex.Message}");
            UpdateStatus($"Error: {ex.Message}");
        }
        finally
        {
            // Restaurar UI
            _progressBar.Fraction = 1.0;
            await Task.Delay(500);
            _progressBar.Hide();
            SetButtonsEnabled(true);
        }
    }
    
    // Habilitar o deshabilitar todos los botones
    private void SetButtonsEnabled(bool enabled)
    {
        _btnReporteUsuarios.Sensitive = enabled;
        _btnReporteVehiculos.Sensitive = enabled;
        _btnReporteRepuestos.Sensitive = enabled;
        _btnReporteServicios.Sensitive = enabled;
        _btnReporteFacturas.Sensitive = enabled;
        _btnReporteGrafo.Sensitive = enabled;
    }
    
    // Animar la barra de progreso
    private async Task AnimateProgress()
    {
        for (double i = 0; i <= 0.9; i += 0.1)
        {
            _progressBar.Fraction = i;
            await Task.Delay(100);
        }
    }
    
    // Actualizar la etiqueta de estado
    private void UpdateStatus(string mensaje)
    {
        _lblStatus.Text = mensaje;
    }

    // ✅ Método para mostrar mensajes de alerta
    private void MostrarMensaje(string titulo, string mensaje)
    {
        MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
        dialog.Title = titulo;
        dialog.Run();
        dialog.Destroy();
    }
}

/// <summary>
/// Clase de servicio para la generación de reportes.
/// </summary>
public static class ReporteService
{
    /// <summary>
    /// Genera el código DOT para el reporte de repuestos.
    /// </summary>
    public static string GenerarDotRepuestos()
    {
        try
        {
            // Aquí puedes llamar al método correspondiente en tu estructura de datos
            // Por ejemplo: return Estructuras.Repuestos.GenerarDotRepuestos();
            // Como no tengo acceso a ese método, utilizo un placeholder
            return "digraph G { label=\"Árbol de Repuestos\"; node1 [label=\"Repuesto 1\"]; }";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar dot de repuestos: {ex.Message}");
            return $"digraph G {{ label=\"Error al generar reporte: {ex.Message}\"; }}";
        }
    }
    
    /// <summary>
    /// Genera el código DOT para el reporte de servicios.
    /// </summary>
    public static string GenerarDotServicios()
    {
        try
        {
            // Aquí puedes llamar al método correspondiente en tu estructura de datos
            // Por ejemplo: return Estructuras.Servicios.GenerarDotServicios();
            // Como no tengo acceso a ese método, utilizo un placeholder
            return "digraph G { label=\"Árbol de Servicios\"; node1 [label=\"Servicio 1\"]; }";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar dot de servicios: {ex.Message}");
            return $"digraph G {{ label=\"Error al generar reporte: {ex.Message}\"; }}";
        }
    }
    
    /// <summary>
    /// Genera el código DOT para el reporte de facturas.
    /// </summary>
    public static string GenerarDotFacturas()
    {
        try
        {
            // Aquí puedes llamar al método correspondiente en tu estructura de datos
            // Por ejemplo: return Estructuras.Facturas.GenerarDotFacturas();
            // Como no tengo acceso a ese método, utilizo un placeholder
            return "digraph G { label=\"Árbol de Facturas\"; node1 [label=\"Factura 1\"]; }";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar dot de facturas: {ex.Message}");
            return $"digraph G {{ label=\"Error al generar reporte: {ex.Message}\"; }}";
        }
    }
}