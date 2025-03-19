using System.Diagnostics;
using AutoGestPro.Core.Graphs;
using Gtk;

namespace AutoGestPro.UI.Admin;

public class GenerarReportes : Window
{
    private Button _btnReporteUsuarios, _btnReporteVehiculos, _btnReporteRepuestos, _btnReporteServicios ,_btnReporteFacturas;
    // Obtener la ruta absoluta de la carpeta raíz del proyecto
    private static string _rutaProyecto = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    // Combinar con la carpeta de reportes
    private string _rutaReportes = System.IO.Path.Combine(_rutaProyecto, "Reports");
    // Contador Reportes para generar nombres únicos
    private int _contadorReportesCliente, _contadorReportesVehiculo, _contadorReportesRepuesto, _contadorReportesServicio, _contadorReportesFactura;
    
    public GenerarReportes() : base("Generación de Reportes")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide();
        // Crear la carpeta "Reportes" si no existe
        if (!Directory.Exists(_rutaReportes))
        {
            Directory.CreateDirectory(_rutaReportes);
        }

        VBox vbox = new VBox(false, 5) { BorderWidth = 10 };
        Label lblTitulo = new Label("Seleccione el reporte a generar:");
        _btnReporteUsuarios = new Button("Generar Reporte de Usuarios");
        _btnReporteUsuarios.Clicked += (sender, e) => GenerarReporteUsuarios();
        _btnReporteVehiculos = new Button("Generar Reporte de Vehículos");
        _btnReporteVehiculos.Clicked += (sender, e) => GenerarReporteVehiculos();
        _btnReporteRepuestos = new Button("Generar Reporte de Repuestos");
        _btnReporteRepuestos.Clicked += (sender, e) => GenerarReporteRepuestos();
        _btnReporteServicios = new Button("Generar Reporte de Servicios");
        _btnReporteServicios.Clicked += (sender, e) => GenerarReporteServicios();
        _btnReporteFacturas = new Button("Generar Reporte de Facturas");
        _btnReporteFacturas.Clicked += (sender, e) => GenerarReporteFacturas();
        
        vbox.PackStart(lblTitulo, false, false, 5);
        vbox.PackStart(_btnReporteUsuarios, false, false, 5);
        vbox.PackStart(_btnReporteVehiculos, false, false, 5);
        vbox.PackStart(_btnReporteRepuestos, false, false, 5);
        vbox.PackStart(_btnReporteServicios, false, false, 5);
        /*vbox.PackStart(_btnReporteFacturas, false, false, 5);*/
        Add(vbox);
        ShowAll();
    }

    // ✅ Generar reporte de Usuarios
    private void GenerarReporteUsuarios()
    {
        string dotFilePath = $"{_rutaReportes}/usuarios_{_contadorReportesCliente}.dot";
        string outputImagePath = $"{_rutaReportes}/usuarios_{_contadorReportesCliente}.png";
        string dotContent = ReporteService.GenerarDotUsuarios();
        GenerarImagenGraphviz(dotFilePath, outputImagePath, dotContent);
        _contadorReportesCliente++;
    }

    // ✅ Generar reporte de Vehículos
    private void GenerarReporteVehiculos()
    {
        string dotFilePath = $"{_rutaReportes}/vehiculos_{_contadorReportesVehiculo}.dot";
        string outputImagePath = $"{_rutaReportes}/vehiculos_{_contadorReportesVehiculo}.png";
        string dotContent = ReporteService.GenerarDotVehiculos();
        GenerarImagenGraphviz(dotFilePath, outputImagePath, dotContent);
        _contadorReportesVehiculo++;
    }

    // ✅ Generar reporte de Repuestos
    private void GenerarReporteRepuestos()
    {
        string dotFilePath = $"{_rutaReportes}/repuestos_{_contadorReportesRepuesto}.dot";
        string outputImagePath = $"{_rutaReportes}/repuestos_{_contadorReportesRepuesto}.png";
        string dotContent = ReporteService.GenerarDotRepuestos();
        GenerarImagenGraphviz(dotFilePath, outputImagePath, dotContent);
        _contadorReportesRepuesto++;
    }
    
    // ✅ Generar reporte de Servicios
    private void GenerarReporteServicios()
    {
        string dotFilePath = $"{_rutaReportes}/servicios_{_contadorReportesServicio}.dot";
        string outputImagePath = $"{_rutaReportes}/servicios_{_contadorReportesServicio}.png";
        string dotContent = ReporteService.GenerarDotServicios();
        GenerarImagenGraphviz(dotFilePath, outputImagePath, dotContent);
        _contadorReportesServicio++;
    }

    // ✅ Generar reporte de Facturas
    private void GenerarReporteFacturas()
    {
        string dotFilePath = $"{_rutaReportes}/facturas_{_contadorReportesFactura}.dot";
        string outputImagePath = $"{_rutaReportes}/facturas_{_contadorReportesFactura}.png";
        string dotContent = ReporteService.GenerarDotFacturas();
        GenerarImagenGraphviz(dotFilePath, outputImagePath, dotContent);
        _contadorReportesFactura++;
    }

    // ✅ Método para generar la imagen con Graphviz
    private void GenerarImagenGraphviz(string dotFilePath, string outputImagePath, string dotContent)
    {
        try
        {
            File.WriteAllText(dotFilePath, dotContent);
            Process process = new Process();
            process.StartInfo.FileName = "dot";
            process.StartInfo.Arguments = $"-Tpng \"{dotFilePath}\" -o \"{outputImagePath}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            if (File.Exists(outputImagePath))
            {
                MostrarMensaje("Éxito", $"Reporte generado correctamente: {outputImagePath}");
            }
            else
            {
                MostrarMensaje("Error", "No se pudo generar la imagen con Graphviz.");
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error", $"Error al generar reporte: {ex.Message}");
        }
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