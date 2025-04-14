using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using Gtk;

namespace AutoGestPro.UI.Views.Admin
{
    /// <summary>
    /// Ventana para la gestión e inserción de usuarios en el sistema.
    /// Esta ventana pertenece al menú de administración del sistema.
    /// </summary>
    public class GestionUsuariosInsercion : Window
    {
        #region Private Fields

        // Constantes
        private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/style.css";
        private const string WINDOW_TITLE = "Gestión de Usuarios - Administrador";
        
        // Servicios
        private readonly ServicioUsuarios _servicioUsuarios = Estructuras.Clientes;

        // UI Components
        private Entry _txtIdUsuario;
        private Entry _txtNombres;
        private Entry _txtApellidos;
        private Entry _txtCorreo;
        private Entry _txtEdad;
        private Entry _txtContrasenia;
        private Button _btnGuardar;
        private Button _btnCancelar;
        private Label _lblStatus;
        private TreeView _treeView;
        private ListStore _listStore;
        private ProgressBar _progressBar;
        private CssProvider _cssProvider;
        private Stack _stack;
        private Box _mainBox;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la ventana de gestión de usuarios
        /// </summary>
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

        /// <summary>
        /// Inicializa la configuración de la ventana
        /// </summary>
        private void InitializeWindow()
        {
            SetDefaultSize(900, 600);
            SetPosition(WindowPosition.Center);
            DeleteEvent += (o, args) => 
            {
                args.RetVal = true; // Permite el cierre de la ventana
                this.Destroy(); // Solo destruye esta ventana, no toda la aplicación
            };
        }

        /// <summary>
        /// Inicializa los estilos CSS para la interfaz
        /// </summary>
        private void InitializeCSS()
        {
            _cssProvider = new CssProvider();
            string cssPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CSS_FILE_PATH);

            try
            {
                if (File.Exists(cssPath))
                {
                    _cssProvider.LoadFromPath(cssPath);
                    StyleContext.AddProviderForScreen(Gdk.Screen.Default, _cssProvider, 800);
                }
                else
                {
                    Console.WriteLine($"Archivo CSS no encontrado en: {cssPath}");
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
            
            var logo = new Label("👥");
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
            var contentBox = new Box(Orientation.Vertical, 16);
            contentBox.StyleContext.AddClass("main-content");
            contentBox.BorderWidth = 16;
            
            var cardBox = new Box(Orientation.Horizontal, 16);
            cardBox.StyleContext.AddClass("card");
            
            // Crear las diferentes secciones
            var leftBox = new Box(Orientation.Vertical, 16);
            var rightBox = new Box(Orientation.Vertical, 16);
            
            CreateForm(leftBox);
            CreateUserTable(rightBox);
            
            cardBox.PackStart(leftBox, true, true, 0);
            cardBox.PackStart(rightBox, true, true, 0);
            
            contentBox.PackStart(cardBox, true, true, 0);
            _mainBox.PackStart(contentBox, true, true, 0);
        }

        /// <summary>
        /// Crea el formulario de inserción de usuarios
        /// </summary>
        private void CreateForm(Box container)
        {
            var formTitle = new Label("Datos del Nuevo Usuario");
            formTitle.StyleContext.AddClass("subtitle");
            formTitle.Xalign = 0;
            container.PackStart(formTitle, false, false, 0);
            
            var formBox = new Box(Orientation.Vertical, 8);
            
            var idUsuario = CreateFormField("ID Usuario:", out _txtIdUsuario);
            formBox.PackStart(idUsuario, false, false, 8);
            
            var nombresBox = CreateFormField("Nombres:", out _txtNombres);
            formBox.PackStart(nombresBox, false, false, 8);

            var apellidosBox = CreateFormField("Apellidos:", out _txtApellidos);
            formBox.PackStart(apellidosBox, false, false, 8);

            var correoBox = CreateFormField("Correo electrónico:", out _txtCorreo);
            formBox.PackStart(correoBox, false, false, 8);

            var edadBox = CreateFormField("Edad:", out _txtEdad);
            formBox.PackStart(edadBox, false, false, 8);

            var contraseniaBox = CreateFormField("Contraseña:", out _txtContrasenia);
            _txtContrasenia.Visibility = false;
            formBox.PackStart(contraseniaBox, false, false, 8);
            
            container.PackStart(formBox, true, true, 0);
            
            // Barra de progreso
            _progressBar = new ProgressBar();
            _progressBar.Fraction = 0.0;
            _progressBar.NoShowAll = true;
            container.PackStart(_progressBar, false, false, 0);
            
            // Botones
            var botonesBox = new Box(Orientation.Horizontal, 8);

            _btnGuardar = new Button("Guardar Usuario");
            _btnGuardar.StyleContext.AddClass("boton-primary");
            _btnGuardar.Clicked += OnGuardarClicked;

            _btnCancelar = new Button("Limpiar");
            _btnCancelar.StyleContext.AddClass("boton-secondary");
            _btnCancelar.Clicked += OnCancelarClicked;

            botonesBox.PackStart(_btnGuardar, true, true, 0);
            botonesBox.PackStart(_btnCancelar, true, true, 0);

            container.PackStart(botonesBox, false, false, 16);

            _lblStatus = new Label("");
            _lblStatus.StyleContext.AddClass("info");
            _lblStatus.Xalign = 0;
            container.PackStart(_lblStatus, false, false, 8);
        }

        /// <summary>
        /// Crea un campo para el formulario
        /// </summary>
        private Box CreateFormField(string labelText, out Entry entry)
        {
            var box = new Box(Orientation.Horizontal, 8);
            box.StyleContext.AddClass("form-field");

            var label = new Label(labelText);
            label.Xalign = 0;
            label.SetSizeRequest(150, -1);
            label.StyleContext.AddClass("form-label");

            entry = new Entry();
            entry.StyleContext.AddClass("form-entry");

            box.PackStart(label, false, false, 0);
            box.PackStart(entry, true, true, 0);

            return box;
        }

        /// <summary>
        /// Crea la tabla de usuarios
        /// </summary>
        private void CreateUserTable(Box container)
        {
            var tableTitle = new Label("Usuarios Registrados");
            tableTitle.StyleContext.AddClass("subtitle");
            tableTitle.Xalign = 0;
            container.PackStart(tableTitle, false, false, 0);
            
            _listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

            _treeView = new TreeView(_listStore)
            {
                HeadersVisible = true
            };
            
            _treeView.StyleContext.AddClass("tree-view");

            AddTreeViewColumn("ID", 0);
            AddTreeViewColumn("Nombres", 1);
            AddTreeViewColumn("Apellidos", 2);
            AddTreeViewColumn("Correo", 3);
            AddTreeViewColumn("Edad", 4);
            AddTreeViewColumn("Hash", 5);

            var scroll = new ScrolledWindow();
            scroll.ShadowType = ShadowType.EtchedIn;
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.SetSizeRequest(-1, 400);
            scroll.Add(_treeView);
            
            container.PackStart(scroll, true, true, 0);
        }
        
        /// <summary>
        /// Añade una columna al TreeView
        /// </summary>
        private void AddTreeViewColumn(string title, int columnIndex)
        {
            var column = new TreeViewColumn();
            column.Title = title;
            var cell = new CellRendererText();
            column.PackStart(cell, true);
            column.AddAttribute(cell, "text", columnIndex);
            _treeView.AppendColumn(column);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Manejador de evento para el botón Guardar
        /// </summary>
        private async void OnGuardarClicked(object sender, EventArgs e)
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

                if (!int.TryParse(_txtIdUsuario.Text, out int id) || id <= 0)
                {
                    UpdateStatus("El ID debe ser un número entero positivo", true);
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
                
                // Mostrar progreso
                _progressBar.Show();
                UpdateStatus("Guardando usuario...", false);
                _btnGuardar.Sensitive = false;
                
                // Animación de progreso
                await AnimateProgress();

                var usuario = new Usuario(
                    id,
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
                    _listStore.AppendValues(
                        usuarioRegistrado.Id.ToString(),
                        usuario.Nombres,
                        usuario.Apellidos,
                        usuario.Correo,
                        usuario.Edad.ToString(),
                        usuario.ContraseniaHash
                    );
                    
                    UpdateStatus("¡Usuario guardado correctamente!", false);
                    _btnGuardar.StyleContext.RemoveClass("boton-primary");
                    _btnGuardar.StyleContext.AddClass("boton-primary-success");
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
            finally
            {
                _progressBar.Fraction = 1.0;
                await Task.Delay(500);
                _progressBar.Hide();
                _btnGuardar.Sensitive = true;
            }
        }

        /// <summary>
        /// Manejador de evento para el botón Cancelar
        /// </summary>
        private void OnCancelarClicked(object sender, EventArgs e)
        {
            LimpiarFormulario();
            UpdateStatus("Formulario limpiado", false);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Anima la barra de progreso
        /// </summary>
        private async Task AnimateProgress()
        {
            for (double i = 0; i <= 0.9; i += 0.1)
            {
                _progressBar.Fraction = i;
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// Carga la lista de usuarios existentes
        /// </summary>
        private void LoadUsuarios()
        {
            _listStore.Clear();
            
            try 
            {
                var usuarios = _servicioUsuarios.ObtenerTodos();
                
                if (usuarios != null && usuarios.Count > 0)
                {
                    foreach (var usuario in usuarios)
                    {
                        _listStore.AppendValues(
                            usuario.Id.ToString(),
                            usuario.Nombres,
                            usuario.Apellidos,
                            usuario.Correo,
                            usuario.Edad.ToString(),
                            usuario.ContraseniaHash
                        );
                    }
                    
                    UpdateStatus($"Se cargaron {usuarios.Count} usuarios del sistema", false);
                }
                else
                {
                    UpdateStatus("No hay usuarios registrados en el sistema", false);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error al cargar usuarios: {ex.Message}", true);
            }
        }

        /// <summary>
        /// Limpia los campos del formulario
        /// </summary>
        private void LimpiarFormulario()
        {
            _txtIdUsuario.Text = "";
            _txtNombres.Text = "";
            _txtApellidos.Text = "";
            _txtCorreo.Text = "";
            _txtEdad.Text = "";
            _txtContrasenia.Text = "";
            
            // Restaurar estilo del botón guardar
            _btnGuardar.StyleContext.RemoveClass("boton-primary-success");
            _btnGuardar.StyleContext.AddClass("boton-primary");
        }

        /// <summary>
        /// Actualiza el texto de estado
        /// </summary>
        private void UpdateStatus(string message, bool isError)
        {
            _lblStatus.Text = message;
            
            _lblStatus.StyleContext.RemoveClass("error");
            _lblStatus.StyleContext.RemoveClass("success");
            
            if (isError)
            {
                _lblStatus.StyleContext.AddClass("error");
            }
            else if (!string.IsNullOrEmpty(message))
            {
                _lblStatus.StyleContext.AddClass("success");
            }
        }

        /// <summary>
        /// Muestra un cuadro de diálogo con un mensaje
        /// </summary>
        private void ShowMessage(string message)
        {
            using (var dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Info,
                ButtonsType.Ok,
                message))
            {
                dialog.Run();
                dialog.Destroy();
            }
        }

        #endregion
    }
}