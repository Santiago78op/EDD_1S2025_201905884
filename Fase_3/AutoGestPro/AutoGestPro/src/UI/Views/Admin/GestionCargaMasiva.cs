using System;
using System.IO;
using System.Threading.Tasks;
using AutoGestPro.Core.Services;
using Gtk;

namespace AutoGestPro.UI.Views.Admin
{
    /// <summary>
    /// Ventana para la gestión de carga masiva de datos desde archivos JSON.
    /// Esta ventana pertenece al menú de administración del sistema.
    /// La lógica de procesamiento de datos se implementa en otra clase.
    /// </summary>
    public class GestionCargaMasiva : Window
    {
        #region Private Fields

        // Constantes
        private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/cargaMasiva.css";
        private const string WINDOW_TITLE = "Sistema de Carga Masiva - Administrador";
        
        // Tipos de entidades disponibles
        private readonly string[] _tiposEntidad = { "USUARIOS", "VEHICULOS", "REPUESTOS", "SERVICIOS" };

        // UI Components
        private ComboBox _cmbTipoEntidad;
        private Button _btnCargar;
        private TreeView _treeView;
        private ListStore _listStore;
        private Label _lblStatus;
        private ProgressBar _progressBar;
        private ScrolledWindow _scrolledWindow;
        private FileChooserButton _fileChooserButton;
        private string _archivoSeleccionado;
        private Stack _stack;
        private Box _mainBox;
        private CssProvider _cssProvider;

        // Delegado para eventos de carga
        public delegate void CargaDatosHandler(string tipoEntidad, string archivoJson);
        public event CargaDatosHandler OnCargaDatosSolicitada;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la ventana de carga masiva
        /// </summary>
        public GestionCargaMasiva() : base(WINDOW_TITLE)
        {
            InitializeWindow();
            InitializeCSS();
            CreateUI();
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
            
            var logo = new Label("🔄");
            logo.SetSizeRequest(40, 40);
            
            var titleLabel = new Label("Sistema de Carga Masiva");
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
            
            var cardBox = new Box(Orientation.Vertical, 16);
            cardBox.StyleContext.AddClass("card");
            
            // Crear las diferentes secciones
            CreateEntitySelector(cardBox);
            CreateFileSelector(cardBox);
            CreateProgressArea(cardBox);
            CreateDataViewer(cardBox);
            
            contentBox.PackStart(cardBox, true, true, 0);
            _mainBox.PackStart(contentBox, true, true, 0);
        }

        /// <summary>
        /// Crea el selector de tipo de entidad
        /// </summary>
        private void CreateEntitySelector(Box container)
        {
            var selectorBox = new Box(Orientation.Horizontal, 8);
            var lblEntidad = new Label("Tipo de Entidad:");
            lblEntidad.Xalign = 0;
            lblEntidad.StyleContext.AddClass("subtitle");
        
            // Crear el modelo de datos para el ComboBox
            var listStore = new ListStore(typeof(string));
            foreach (var tipo in _tiposEntidad)
            {
                listStore.AppendValues(tipo);
            }
        
            // Crear el ComboBox con el modelo de datos
            _cmbTipoEntidad = new ComboBox(listStore);
        
            // Configurar el renderizador para mostrar texto
            var cellRenderer = new CellRendererText();
            _cmbTipoEntidad.PackStart(cellRenderer, true);
            _cmbTipoEntidad.AddAttribute(cellRenderer, "text", 0);
        
            // Seleccionar el primer elemento por defecto
            _cmbTipoEntidad.Active = 0;
        
            // Conectar el evento Changed
            _cmbTipoEntidad.Changed += OnTipoEntidadChanged;
        
            selectorBox.PackStart(lblEntidad, false, false, 0);
            selectorBox.PackStart(_cmbTipoEntidad, true, true, 0);
        
            container.PackStart(selectorBox, false, false, 0);
        }

        /// <summary>
        /// Crea el selector de archivo
        /// </summary>
        private void CreateFileSelector(Box container)
        {
            var fileBox = new Box(Orientation.Horizontal, 8);
            var lblArchivo = new Label("Archivo JSON:");
            lblArchivo.Xalign = 0;
            lblArchivo.StyleContext.AddClass("subtitle");
            
            _fileChooserButton = new FileChooserButton("Seleccionar archivo JSON", FileChooserAction.Open);
            var filterJson = new FileFilter();
            filterJson.AddPattern("*.json");
            filterJson.Name = "Archivos JSON";
            _fileChooserButton.AddFilter(filterJson);
            _fileChooserButton.FileSet += OnFileSelected;
            
            _btnCargar = new Button("Cargar Datos");
            _btnCargar.StyleContext.AddClass("boton-primary");
            _btnCargar.Sensitive = false;
            _btnCargar.Clicked += OnCargarClicked;
            
            fileBox.PackStart(lblArchivo, false, false, 0);
            fileBox.PackStart(_fileChooserButton, true, true, 0);
            fileBox.PackStart(_btnCargar, false, false, 0);
            
            container.PackStart(fileBox, false, false, 0);
        }

        /// <summary>
        /// Crea la barra de progreso
        /// </summary>
        private void CreateProgressArea(Box container)
        {
            _progressBar = new ProgressBar();
            _progressBar.Fraction = 0.0;
            _progressBar.NoShowAll = true;
            
            container.PackStart(_progressBar, false, false, 0);
        }

        /// <summary>
        /// Crea el visualizador de datos
        /// </summary>
        private void CreateDataViewer(Box container)
        {
            var treeViewBox = new Box(Orientation.Vertical, 8);
            
            var lblDatos = new Label("Datos cargados:");
            lblDatos.Xalign = 0;
            lblDatos.StyleContext.AddClass("subtitle");
            
            treeViewBox.PackStart(lblDatos, false, false, 0);
            
            // Crear TreeView para mostrar los datos
            _scrolledWindow = new ScrolledWindow();
            _scrolledWindow.ShadowType = ShadowType.EtchedIn;
            _scrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            _scrolledWindow.SetSizeRequest(-1, 300);
            
            // Etiqueta de estado
            _lblStatus = new Label("Seleccione un archivo JSON para cargar");
            _lblStatus.StyleContext.AddClass("info");
            _lblStatus.Xalign = 0;
            
            // Inicializar TreeView
            _listStore = new ListStore(typeof(string), typeof(string), typeof(string));
            _treeView = new TreeView(_listStore);
            ConfigureTreeViewForEntity(_tiposEntidad[0]); // Configuración predeterminada
            
            _scrolledWindow.Add(_treeView);
            treeViewBox.PackStart(_scrolledWindow, true, true, 0);
            treeViewBox.PackStart(_lblStatus, false, false, 0);
            
            container.PackStart(treeViewBox, true, true, 0);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Manejador de evento para la selección de archivo
        /// </summary>
        private void OnFileSelected(object sender, EventArgs e)
        {
            _archivoSeleccionado = _fileChooserButton.Filename;
            if (!string.IsNullOrEmpty(_archivoSeleccionado))
            {
                _btnCargar.Sensitive = true;
                UpdateStatus($"Archivo seleccionado: {System.IO.Path.GetFileName(_archivoSeleccionado)}");
            }
        }
        
        /// <summary>
        /// Manejador de evento para el cambio de tipo de entidad
        /// </summary>
        private void OnTipoEntidadChanged(object sender, EventArgs e)
        {
            // Obtener el índice activo
            int activeIndex = _cmbTipoEntidad.Active;
        
            // Validar que el índice sea válido
            if (activeIndex >= 0)
            {
                // Obtener el texto del modelo de datos
                TreeIter iter;
                if (_cmbTipoEntidad.Model.GetIterFromString(out iter, activeIndex.ToString()))
                {
                    string tipoEntidad = (string)_cmbTipoEntidad.Model.GetValue(iter, 0);
                    ConfigureTreeViewForEntity(tipoEntidad);
                }
            }
        
            // Limpiar datos anteriores
            _listStore.Clear();
        }
        
        /// <summary>
        /// Manejador de evento para el botón Cargar
        /// </summary>
        private async void OnCargarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_archivoSeleccionado))
            {
                ShowMessage("Por favor, seleccione un archivo JSON primero.");
                return;
            }
            
            // Mostrar progreso
            _progressBar.Show();
            UpdateStatus("Cargando datos...");
            _btnCargar.Sensitive = false;
            
            try
            {
                // Animación de progreso
                await AnimateProgress();
                
                // Obtener el índice activo
                int activeIndex = _cmbTipoEntidad.Active;
                
                // Validar que el índice sea válido
                if (activeIndex >= 0)
                {
                    // Crear un TreePath a partir del índice activo
                    TreePath path = new TreePath(new[] { activeIndex });
                
                    // Obtener el texto del modelo de datos
                    TreeIter iter;
                    if (_cmbTipoEntidad.Model.GetIter(out iter, path))
                    {
                        string tipoEntidadNow = (string)_cmbTipoEntidad.Model.GetValue(iter, 0);
                
                        // Crear instancia del servicio de carga masiva
                        var servicioCarga = new CargaMasivaService();
                
                        // Ejecutar la carga masiva
                        bool resultado = servicioCarga.GestionarCargaMasiva(tipoEntidadNow, _archivoSeleccionado);
                
                        // Actualizar el estado según el resultado 
                        if (resultado)
                        {
                            // Actualizar datos en el TreeView
                            ActualizarDatos(servicioCarga.DatosCargados, tipoEntidadNow);
                            UpdateStatus($"Datos de {tipoEntidadNow} cargados correctamente.");
                            _btnCargar.StyleContext.RemoveClass("boton-primary");
                            _btnCargar.StyleContext.AddClass("boton-primary-success");
                        }
                        else
                        {
                            UpdateStatus($"Error al cargar datos de {tipoEntidadNow}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error al leer el archivo: {ex.Message}");
            }
            finally
            {
                _progressBar.Fraction = 1.0;
                await Task.Delay(500);
                _progressBar.Hide();
                _btnCargar.Sensitive = true;
            }
        }

        #endregion

        #region TreeView Configuration

        /// <summary>
        /// Configura el TreeView según el tipo de entidad seleccionado
        /// </summary>
        private void ConfigureTreeViewForEntity(string tipoEntidad)
        {
            // Limpiar columnas existentes
            foreach (var col in _treeView.Columns)
            {
                _treeView.RemoveColumn(col);
            }
            
            // Configurar columnas según el tipo de entidad
            switch (tipoEntidad)
            {
                case "USUARIOS":
                    AddTreeViewColumn("ID", 0);
                    AddTreeViewColumn("Nombres", 1);
                    AddTreeViewColumn("Apellidos", 2);
                    AddTreeViewColumn("Correo", 3);
                    AddTreeViewColumn("Edad", 4);
                    AddTreeViewColumn("Contraseña", 5);
                    _listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));
                    break;
                case "VEHICULOS":
                    AddTreeViewColumn("ID", 0);
                    AddTreeViewColumn("ID Usuario", 1);
                    AddTreeViewColumn("Marca", 2);
                    AddTreeViewColumn("Modelo", 3);
                    AddTreeViewColumn("Placa", 4);
                    _listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));
                    break;
                case "REPUESTOS":
                    AddTreeViewColumn("ID", 0);
                    AddTreeViewColumn("Repuesto", 1);
                    AddTreeViewColumn("Detalles", 2);
                    AddTreeViewColumn("Costo", 3);
                    _listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));
                    break;
                case "SERVICIOS":
                    AddTreeViewColumn("ID", 0);
                    AddTreeViewColumn("Id Usuario", 1);
                    AddTreeViewColumn("Id Repuesto", 2);
                    AddTreeViewColumn("Id Vehículo", 3);
                    AddTreeViewColumn("Detalles", 4);
                    AddTreeViewColumn("Costo", 5);
                    _listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));
                    break;
                default:
                    ShowMessage("Tipo de entidad no soportado.");
                    return;
            }
            
            // Recrear el ListStore
            _treeView.Model = _listStore;
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

        #region Public Methods

        /// <summary>
        /// Método público para actualizar los datos en el TreeView
        /// </summary>
        /// <param name="datos">Array de arrays de strings con los datos a mostrar</param>
        public void ActualizarDatos(string[][] datos, string tipoEntidad )
        {
            _listStore.Clear();
            
            if (datos != null)
            {
                foreach (var fila in datos)
                {
                    if (fila.Length >= 3)
                    {
                        switch (tipoEntidad)
                        {
                            case "USUARIOS":
                                _listStore.AppendValues(fila[0], fila[1], fila[2], fila[3], fila[4], fila[5]);
                                break;
                            case "VEHICULOS":
                                _listStore.AppendValues(fila[0], fila[1], fila[2], fila[3], fila[4]);
                                break;
                            case "REPUESTOS":
                                _listStore.AppendValues(fila[0], fila[1], fila[2], fila[3]);
                                break;
                            case "SERVICIOS":
                                _listStore.AppendValues(fila[0], fila[1], fila[2], fila[3], fila[4], fila[5]);
                                break;
                            default:
                                _listStore.AppendValues("Error", "Tipo de entidad no soportado", "");
                                break;
                        }
                    }
                }
                
                UpdateStatus($"Se cargaron {datos.Length} registros correctamente.");
            }
            else
            {
                UpdateStatus("No se recibieron datos para mostrar.");
            }
        }

        /// <summary>
        /// Método para mostrar un mensaje de error en la interfaz
        /// </summary>
        public void MostrarError(string mensaje)
        {
            UpdateStatus($"Error: {mensaje}");
            ShowMessage(mensaje);
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
        /// Actualiza el texto de estado
        /// </summary>
        private void UpdateStatus(string message)
        {
            _lblStatus.Text = message;
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