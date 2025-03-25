using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using Gtk;

namespace AutoGestPro.UI.User;

public class CancelarFacturas : Window
{
    private Entry _entryIdFactura;
    private Button _btnPagar;
    private Cliente _clienteActual;

    public CancelarFacturas(Cliente cliente) : base("Cancelar Facturas")
    {
        _clienteActual = cliente;

        SetDefaultSize(400, 200);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide();

        VBox vbox = new VBox(false, 10);
        Label lblIdFactura = new Label("Ingrese el ID de la factura:");

        _entryIdFactura = new Entry { PlaceholderText = "ID Factura" };

        _btnPagar = new Button("Pagar Factura");
        _btnPagar.Clicked += OnPagarFacturaClicked;

        vbox.PackStart(lblIdFactura, false, false, 5);
        vbox.PackStart(_entryIdFactura, false, false, 5);
        vbox.PackStart(_btnPagar, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private void OnPagarFacturaClicked(object sender, EventArgs e)
    {
        int idFactura;
        if (!int.TryParse(_entryIdFactura.Text, out idFactura))
        {
            MostrarMensaje("Error", "Ingrese un ID de factura válido.");
            return;
        }

        // 🔥 Buscar la factura en la lista del usuario
        var factura = _clienteActual.Facturas.Search(idFactura);
        if (factura != null)
        {
            _clienteActual.Facturas.Delete(idFactura); // Se elimina la factura al pagar
            Estructuras.Facturas.Delete(idFactura);
            MostrarMensaje("Pago Exitoso", $"La factura {idFactura} ha sido cancelada y eliminada.");
        }
        else
        {
            MostrarMensaje("Error", "Factura no encontrada.");
        }
    }

    // 📌 Muestra un mensaje en pantalla
    private void MostrarMensaje(string titulo, string mensaje)
    {
        MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
        dialog.Title = titulo;
        dialog.Run();
        dialog.Destroy();
    }
}