using AutoGestPro.Core.Models;
using Gtk;

namespace AutoGestPro.UI.User;

public class VisualizarServicios : Window
{
    private TreeView _treeView;
    private ListStore _listStore;
    private ComboBoxText _comboOrden;
    private Button _btnActualizar;
    private Cliente _clienteActual;

    public VisualizarServicios(Cliente cliente) : base("Servicios Realizados")
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
        
        // TreeView para mostrar los servicios
        _treeView = new TreeView();
        CrearColumnasServicios();
        RefresacarServicios();
        
        ScrolledWindow scrollWindow = new ScrolledWindow();
        scrollWindow.Add(_treeView);

        vbox.PackStart(scrollWindow, true, true, 5);
        Add(vbox);
        ShowAll();
    }

    private void CrearColumnasServicios()
    {
        _listStore = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
        _treeView.Model = _listStore;

        _treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
        _treeView.AppendColumn("Repuesto", new CellRendererText(), "text", 1);
        _treeView.AppendColumn("Vehículo", new CellRendererText(), "text", 2);
        _treeView.AppendColumn("Detalles", new CellRendererText(), "text", 3);
        _treeView.AppendColumn("Costo", new CellRendererText(), "text", 4);
    }
    
    // ✅ Método para actualizar la visualización de los servicios según el orden seleccionado
    private void RefresacarServicios()
    {
        _listStore.Clear();
        string ordenSeleccionado = _comboOrden.ActiveText;
        switch (ordenSeleccionado)
        {
            case "Pre-orden":
                // 🔥 Recorrer el árbol binario de servicios, mostrar los datos en el TreeView
                foreach (var servicio in _clienteActual.Servicios.PreOrder())
                {
                    if (servicio != null)
                    {
                        Servicio s = (Servicio)servicio;
                        _listStore.AppendValues(s.Id, s.IdRepuesto, s.IdVehiculo, s.Detalles, s.Costo.ToString());
                    }
                }
                break;
            case "In-orden":
                // 🔥 Recorrer el árbol binario de servicios, mostrar los datos en el TreeView
                foreach (var servicio in _clienteActual.Servicios.InOrder())
                {
                    if (servicio != null)
                    {
                        Servicio s = (Servicio)servicio;
                        _listStore.AppendValues(s.Id, s.IdRepuesto, s.IdVehiculo, s.Detalles, s.Costo.ToString());
                    }
                }
                break;
            case "Post-orden":
                // 🔥 Recorrer el árbol binario de servicios, mostrar los datos en el TreeView
                foreach (var servicio in _clienteActual.Servicios.PostOrder())
                {
                    if (servicio != null)
                    {
                        Servicio s = (Servicio)servicio;
                        _listStore.AppendValues(s.Id, s.IdRepuesto, s.IdVehiculo, s.Detalles, s.Costo.ToString());
                    }
                }
                break;
        }
    }
    
    // ✅ Evento para actualizar la visualización al hacer clic en el botón
    private void OnActualizarClicked(object? sender, EventArgs e)
    {
        RefresacarServicios();
    }
}