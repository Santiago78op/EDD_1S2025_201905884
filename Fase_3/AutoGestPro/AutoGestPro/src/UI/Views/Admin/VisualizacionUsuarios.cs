using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
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

    private Entry _txtBuscarId;
    private Button _btnBuscar;
    private Label _lblResultado;

    public VisualizacionUsuarios() : base(WINDOW_TITLE)
    {
        InitializeWindow();
        CreateUI();
        ShowAll();
    }

    private void InitializeWindow()
    {
        SetDefaultSize(500, 300);
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

        mainBox.PackStart(searchBox, false, false, 0);
        mainBox.PackStart(_lblResultado, false, false, 16);

        Add(mainBox);
    }

    private void OnBuscarClicked(object sender, EventArgs e)
    {
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
        }
        else
        {
            _lblResultado.Text = $"No se encontró ningún usuario con ID {id}.";
        }
    }
}
