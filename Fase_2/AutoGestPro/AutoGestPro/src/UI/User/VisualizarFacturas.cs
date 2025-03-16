using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using Gtk;

namespace AutoGestPro.UI.User;

public class VisualizarFacturas : Window
{
    private TreeView _treeView;
    private ListStore _listStore;
    private ComboBoxText _comboOrden;
    private Button _btnActualizar;
    private Button _btnPagarFactura;
    private Cliente _clienteActual;

    public VisualizarFacturas(Cliente cliente) : base("Facturas Pendientes")
    {
        _clienteActual = cliente;

        SetDefaultSize(600, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide();

        VBox vbox = new VBox(false, 5);

       // ComboBox para seleccionar el orden de visualización
        HBox hboxOrden = new HBox(false, 5);
        Label lblOrden = new Label("Selecciona el orden:");
        _comboOrden = new ComboBoxText();
        _comboOrden.AppendText("Pre-orden");
        _comboOrden.AppendText("In-orden");
        _comboOrden.AppendText("Post-orden");
        _comboOrden.Active = 1; // In-orden por defecto
        hboxOrden.PackStart(lblOrden, false, false, 5);
        hboxOrden.PackStart(_comboOrden, false, false, 5);
        vbox.PackStart(hboxOrden, false, false, 5);
        
        // Botón para actualizar la visualización
        _btnActualizar = new Button("🔄 Actualizar Vista");
        _btnActualizar.Clicked += OnActualizarClicked;
        vbox.PackStart(_btnActualizar, false, false, 5);
        
        // TreeView para mostrar las facturas
        _treeView = new TreeView();
        CrearColumnasFacturas();
        RefresacarFacturas();
        
        ScrolledWindow scrollWindow = new ScrolledWindow();
        scrollWindow.Add(_treeView);

        vbox.PackStart(scrollWindow, true, true, 5);
        Add(vbox);
        ShowAll();
    }

    private void CrearColumnasFacturas()
    {
        _listStore = new ListStore(typeof(int), typeof(int), typeof(string));
        _treeView.Model = _listStore;

        _treeView.AppendColumn("ID Factura", new CellRendererText(), "text", 0);
        _treeView.AppendColumn("ID Servicio", new CellRendererText(), "text", 1);
        _treeView.AppendColumn("Total", new CellRendererText(), "text", 2);
    }

    // ✅ Método para actualizar la visualización de los servicios según el orden seleccionado
    private void RefresacarFacturas()
    {
        _listStore.Clear();
        string ordenSeleccionado = _comboOrden.ActiveText;
        switch (ordenSeleccionado)
        {
            case "Pre-orden":
                _clienteActual.Facturas.PreOrder().ForEach(factura =>
                {
                    Factura f = (Factura)factura;
                    _listStore.AppendValues(f.Id, f.IdServicio, f.Total);
                });
                break;
            case "In-orden":
                _clienteActual.Facturas.InOrder().ForEach(factura =>
                {
                    Factura f = (Factura)factura;
                    _listStore.AppendValues(f.Id, f.IdServicio, f.Total);
                });
                break;
            case "Post-orden":
                _clienteActual.Facturas.PostOrder().ForEach(factura =>
                {
                    Factura f = (Factura)factura;
                    _listStore.AppendValues(f.Id, f.IdServicio, f.Total);
                });
                break;
        }
    }

    // 📌 Muestra un mensaje en pantalla
    private void OnActualizarClicked(object? sender, EventArgs e)
    {
        RefresacarFacturas();
    }
}