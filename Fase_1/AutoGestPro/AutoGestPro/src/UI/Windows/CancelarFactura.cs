using AutoGestPro.Core.Models;
using AutoGestPro.Core.Services;
using Gtk;

namespace AutoGestPro.UI.Windows;

public class CancelarFactura : Window
{
        private FacturaService facturaService;
        private Label lblFacturaInfo;
        private Button btnCancelarFactura;

        public CancelarFactura() : base("Cancelar Factura")
        {
            facturaService = new FacturaService();
            SetDefaultSize(400, 200);
            SetPosition(WindowPosition.Center);

            VBox vbox = new VBox(false, 10);
            Add(vbox);

            Label lblTitulo = new Label("<b>Factura Pendiente</b>") { UseMarkup = true };
            vbox.PackStart(lblTitulo, false, false, 5);

            lblFacturaInfo = new Label("No hay facturas pendientes.");
            vbox.PackStart(lblFacturaInfo, false, false, 5);

            btnCancelarFactura = new Button("Confirmar Pago");
            btnCancelarFactura.Clicked += OnCancelarFacturaClicked;
            btnCancelarFactura.Sensitive = false;
            vbox.PackStart(btnCancelarFactura, false, false, 5);

            MostrarUltimaFactura();

            ShowAll();
        }

        private void MostrarUltimaFactura()
        {
            Factura factura = facturaService.ObtenerUltimaFactura();

            if (factura != null)
            {
                lblFacturaInfo.Text = $"Factura ID: {factura.Id}\nOrden: {factura.IdOrden}\nTotal: Q{factura.Total}";
                btnCancelarFactura.Sensitive = true;
            }
            else
            {
                lblFacturaInfo.Text = "No hay facturas pendientes.";
                btnCancelarFactura.Sensitive = false;
            }
        }

        private void OnCancelarFacturaClicked(object sender, EventArgs e)
        {
            Factura factura = facturaService.RetirarFactura();

            if (factura != null)
            {
                MessageDialog confirmDialog = new MessageDialog(
                    this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok,
                    $"✅ Factura ID {factura.Id} cancelada con éxito.\nTotal pagado: Q{factura.Total}"
                );
                confirmDialog.Run();
                confirmDialog.Destroy();
            }
            else
            {
                MessageDialog errorDialog = new MessageDialog(
                    this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,
                    "❌ No hay facturas pendientes."
                );
                errorDialog.Run();
                errorDialog.Destroy();
            }

            MostrarUltimaFactura();
        }
}