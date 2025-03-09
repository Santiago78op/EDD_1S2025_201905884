using AutoGestPro.Core.Global;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;
using Gtk;

namespace AutoGestPro.UI.Admin;

public class GestionEntidadesWindow : Window
{
    private ComboBox _comboEntidades;
    private TreeView _treeView;
    private ListStore _listStore;
    private Button _btnEliminar;
    private LinkedList _clienteService;
    private DoubleList _vehiculoService;

    public GestionEntidadesWindow() : base("Gestión de Entidades")
    {
        SetDefaultSize(600, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Destroy(); };

        _clienteService = Estructuras.Clientes;
        _vehiculoService = Estructuras.Vehiculos;

        VBox vbox = new VBox();

        Label lblSeleccion = new Label("Seleccione la entidad a gestionar:");
        vbox.PackStart(lblSeleccion, false, false, 5);

        _comboEntidades = new ComboBox(new string[] { "Usuarios", "Vehículos" });
        _comboEntidades.Changed += OnEntidadSeleccionada;
        vbox.PackStart(_comboEntidades, false, false, 5);

        _treeView = new TreeView();
        vbox.PackStart(_treeView, true, true, 5);

        _btnEliminar = new Button("Eliminar Seleccionado");
        _btnEliminar.Clicked += OnEliminarClicked;
        vbox.PackStart(_btnEliminar, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private void OnEntidadSeleccionada(object? sender, EventArgs e)
    {
        TreeIter iter;
        if (_comboEntidades.GetActiveIter(out iter))
        {
            string entidadSeleccionada = (string)_comboEntidades.Model.GetValue(iter, 0);
            if (_listStore != null)
            {
                _listStore.Clear();
            }

            CargarDatos(entidadSeleccionada);
        }
    }

    private void CargarDatos(string entidad)
    {
        if (entidad == "Usuarios")
        {
            _listStore = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
            _treeView.Model = _listStore;

            _treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            _treeView.AppendColumn("Nombres", new CellRendererText(), "text", 1);
            _treeView.AppendColumn("Apellidos", new CellRendererText(), "text", 2);
            _treeView.AppendColumn("Correo", new CellRendererText(), "text", 3);

            _listStore.Clear();

            NodeLinked? usuarios = _clienteService.Head;

            while (usuarios != null)
            {
                Cliente cliente = (Cliente)usuarios.Data;
                _listStore.AppendValues(cliente.Id, cliente.Nombres, cliente.Apellidos, cliente.Correo);
                usuarios = usuarios.Next;
            }
        }
        else if (entidad == "Vehículos")
        {
            _listStore = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
            _treeView.Model = _listStore;

            _treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            _treeView.AppendColumn("Marca", new CellRendererText(), "text", 1);
            _treeView.AppendColumn("Modelo", new CellRendererText(), "text", 2);
            _treeView.AppendColumn("Placa", new CellRendererText(), "text", 3);

            _listStore.Clear();
            
            NodeDouble? vehiculos = _vehiculoService.Head;
            
            while (vehiculos != null)
            {
                Vehiculo vehiculo = (Vehiculo)vehiculos.Data;
                _listStore.AppendValues(vehiculo.Id, vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
                vehiculos = vehiculos.Next;
            }
        }
    }

private void OnEliminarClicked(object sender, EventArgs e)
{
    TreeIter iter;

    if (_treeView.Selection.GetSelected(out iter))
    {
        int idSeleccionado = (int)_treeView.Model.GetValue(iter, 0);
        string entidadSeleccionada = (string)_comboEntidades.Model.GetValue(iter, 0);

        if (entidadSeleccionada == "Usuarios")
        {
            if (_clienteService.DeleteNode(idSeleccionado))
            {
                Console.WriteLine("Usuario eliminado correctamente");
            }
            else
            {
                Console.WriteLine("No se pudo eliminar el usuario");
            }
        }
        else if (entidadSeleccionada == "Vehículos")
        {
            if (_vehiculoService.DeleteNode(idSeleccionado))
            {
                Console.WriteLine("Vehículo eliminado correctamente");
            }
            else
            {
                Console.WriteLine("No se pudo eliminar el vehículo");
            }
        }

        CargarDatos(entidadSeleccionada);
    }
}
}