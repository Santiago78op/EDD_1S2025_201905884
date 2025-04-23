using System.Diagnostics;
using System.Reflection;
using AutoGestPro.Core.Global;

namespace AutoGestPro.Core.Graphs;

/// <summary>
/// Clase para generar imágenes a partir de archivos .dot utilizando Graphviz.
/// </summary>
public class Graphviz
{

    private int _contUser, _contCar, _contRep, _contFac, _contSer, _contCvr;
    
    // Obtener la ruta absoluta de la carpeta raíz del proyecto
    private static string GetProjectRootPath()
    {
        // Obtiene la ubbicación del ejecutable actual
        string exePath = Assembly.GetExecutingAssembly().Location;
        // Sube cuatro niveles para llegar a la raíz del proyecto
        string projectRoot = new DirectoryInfo(Path.GetDirectoryName(exePath))
            .Parent?.Parent?.Parent?.FullName;
    
        if (projectRoot == null)
            throw new DirectoryNotFoundException("No se pudo encontrar la raíz del proyecto");

        return projectRoot;
    }

    // Combinar con la carpeta de Reportes
    public static string GetReportPath()
    {
        string projectRootPath = GetProjectRootPath();
        string reportPath = Path.Combine(projectRootPath, "Reports");
        // Crear la carpeta "Reports" si no existe
        if (!Directory.Exists(reportPath))
        {
            Directory.CreateDirectory(reportPath);
        }
        return reportPath;
    }
    
    /// <summary>
    /// Método para generar un reporte de usuario en formato .dot.
    /// </summary>
    public void GeneraReporteUsuario()
    {
        string dotFilePath = $"{GetReportPath()}/usuarios_{++_contUser}.dot";
        string outputFilePath = $"{GetReportPath()}/usuarios_{_contUser}.png";
        string dotContent = Estructuras.Clientes.GenerarDotUsuarios();
        GenerarDotImg(dotFilePath, outputFilePath, dotContent);
    }
    
    /// <summary>
    /// Método para generar un reporte de vehículos en formato .dot.
    /// </summary>
    public void GeneraReporteVehiculo()
    {
        string dotFilePath = $"{GetReportPath()}/vehiculos_{++_contCar}.dot";
        string outputFilePath = $"{GetReportPath()}/vehiculos_{_contCar}.png";
        string dotContent = Estructuras.Vehiculos.GenerarDotVehiculos();
        GenerarDotImg(dotFilePath, outputFilePath, dotContent);
    }
    
    /// <summary>
    /// Método para generar un reporte de Repusto en formato .dot.
    /// </summary>
    public void GeneraReporteRepuesto()
    {
        string dotFilePath = $"{GetReportPath()}/repuestos_{++_contRep}.dot";
        string outputFilePath = $"{GetReportPath()}/repuestos_{_contRep}.png";
        string dotContent = Estructuras.Repuestos.GenerarDotRepuestos();
        GenerarDotImg(dotFilePath, outputFilePath, dotContent);
    }
    
    /// <summary>
    /// Método para generar un reporte de servicios en formato .dot.
    /// </summary>
    public void GeneraReporteServicio()
    {
        string dotFilePath = $"{GetReportPath()}/servicios_{++_contSer}.dot";
        string outputFilePath = $"{GetReportPath()}/servicios_{_contSer}.png";
        string dotContent = Estructuras.Servicios.GenerarDotServicios();
        GenerarDotImg(dotFilePath, outputFilePath, dotContent);
    }
    
    /// <summary>
    /// Método para generar un reporte de facturas en formato .dot.
    /// </summary>
    public void GeneraReporteFactura()
    {
        string dotFilePath = $"{GetReportPath()}/facturas_{++_contFac}.dot";
        string outputFilePath = $"{GetReportPath()}/facturas_{_contFac}.png";
        string dotContent = Estructuras.Facturas.GenerarDotFacturas();
        GenerarDotImg(dotFilePath, outputFilePath, dotContent);
    }
    
    /// <summary>
    /// Método para generar un reporte de compatibilidad entre vehículos y repuestos en formato .dot.
    /// </summary>
    public void GeneraReporteCompatibilidad()
    {
        string dotFilePath = $"{GetReportPath()}/compatibilidad_{++_contCvr}.dot";
        string outputFilePath = $"{GetReportPath()}/compatibilidad_{_contCvr}.png";
        string dotContent = Estructuras.Grafo.GenerarDot();
        GenerarDotImg(dotFilePath, outputFilePath, dotContent);
    }
    
    /// <summary>
    /// Genera una imagen a partir de un archivo .dot.
    /// </summary>
    public void GenerarDotImg(string dotFilePath, string outputImagePath, string dotContent)
    {
        try
        {
            File.WriteAllText(dotFilePath,dotContent);
            Process process = new Process();
            process.StartInfo.FileName = "dot";
            process.StartInfo.Arguments = $"-Tpng \"{dotFilePath}\" -o \"{outputImagePath}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}