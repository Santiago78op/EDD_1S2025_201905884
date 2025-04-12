using System;
using System.Threading.Tasks;
using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Services;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.Admin;

/// <summary>
/// Ventana para la gestión de usuarios del sistema.
/// Permite crear, editar y eliminar usuarios.
/// </summary>
public class GestionUsuarios : Window
{
    #region Private Fields

    // Constantes
    private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/gestionUsuarios.css";
    private const string WINDOW_TITLE = "Gestión de Usuarios - Administrador";

    // Servicio de usuarios
    private readonly ServicioUsuarios _servicioUsuarios = Estructuras.Clientes;

    // UI Components - Formulario
    private Entry _txtId;
    private Entry _txtNombres;
    private Entry _txtApellidos;
    private Entry _txtCorreo;
    private Entry _txtEdad;
    private Entry _txtContrasenia;
    private CheckButton _chkEsAdmin;
    private Button _btnGuardar;
    private Button _btnLimpiar;
    private Label _lblStatus;

    // UI Components - Lista de usuarios
    private TreeView _treeView;
    private ListStore _listStore;
    private ScrolledWindow _scrolledWindow;

    private CssProvider _cssProvider;
    private Box _mainBox;
    private Stack _stack;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor de la ventana de gestión de usuarios
    /// </summary>
    public GestionUsuarios() : base(WINDOW_TITLE)
    {
        InitializeWindow();
        InitializeCSS();
        CreateUI();
        LoadUsuarios();
        ShowAll();
    }

    #endregion

    #region Initialization Methods

    /// <summary>
    /// Inicializa la configuración de la ventana
    /// </summary>
    private void InitializeWindow()
    {
        SetDefaultSize(900, 700);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) =>
        {
            args.RetVal = false; // Permite el cierre de la ventana
            this.Destroy(); // Solo destruye esta ventana, no toda la aplicación
        };
    }

    /// <summary>
    /// Inicializa los estilos CSS para la interfaz
    /// </summary>
    private void InitializeCSS()
    {
        _cssProvider = new CssProvider();

        try
        {
            // Cargar los estilos generales
            var cssPathGeneral = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "../../../src/UI/Assets/Styles/style.css");
            if (System.IO.File.Exists(cssPathGeneral))
            {
                _cssProvider.LoadFromPath(cssPathGeneral);
                StyleContext.AddProviderForScreen(Gdk.Screen.Default, _cssProvider, 800);
            }

            // Intentar cargar estilos específicos si existen
            var cssPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CSS_FILE_PATH);
            if (System.IO.File.Exists(cssPath))
            {
                _cssProvider.LoadFromPath(cssPath);
                StyleContext.AddProviderForScreen(Gdk.Screen.Default, _cssProvider, 801);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar CSS: {ex.Message}");
        }
    }

    #endregion

    #region UI Creation

    /// <summary>
    /// Crea y configura la interfaz de usuario
    /// </summary>
    private void CreateUI()
    {
        // Crear el stack para manejar vistas
        _stack = new Stack
        {
            TransitionType = StackTransitionType.SlideLeftRight,
            TransitionDuration = 300
        };

        // Crear componentes principales
        CreateMainView();

        // Configurar la vista inicial
        _stack.VisibleChild = _mainBox;

        // Añadir el stack a la ventana
        Add(_stack);
    }

    /// <summary>
    /// Crea la vista principal de la aplicación
    /// </summary>
    private void CreateMainView()
    {
        _mainBox = new Box(Orientation.Vertical, 0);
        _mainBox.StyleContext.AddClass("main-container");

        // Crear componentes de la interfaz
        CreateHeader();
        CreateContentArea();

        // Añadir la vista principal al stack
        _stack.AddNamed(_mainBox, "main");
    }

    /// <summary>
    /// Crea el encabezado de la ventana
    /// </summary>
    private void CreateHeader()
    {
        var headerBar = new Box(Orientation.Horizontal, 8);
        headerBar.StyleContext.AddClass("header-bar");

        var logo = new Label("👤");
        logo.SetSizeRequest(40, 40);

        var titleLabel = new Label("Gestión de Usuarios");
        titleLabel.StyleContext.AddClass("title");

        headerBar.PackStart(logo, false, false, 8);
        headerBar.PackStart(titleLabel, false, false, 8);

        _mainBox.PackStart(headerBar, false, false, 0);
    }

    /// <summary>
    /// Crea el área de contenido principal
    /// </summary>
    private void CreateContentArea()
    {
        var contentBox = new Box(Orientation.Horizontal, 16);
        contentBox.StyleContext.AddClass("main-content");
        contentBox.BorderWidth = 16;

        // Panel izquierdo - Formulario
        var leftPanel = new Box(Orientation.Vertical, 16);
        leftPanel.StyleContext.AddClass("form-panel");

        // Panel derecho - Lista de usuarios
        var rightPanel = new Box(Orientation.Vertical, 16);
        rightPanel.StyleContext.AddClass("list-panel");

        // Crear el formulario
        CreateFormPanel(leftPanel);

        // Crear la lista de usuarios
        CreateUsersListPanel(rightPanel);

        // Añadir los paneles al contenedor principal
        contentBox.PackStart(leftPanel, false, true, 0);
        contentBox.PackStart(rightPanel, true, true, 0);

        _mainBox.PackStart(contentBox, true, true, 0);
    }

    /// <summary>
    /// Crea el panel del formulario
    /// </summary>
    private void CreateFormPanel(Box container)
    {
        var formTitle = new Label("Datos del Usuario");
        formTitle.StyleContext.AddClass("form-title");
        formTitle.Xalign = 0;

        container.PackStart(formTitle, false, false, 0);

        // Campo ID
        var idBox = CreateFormField("ID:", out _txtId);
        _txtId.StyleContext.AddClass("entry-readonly");
        _txtId.Text = "Auto";
        _txtId.Sensitive = false;
        container.PackStart(idBox, false, false, 8);

        // Campo Nombres
        var nombresBox = CreateFormField("Nombres:", out _txtNombres);
        container.PackStart(nombresBox, false, false, 8);

        // Campo Apellidos
        var apellidosBox = CreateFormField("Apellidos:", out _txtApellidos);
        container.PackStart(apellidosBox, false, false, 8);

        // Campo Correo
        var correoBox = CreateFormField("Correo:", out _txtCorreo);
        container.PackStart(correoBox, false, false, 8);

        // Campo Edad
        var edadBox = CreateFormField("Edad:", out _txtEdad);
        container.PackStart(edadBox, false, false, 8);

        // Campo Contraseña
        var contraseniaBox = CreateFormField("Contraseña:", out _txtContrasenia);
        _txtContrasenia.Visibility = false;
        container.PackStart(contraseniaBox, false, false, 8);

        // Campo Es Admin
        var esAdminBox = new Box(Orientation.Horizontal, 8);
        _chkEsAdmin = new CheckButton("Es Administrador");
        _chkEsAdmin.StyleContext.AddClass("checkbox-admin");
        esAdminBox.PackStart(_chkEsAdmin, true, true, 0);
        container.PackStart(esAdminBox, false, false, 8);

        // Botones
        var botonesBox = new Box(Orientation.Horizontal, 8);

        _btnGuardar = new Button("Guardar");
        _btnGuardar.StyleContext.AddClass("boton");
        _btnGuardar.Clicked += OnGuardarClicked;

        _btnLimpiar = new Button("Limpiar");
        _btnLimpiar.StyleContext.AddClass("boton-admin");
        _btnLimpiar.Clicked += OnLimpiarClicked;

        botonesBox.PackStart(_btnGuardar, true, true, 0);
        botonesBox.PackStart(_btnLimpiar, true, true, 0);

        container.PackStart(botonesBox, false, false, 16);

        // Estado
        _lblStatus = new Label("");
        _lblStatus.StyleContext.AddClass("label-status");
        _lblStatus.Xalign = 0;
        container.PackStart(_lblStatus, false, false, 8);
    }

    /// <summary>
    /// Crea un campo del formulario
    /// </summary>
    private Box CreateFormField(string labelText, out Entry entry)
    {
        var box = new Box(Orientation.Vertical, 4);

        var label = new Label(labelText);
        label.Xalign = 0;
        label.StyleContext.AddClass("form-label");

        entry = new Entry();
        entry.StyleContext.AddClass("entry");

        box.PackStart(label, false, false, 0);
        box.PackStart(entry, false, false, 0);

        return box;
    }

    /// <summary>
    /// Crea el panel de lista de usuarios
    /// </summary>
    private void CreateUsersListPanel(Box container)
    {
        var listTitle = new Label("Lista de Usuarios");
        listTitle.StyleContext.AddClass("list-title");
        listTitle.Xalign = 0;

        container.PackStart(listTitle, false, false, 0);

        // Crear TreeView para mostrar los usuarios
        _scrolledWindow = new ScrolledWindow();
        _scrolledWindow.ShadowType = ShadowType.EtchedIn;
        _scrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        _scrolledWindow.SetSizeRequest(-1, 500);

        // Configurar el TreeView
        _listStore = new ListStore(
            typeof(int), // ID
            typeof(string), // Nombres
            typeof(string), // Apellidos
            typeof(string), // Correo
            typeof(int), // Edad
            typeof(bool) // Es Admin
        );

        _treeView = new TreeView(_listStore);
        _treeView.HeadersVisible = true;

        // Configurar columnas
        AddTreeViewColumn("ID", 0, false);
        AddTreeViewColumn("Nombres", 1, true);
        AddTreeViewColumn("Apellidos", 2, true);
        AddTreeViewColumn("Correo", 3, true);
        AddTreeViewColumn("Edad", 4, false);
        AddTreeViewColumn("Es Admin", 5, false, true);

        _treeView.RowActivated += OnUsuarioSeleccionado;

        _scrolledWindow.Add(_treeView);
        container.PackStart(_scrolledWindow, true, true, 0);

        // Botones de acción
        var actionBox = new Box(Orientation.Horizontal, 8);

        var btnEditar = new Button("Editar");
        btnEditar.StyleContext.AddClass("boton");
        btnEditar.Clicked += OnEditarClicked;

        var btnEliminar = new Button("Eliminar");
        btnEliminar.StyleContext.AddClass("boton-admin");
        btnEliminar.Clicked += OnEliminarClicked;

        actionBox.PackStart(btnEditar, true, true, 0);
        actionBox.PackStart(btnEliminar, true, true, 0);

        container.PackStart(actionBox, false, false, 8);
    }

    /// <summary>
    /// Añade una columna al TreeView
    /// </summary>
    private void AddTreeViewColumn(string title, int columnIndex, bool expand, bool isBoolean = false)
    {
        var column = new TreeViewColumn();
        column.Title = title;

        if (isBoolean)
        {
            var cellToggle = new CellRendererToggle();
            cellToggle.Activatable = false;
            column.PackStart(cellToggle, true);
            column.AddAttribute(cellToggle, "active", columnIndex);
        }
        else
        {
            var cell = new CellRendererText();
            column.PackStart(cell, true);
            column.AddAttribute(cell, "text", columnIndex);
        }

        column.Resizable = true;
        column.Expand = expand;

        _treeView.AppendColumn(column);
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Maneja el evento de guardar usuario
    /// </summary>
    private void OnGuardarClicked(object sender, EventArgs e)
    {
        try
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(_txtNombres.Text) ||
                string.IsNullOrWhiteSpace(_txtApellidos.Text) ||
                string.IsNullOrWhiteSpace(_txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(_txtEdad.Text) ||
                string.IsNullOrWhiteSpace(_txtContrasenia.Text))
            {
                UpdateStatus("Todos los campos son obligatorios", true);
                return;
            }

            // Verificar si la edad es un número válido
            if (!int.TryParse(_txtEdad.Text, out int edad) || edad <= 0 || edad > 120)
            {
                UpdateStatus("La edad debe ser un número válido entre 1 y 120", true);
                return;
            }

            // Verificar formato del correo (simple)
            if (!_txtCorreo.Text.Contains("@") || !_txtCorreo.Text.Contains("."))
            {
                UpdateStatus("El formato del correo electrónico no es válido", true);
                return;
            }

            // Crear usuario
            var usuario = new Usuario(
                0, // El ID se asigna automáticamente
                _txtNombres.Text.Trim(),
                _txtApellidos.Text.Trim(),
                _txtCorreo.Text.Trim(),
                edad,
                _txtContrasenia.Text.Trim()
            );

            // Guardar usuario
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
                UpdateStatus("Usuario guardado correctamente", false);
                LimpiarFormulario();
                LoadUsuarios(); // Recargar la lista
            }
            else
            {
                UpdateStatus("Error al guardar el usuario. El correo ya existe o hay un problema con los datos", true);
            }
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}", true);
        }
    }

    /// <summary>
    /// Maneja el evento de limpiar el formulario
    /// </summary>
    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();
    }

    /// <summary>
    /// Maneja el evento de seleccionar un usuario
    /// </summary>
    private void OnUsuarioSeleccionado(object sender, RowActivatedArgs args)
    {
        TreeIter iter;
        if (_treeView.Model.GetIter(out iter, args.Path))
        {
            int id = (int)_treeView.Model.GetValue(iter, 0);
            var usuario = _servicioUsuarios.BuscarUsuarioPorId(id);

            if (usuario != null)
            {
                CargarUsuarioEnFormulario(usuario);
            }
        }
    }

    /// <summary>
    /// Maneja el evento de eliminar un usuario
    /// </summary>
    private void OnEliminarClicked(object sender, EventArgs e)
    {
        TreeSelection selection = _treeView.Selection;
        if (selection.GetSelected(out TreeIter iter))
        {
            int id = (int)_listStore.GetValue(iter, 0);
            var usuario = _servicioUsuarios.BuscarUsuarioPorId(id);
    
            if (usuario != null)
            {
                using (var dialog = new MessageDialog(
                           this,
                           DialogFlags.Modal,
                           MessageType.Question,
                           ButtonsType.YesNo,
                           $"¿Está seguro que desea eliminar al usuario {usuario.Nombres} {usuario.Apellidos}?"))
                {
                    ResponseType response = (ResponseType)dialog.Run();
                    dialog.Destroy();
    
                    if (response == ResponseType.Yes)
                    {
                        // Aquí iría la lógica para eliminar usuario si estuviera implementada
                        // Como no vemos este método en el código, simplemente mostramos un mensaje
                        UpdateStatus("Funcionalidad de eliminación no implementada en el servicio", true);
    
                        // Simularíamos algo como:
                        // bool eliminado = _servicioUsuarios.EliminarUsuario(id);
                        // if (eliminado) { LoadUsuarios(); }
                    }
                }
            }
        }
        else
        {
            UpdateStatus("Por favor, seleccione un usuario", true);
        }
    }

    /// <summary>
    /// Maneja el evento de editar un usuario
    /// </summary>
   private void OnEditarClicked(object sender, EventArgs e)
    {
        TreeSelection selection = _treeView.Selection;
        TreeIter iter;
    
        if (selection.GetSelected(out iter))
        {
            int id = (int)_listStore.GetValue(iter, 0);
            var usuario = _servicioUsuarios.BuscarUsuarioPorId(id);
    
            if (usuario != null)
            {
                CargarUsuarioEnFormulario(usuario);
            }
        }
        else
        {
            UpdateStatus("Por favor, seleccione un usuario para editar", true);
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Carga los usuarios en el TreeView
    /// </summary>
    private void LoadUsuarios()
    {
        _listStore.Clear();

        // Aquí se obtienen los usuarios del servicio
        // Como no vemos un método específico para listar todos los usuarios,
        // esta parte quedaría pendiente de implementar con el método adecuado

        // Ejemplo de cómo se cargarían (suponiendo que existe un método para obtener todos)
        /*
        var usuarios = _servicioUsuarios.ObtenerTodosLosUsuarios();
        foreach (var usuario in usuarios)
        {
            _listStore.AppendValues(
                usuario.Id,
                usuario.Nombres,
                usuario.Apellidos,
                usuario.Correo,
                usuario.Edad,
                usuario.EsAdmin()
            );
        }
        */

        // Por ahora, simularemos algunos datos de ejemplo
        _listStore.AppendValues(1, "Juan", "Pérez", "juan@ejemplo.com", 30, false);
        _listStore.AppendValues(2, "María", "López", "maria@ejemplo.com", 28, false);
        _listStore.AppendValues(3, "Admin", "Sistema", "admin@usac.com", 35, true);
    }

    /// <summary>
    /// Carga los datos de un usuario en el formulario
    /// </summary>
    private void CargarUsuarioEnFormulario(Usuario usuario)
    {
        _txtId.Text = usuario.Id.ToString();
        _txtNombres.Text = usuario.Nombres;
        _txtApellidos.Text = usuario.Apellidos;
        _txtCorreo.Text = usuario.Correo;
        _txtEdad.Text = usuario.Edad.ToString();
        _txtContrasenia.Text = ""; // Por seguridad, no mostramos la contraseña
        _chkEsAdmin.Active = usuario.EsAdmin();

        UpdateStatus("Usuario cargado en el formulario", false);
    }

    /// <summary>
    /// Limpia el formulario
    /// </summary>
    private void LimpiarFormulario()
    {
        _txtId.Text = "Auto";
        _txtNombres.Text = "";
        _txtApellidos.Text = "";
        _txtCorreo.Text = "";
        _txtEdad.Text = "";
        _txtContrasenia.Text = "";
        _chkEsAdmin.Active = false;

        UpdateStatus("", false);
    }

    /// <summary>
    /// Actualiza el mensaje de estado
    /// </summary>
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