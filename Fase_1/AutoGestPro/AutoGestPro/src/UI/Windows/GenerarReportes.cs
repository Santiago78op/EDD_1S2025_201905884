using AutoGestPro.Core.Services;
using Gtk;

namespace AutoGestPro.UI.Windows;

public class GenerarReportes : Window
{
        private ReporteService reporteService;
        private ComboBoxText comboEntidades;
        private Button btnGenerarReporte;
        private Button btnTopVehiculos;
        private Image imagenReporte;

        public GenerarReportes() : base("Generar Reportes con Graphviz")
        {
            reporteService = new ReporteService();
            SetDefaultSize(600, 500);
            SetPosition(WindowPosition.Center);

            VBox vbox = new VBox(false, 10);
            Add(vbox);

            Label lblTitulo = new Label("<b>Seleccione el reporte a generar</b>") { UseMarkup = true };
            vbox.PackStart(lblTitulo, false, false, 5);

            comboEntidades = new ComboBoxText();
            comboEntidades.AppendText("Usuarios");
            comboEntidades.AppendText("Vehículos");
            comboEntidades.AppendText("Repuestos");
            comboEntidades.AppendText("Servicios");
            comboEntidades.AppendText("Facturación");
            comboEntidades.Active = 0;

            vbox.PackStart(comboEntidades, false, false, 5);
            

            btnGenerarReporte = new Button("Generar Reporte");
            btnGenerarReporte.Clicked += OnGenerarReporteClicked;
            vbox.PackStart(btnGenerarReporte, false, false, 5);
            
            btnTopVehiculos = new Button("Top 5 Vehículos");
            btnTopVehiculos.Clicked += OnTopVehiculosClicked;
            vbox.PackStart(btnTopVehiculos, false, false, 5);
            
            imagenReporte = new Image();
            vbox.PackStart(imagenReporte, true, true, 5);

            ShowAll();
        }

        private void OnGenerarReporteClicked(object sender, EventArgs e)
        {
            string entidad = comboEntidades.ActiveText;
            string rutaSalida = $"reporte_{entidad}.png";

            if (reporteService.GenerarReporteGraphviz(entidad, rutaSalida))
            {
                FileChooserDialog fileChooser = new FileChooserDialog(
                    "Guardar reporte como...",
                    this,
                    FileChooserAction.Save,
                    "Cancelar", ResponseType.Cancel,
                    "Guardar", ResponseType.Accept
                );

                fileChooser.CurrentName = rutaSalida;

                if (fileChooser.Run() == (int)ResponseType.Accept)
                {
                    File.Copy(rutaSalida, fileChooser.Filename, true);
                    MostrarMensaje("Éxito", $"Reporte guardado correctamente: {fileChooser.Filename}");
                }
                else
                {
                    MostrarMensaje("Error", "Operación cancelada por el usuario.");
                }

                fileChooser.Destroy();
            }
            else
            {
                MostrarMensaje("Error", $"No hay datos suficientes para generar el reporte de {entidad}.");
            }
        }
        
        private void OnTopVehiculosClicked(object sender, EventArgs e)
        {
            
            var topVehiculosServicios = reporteService.GetTopVehiculosConMasServicios(5);
            var topVehiculosAntiguos = reporteService.GetTopVehiculosMasAntiguos(5);

            string mensaje = "Top 5 Vehículos con más servicios:\n";
            foreach (var vehiculo in topVehiculosServicios)
            {
                mensaje += $"ID: {vehiculo.Id}, Marca: {vehiculo.Marca}, Modelo: {vehiculo.Modelo}, Servicios: {vehiculo.Servicios.Count}\n";
            }

            mensaje += "\nTop 5 Vehículos más antiguos:\n";
            foreach (var vehiculo in topVehiculosAntiguos)
            {
                mensaje += $"ID: {vehiculo.Id}, Marca: {vehiculo.Marca}, Modelo: {vehiculo.Modelo}, Año: {vehiculo.Año}\n";
            }
            
            MostrarMensaje("Top 5 Vehículos", "");
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
            dialog.Title = titulo;
            dialog.Run();
            dialog.Destroy();
        }
}