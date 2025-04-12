using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.Admin;

/// <summary>
/// Ventana para la visualización de usuarios por ID.
/// </summary>
public class VisualizacionUsuarios : Window
{
    private const string WINDOW_TITLE = "Visualización de Usuario por ID";

    private readonly ServicioUsuarios _servicioUsuarios = Estructuras.Clientes;
    private readonly DoubleList _servicioVehiculos = Estructuras.Vehiculos;

    private Entry _txtBuscarId;
    private Button _btnBuscar;
    private Label _lblResultado;
    private TreeView _treeViewVehiculos;
    private ListStore _listStoreVehiculos;

    public VisualizacionUsuarios() : base(WINDOW_TITLE)
    {
        InitializeWindow();
        CreateUI();
        ShowAll();
    }

    private void InitializeWindow()
    {
        SetDefaultSize(700, 500);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) =>
        {
            args.RetVal = false;
            this.Destroy();
        };
    }

    private void CreateUI()
    {
        var mainBox = new Box(Orientation.Vertical, 16) { Margin = 20 };

        var title = new Label("Buscar Usuario por ID");
        title.Xalign = 0;
        title.StyleContext.AddClass("label-titulo");
        mainBox.PackStart(title, false, false, 0);

        var searchBox = new Box(Orientation.Horizontal, 8);
        _txtBuscarId = new Entry { PlaceholderText = "Ingrese ID" };
        _btnBuscar = new Button("Buscar");
        _btnBuscar.Clicked += OnBuscarClicked;

        searchBox.PackStart(_txtBuscarId, true, true, 0);
        searchBox.PackStart(_btnBuscar, false, false, 0);

        _lblResultado = new Label("");
        _lblResultado.Xalign = 0;

        _listStoreVehiculos = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));
        _treeViewVehiculos = new TreeView(_listStoreVehiculos)
        {
            HeadersVisible = true
        };

        _treeViewVehiculos.AppendColumn("Id", new CellRendererText(), "text", 0);
        _treeViewVehiculos.AppendColumn("Id Usuario", new CellRendererText(), "text", 1);
        _treeViewVehiculos.AppendColumn("Marca", new CellRendererText(), "text", 2);
        _treeViewVehiculos.AppendColumn("Modelo", new CellRendererText(), "text", 3);
        _treeViewVehiculos.AppendColumn("Placa", new CellRendererText(), "text", 4);

        var scroll = new ScrolledWindow();
        scroll.Add(_treeViewVehiculos);
        scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        scroll.SetSizeRequest(-1, 250);

        mainBox.PackStart(searchBox, false, false, 0);
        mainBox.PackStart(_lblResultado, false, false, 16);
        mainBox.PackStart(scroll, true, true, 0);

        Add(mainBox);
    }

    private void OnBuscarClicked(object sender, EventArgs e)
    {
        _listStoreVehiculos.Clear();

        if (!int.TryParse(_txtBuscarId.Text, out int id) || id <= 0)
        {
            _lblResultado.Text = "Por favor, ingrese un ID válido.";
            return;
        }

        var usuario = _servicioUsuarios.BuscarUsuarioPorId(id);

        if (usuario != null)
        {
            _lblResultado.Text =
                $"ID: {usuario.Id}\n" +
                $"Nombre: {usuario.Nombres} {usuario.Apellidos}\n" +
                $"Correo: {usuario.Correo}\n" +
                $"Edad: {usuario.Edad}";

            var vehiculos = _servicioVehiculos.SearchNode(id);

            if (vehiculos != null)
            {
                NodeDouble? current = Estructuras.Vehiculos.Head;

                while (current != null)
                {
                    Vehiculo vehiculo = (Vehiculo)current.Data;
                    if (vehiculo.IdUsuario == id)
                    {
                        _listStoreVehiculos.AppendValues(vehiculo.Id.ToString(), vehiculo.IdUsuario.ToString(), vehiculo.Marca, vehiculo.Modelo.ToString(), vehiculo.Placa);
                    }
                    current = current.Next;
                }
            }
        }
        else
        {
            _lblResultado.Text = $"No se encontró ningún usuario con ID {id}.";
        }
    }
}

