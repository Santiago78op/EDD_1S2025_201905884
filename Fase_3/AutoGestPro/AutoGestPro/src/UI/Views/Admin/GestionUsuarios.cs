using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.Admin;

/// <summary>
/// Ventana para la inserción de nuevos usuarios en el sistema.
/// </summary>
public class GestionUsuariosInsercion : Window
{
    #region Private Fields

    private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/style.css";
    private const string WINDOW_TITLE = "Inserción de Usuario - Administrador";

    private readonly ServicioUsuarios _servicioUsuarios = Estructuras.Clientes;

    private Entry _txtIdUsuario;
    private Entry _txtNombres;
    private Entry _txtApellidos;
    private Entry _txtCorreo;
    private Entry _txtEdad;
    private Entry _txtContrasenia;
    private Entry _txtConfirmarContrasenia;
    private Button _btnGuardar;
    private Button _btnCancelar;
    private Label _lblStatus;
    private TreeView _treeView;
    private ListStore _listStore;

    private CssProvider _cssProvider;
    private Box _mainBox;

    #endregion

    #region Constructor

    public GestionUsuariosInsercion() : base(WINDOW_TITLE)
    {
        InitializeWindow();
        InitializeCSS();
        CreateUI();
        LoadUsuarios();
        ShowAll();
    }

    #endregion

    #region Initialization Methods

    private void InitializeWindow()
    {
        SetDefaultSize(900, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) =>
        {
            args.RetVal = true; 
            Destroy();
        };
    }

    private void InitializeCSS()
    {
        _cssProvider = new CssProvider();

        try
        {
            var cssPathGeneral = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CSS_FILE_PATH);
            if (File.Exists(cssPathGeneral))
            {
                _cssProvider.LoadFromPath(cssPathGeneral);
                StyleContext.AddProviderForScreen(Gdk.Screen.Default, _cssProvider, 800);
            }
            else
            {
                Console.WriteLine($"Archivo CSS no encontrado en: {cssPathGeneral}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar CSS: {ex.Message}");
        }
    }

    #endregion

    #region UI Creation

    private void CreateUI()
    {
        _mainBox = new Box(Orientation.Vertical, 0);
        _mainBox.StyleContext.AddClass("main-container");

        CreateHeader();
        CreateContentArea();

        Add(_mainBox);
    }

    private void CreateHeader()
    {
        var headerBar = new Box(Orientation.Horizontal, 8);
        headerBar.StyleContext.AddClass("header-bar");

        var logo = new Label("👥");
        logo.SetSizeRequest(40, 40);

        var titleLabel = new Label("Inserción de Usuario");
        titleLabel.AddCssClass("label-titulo");

        headerBar.PackStart(logo, false, false, 8);
        headerBar.PackStart(titleLabel, false, false, 8);

        _mainBox.PackStart(headerBar, false, false, 0);
    }

    private void CreateContentArea()
    {
        var contentBox = new Box(Orientation.Horizontal, 16) { Margin = 20 };
        contentBox.StyleContext.AddClass("form-panel");

        var leftBox = new Box(Orientation.Vertical, 16);
        var rightBox = new Box(Orientation.Vertical, 16);

        var formTitle = new Label("Datos del Nuevo Usuario");
        formTitle.AddCssClass("label-titulo");
        formTitle.Xalign = 0;
        leftBox.PackStart(formTitle, false, false, 10);

        CreateForm(leftBox);
        CreateTable(rightBox);

        contentBox.PackStart(leftBox, true, true, 0);
        contentBox.PackStart(rightBox, true, true, 0);

        _mainBox.PackStart(contentBox, true, true, 0);
    }

    private void CreateForm(Box container)
    {
        var idUsuario = CreateFormField("ID Usuario:", out _txtIdUsuario);
        container.PackStart(idUsuario, false, false, 8);
        
        var nombresBox = CreateFormField("Nombres:", out _txtNombres);
        container.PackStart(nombresBox, false, false, 8);

        var apellidosBox = CreateFormField("Apellidos:", out _txtApellidos);
        container.PackStart(apellidosBox, false, false, 8);

        var correoBox = CreateFormField("Correo electrónico:", out _txtCorreo);
        container.PackStart(correoBox, false, false, 8);

        var edadBox = CreateFormField("Edad:", out _txtEdad);
        container.PackStart(edadBox, false, false, 8);

        var contraseniaBox = CreateFormField("Contraseña:", out _txtContrasenia);
        _txtContrasenia.Visibility = false;
        container.PackStart(contraseniaBox, false, false, 8);

        var botonesBox = new Box(Orientation.Horizontal, 8);

        _btnGuardar = new Button("Guardar");
        _btnGuardar.AddCssClass("boton");
        _btnGuardar.Clicked += OnGuardarClicked;

        _btnCancelar = new Button("Cancelar");
        _btnCancelar.AddCssClass("boton-admin");
        _btnCancelar.Clicked += OnCancelarClicked;

        botonesBox.PackStart(_btnGuardar, true, true, 0);
        botonesBox.PackStart(_btnCancelar, true, true, 0);

        container.PackStart(botonesBox, false, false, 16);

        _lblStatus = new Label("");
        _lblStatus.AddCssClass("label-status");
        _lblStatus.Xalign = 0;
        container.PackStart(_lblStatus, false, false, 8);
    }

    private void CreateTable(Box container)
    {
        _listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

        _treeView = new TreeView(_listStore)
        {
            HeadersVisible = true
        };

        _treeView.AppendColumn("ID Usuario", new CellRendererText(), "text", 0);
        _treeView.AppendColumn("Nombres", new CellRendererText(), "text", 1);
        _treeView.AppendColumn("Apellidos", new CellRendererText(), "text", 2);
        _treeView.AppendColumn("Correo", new CellRendererText(), "text", 3);
        _treeView.AppendColumn("Edad", new CellRendererText(), "text", 4);
        _treeView.AppendColumn("Contraseña", new CellRendererText(), "text", 5);

        var scroll = new ScrolledWindow();
        scroll.Add(_treeView);
        scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        scroll.SetSizeRequest(500, 400);

        container.PackStart(scroll, true, true, 8);
    }

    private Box CreateFormField(string labelText, out Entry entry)
    {
        var box = new Box(Orientation.Vertical, 4);

        var label = new Label(labelText);
        label.Xalign = 0;
        label.AddCssClass("form-label");

        entry = new Entry();
        entry.AddCssClass("entry");

        box.PackStart(label, false, false, 0);
        box.PackStart(entry, false, false, 0);

        return box;
    }

    #endregion

    #region Event Handlers

    private void OnGuardarClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_txtIdUsuario.Text) ||
                string.IsNullOrWhiteSpace(_txtNombres.Text) ||
                string.IsNullOrWhiteSpace(_txtApellidos.Text) ||
                string.IsNullOrWhiteSpace(_txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(_txtEdad.Text) ||
                string.IsNullOrWhiteSpace(_txtContrasenia.Text))
            {
                UpdateStatus("Todos los campos son obligatorios", true);
                return;
            }

            if (!int.TryParse(_txtEdad.Text, out int edad) || edad <= 0 || edad > 120)
            {
                UpdateStatus("La edad debe ser un número válido entre 1 y 120", true);
                return;
            }

            if (!_txtCorreo.Text.Contains("@") || !_txtCorreo.Text.Contains("."))
            {
                UpdateStatus("El formato del correo electrónico no es válido", true);
                return;
            }

            var usuario = new Usuario(
                int.Parse(_txtIdUsuario.Text.Trim()),
                _txtNombres.Text.Trim(),
                _txtApellidos.Text.Trim(),
                _txtCorreo.Text.Trim(),
                edad,
                _txtContrasenia.Text.Trim()
            );

            var usuarioRegistrado = _servicioUsuarios.RegistrarUsuario(
                usuario.Id,
                usuario.Nombres,
                usuario.Apellidos,
                usuario.Correo,
                usuario.Edad,
                usuario.ContraseniaHash
            );

            if (usuarioRegistrado != null)
            {
                UpdateStatus("¡Usuario guardado correctamente!", false);
                _listStore.AppendValues(
                    usuarioRegistrado.Id.ToString(),
                    usuario.Nombres,
                    usuario.Apellidos,
                    usuario.Correo,
                    usuario.Edad.ToString(),
                    usuario.ContraseniaHash
                );
                LimpiarFormulario();
            }
            else
            {
                UpdateStatus("Error al guardar el usuario. El correo o ID ya existe o hay un problema con los datos", true);
            }
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}", true);
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();
    }

    #endregion

    #region Helper Methods

    private void LoadUsuarios()
    {
        _listStore.Clear();
    }

    private void LimpiarFormulario()
    {
        _txtIdUsuario.Text = "";
        _txtNombres.Text = "";
        _txtApellidos.Text = "";
        _txtCorreo.Text = "";
        _txtEdad.Text = "";
        _txtContrasenia.Text = "";

        UpdateStatus("", false);
    }

    private void UpdateStatus(string message, bool isError)
    {
        _lblStatus.Text = message;

        _lblStatus.StyleContext.RemoveClass("status-error");
        _lblStatus.StyleContext.RemoveClass("status-success");

        if (isError)
        {
            _lblStatus.StyleContext.AddClass("status-error");
        }
        else if (!string.IsNullOrEmpty(message))
        {
            _lblStatus.StyleContext.AddClass("status-success");
        }
    }

    #endregion
}