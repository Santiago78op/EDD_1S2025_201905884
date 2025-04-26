using AutoGestPro.Core.Services;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.Admin
{
    /// <summary>
    /// Ventana para la visualización de logs de usuarios en el sistema.
    /// </summary>
    public class VisualizacionLogs : Window
    {
        private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/style.css";
        private const string WINDOW_TITLE = "Logs de Usuarios - Administrador";

        private TreeView _treeViewLogs;
        private ListStore _listStore;
        private Button _btnExportarTodos;
        private Button _btnExportarPeriodo;
        private Button _btnActualizar;
        private Label _lblStatus;
        private ProgressBar _progressBar;
        private CssProvider _cssProvider;
        private Calendar _calendarInicio;
        private Calendar _calendarFin;

        private readonly LogService _logService;

        /// <summary>
        /// Constructor de la ventana de visualización de logs.
        /// </summary>
        public VisualizacionLogs() : base(WINDOW_TITLE)
        {
            // Obtener el servicio de logs desde la sesión
            _logService = Sesion.ObtenerServicioLogs();

            InitializeWindow();
            InitializeCSS();
            CreateUI();
            CargarLogs();
            ShowAll();
        }

        /// <summary>
        /// Inicializa la configuración de la ventana.
        /// </summary>
        private void InitializeWindow()
        {
            SetDefaultSize(900, 600);
            SetPosition(WindowPosition.Center);
            DeleteEvent += (o, args) =>
            {
                args.RetVal = true; // Permite el cierre de la ventana
                Hide(); // Solo oculta esta ventana, no la destruye
            };
        }

        /// <summary>
        /// Inicializa los estilos CSS para la interfaz.
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

        /// <summary>
        /// Crea la interfaz de usuario.
        /// </summary>
        private void CreateUI()
        {
            var mainBox = new Box(Orientation.Vertical, 16);
            mainBox.BorderWidth = 16;

            // Título
            var titleLabel = new Label("Registro de Actividad de Usuarios");
            titleLabel.AddCssClass("label-titulo");
            titleLabel.Xalign = 0;
            mainBox.PackStart(titleLabel, false, false, 0);

            // Descripción
            var descriptionLabel = new Label(
                "Este módulo permite visualizar y exportar los registros de entrada y salida de usuarios en el sistema."
            );
            descriptionLabel.Xalign = 0;
            descriptionLabel.LineWrap = true;
            mainBox.PackStart(descriptionLabel, false, false, 8);

            // Área de filtros y exportación
            var controlBox = new Box(Orientation.Horizontal, 16);

            // Sección de botones
            var buttonsBox = new Box(Orientation.Vertical, 8);
            
            _btnActualizar = new Button("🔄 Actualizar Lista");
            _btnActualizar.AddCssClass("boton");
            _btnActualizar.Clicked += OnActualizarClicked;
            
            _btnExportarTodos = new Button("📤 Exportar Todos");
            _btnExportarTodos.AddCssClass("boton-primary");
            _btnExportarTodos.Clicked += OnExportarTodosClicked;
            
            buttonsBox.PackStart(_btnActualizar, false, false, 0);
            buttonsBox.PackStart(_btnExportarTodos, false, false, 0);
            
            controlBox.PackStart(buttonsBox, false, false, 0);
            
            // Sección de calendarios
            var calendarsBox = new Box(Orientation.Vertical, 8);
            
            var calendarTitulo = new Label("Exportar por Periodo");
            calendarTitulo.AddCssClass("subtitle");
            calendarTitulo.Xalign = 0;
            calendarsBox.PackStart(calendarTitulo, false, false, 0);
            
            var calendarGrid = new Grid();
            calendarGrid.ColumnSpacing = 16;
            calendarGrid.RowSpacing = 8;
            
            var inicioLabel = new Label("Fecha Inicio:");
            inicioLabel.Xalign = 0;
            calendarGrid.Attach(inicioLabel, 0, 0, 1, 1);
            
            _calendarInicio = new Calendar();
            calendarGrid.Attach(_calendarInicio, 0, 1, 1, 1);
            
            var finLabel = new Label("Fecha Fin:");
            finLabel.Xalign = 0;
            calendarGrid.Attach(finLabel, 1, 0, 1, 1);
            
            _calendarFin = new Calendar();
            calendarGrid.Attach(_calendarFin, 1, 1, 1, 1);
            
            calendarsBox.PackStart(calendarGrid, false, false, 0);
            
            _btnExportarPeriodo = new Button("📥 Exportar Periodo");
            _btnExportarPeriodo.AddCssClass("boton-primary");
            _btnExportarPeriodo.Clicked += OnExportarPeriodoClicked;
            
            calendarsBox.PackStart(_btnExportarPeriodo, false, false, 8);
            
            controlBox.PackStart(calendarsBox, true, true, 0);
            
            mainBox.PackStart(controlBox, false, false, 0);
            
            // Separador
            var separator = new Separator(Orientation.Horizontal);
            mainBox.PackStart(separator, false, false, 8);

            // Tabla de logs
            var tableLabel = new Label("Registros de Actividad");
            tableLabel.AddCssClass("subtitle");
            tableLabel.Xalign = 0;
            mainBox.PackStart(tableLabel, false, false, 0);

            _listStore = new ListStore(
                typeof(string),   // Usuario
                typeof(string),   // Entrada (fecha formateada)
                typeof(string)    // Salida (fecha formateada)
            );

            _treeViewLogs = new TreeView(_listStore)
            {
                HeadersVisible = true
            };

            _treeViewLogs.AppendColumn("Usuario", new CellRendererText(), "text", 0);
            _treeViewLogs.AppendColumn("Entrada", new CellRendererText(), "text", 1);
            _treeViewLogs.AppendColumn("Salida", new CellRendererText(), "text", 2);

            var scrollWindow = new ScrolledWindow();
            scrollWindow.ShadowType = ShadowType.EtchedIn;
            scrollWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollWindow.Add(_treeViewLogs);

            mainBox.PackStart(scrollWindow, true, true, 0);

            // Barra de progreso
            _progressBar = new ProgressBar();
            _progressBar.Fraction = 0.0;
            _progressBar.NoShowAll = true;
            mainBox.PackStart(_progressBar, false, false, 0);

            // Etiqueta de estado
            _lblStatus = new Label("");
            _lblStatus.Xalign = 0;
            mainBox.PackStart(_lblStatus, false, false, 0);

            Add(mainBox);
        }

        /// <summary>
        /// Carga los logs en la tabla.
        /// </summary>
        private void CargarLogs()
        {
            _listStore.Clear();

            try
            {
                var logs = _logService.ObtenerTodosLogs();

                foreach (var log in logs)
                {
                    _listStore.AppendValues(
                        log.Usuario,
                        log.Entrada.ToString("yyyy-MM-dd HH:mm:ss"),
                        log.Salida.HasValue ? log.Salida.Value.ToString("yyyy-MM-dd HH:mm:ss") : "Sesión activa"
                    );
                }

                UpdateStatus($"Se cargaron {logs.Count} registros de actividad.");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error al cargar logs: {ex.Message}", true);
            }
        }

        /// <summary>
        /// Manejador del evento del botón Actualizar.
        /// </summary>
        private void OnActualizarClicked(object sender, EventArgs e)
        {
            CargarLogs();
        }

        /// <summary>
        /// Manejador del evento del botón Exportar Todos.
        /// </summary>
        private async void OnExportarTodosClicked(object sender, EventArgs e)
        {
            await ExportarLogs(() => _logService.ExportarLogsAsync(), "todos los logs");
        }

        /// <summary>
        /// Manejador del evento del botón Exportar Periodo.
        /// </summary>
        private async void OnExportarPeriodoClicked(object sender, EventArgs e)
        {
            // Obtener fechas seleccionadas
            var fechaInicio = new DateTime(
                _calendarInicio.Date.Year,
                _calendarInicio.Date.Month + 1, // Gtk Calendar usa 0-11 para meses
                _calendarInicio.Date.Day,
                0, 0, 0
            );

            var fechaFin = new DateTime(
                _calendarFin.Date.Year,
                _calendarFin.Date.Month + 1, // Gtk Calendar usa 0-11 para meses
                _calendarFin.Date.Day,
                23, 59, 59
            );

            // Validar que fecha inicio sea menor que fecha fin
            if (fechaInicio > fechaFin)
            {
                ShowMessage("Error", "La fecha de inicio debe ser anterior a la fecha de fin.", MessageType.Error);
                return;
            }

            await ExportarLogs(
                () => _logService.ExportarLogsPeriodoAsync(fechaInicio, fechaFin),
                $"logs del periodo {fechaInicio:yyyy-MM-dd} al {fechaFin:yyyy-MM-dd}"
            );
        }

        /// <summary>
        /// Método auxiliar para exportar logs con animación de progreso.
        /// </summary>
        private async Task ExportarLogs(Func<Task<string>> exportFunc, string descripcion)
        {
            try
            {
                _progressBar.Show();
                SetButtonsEnabled(false);
                UpdateStatus($"Exportando {descripcion}...");

                // Animar la barra de progreso
                await AnimateProgress(0, 0.7);

                // Realizar la exportación
                string filePath = await exportFunc();

                await AnimateProgress(0.7, 1.0);
                UpdateStatus($"Exportación completada: {System.IO.Path.GetFileName(filePath)}");

                ShowMessage("Exportación completada", 
                    $"Se han exportado {descripcion} al archivo:\n{filePath}", 
                    MessageType.Info);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error al exportar logs: {ex.Message}", true);
                ShowMessage("Error de exportación", 
                    $"Se produjo un error al exportar los logs:\n{ex.Message}", 
                    MessageType.Error);
            }
            finally
            {
                _progressBar.Hide();
                SetButtonsEnabled(true);
            }
        }

        /// <summary>
        /// Anima la barra de progreso.
        /// </summary>
        private async Task AnimateProgress(double start, double end)
        {
            for (double i = start; i <= end; i += 0.05)
            {
                _progressBar.Fraction = i;
                await Task.Delay(50);
            }
        }

        /// <summary>
        /// Habilita o deshabilita los botones.
        /// </summary>
        private void SetButtonsEnabled(bool enabled)
        {
            _btnActualizar.Sensitive = enabled;
            _btnExportarTodos.Sensitive = enabled;
            _btnExportarPeriodo.Sensitive = enabled;
            _calendarInicio.Sensitive = enabled;
            _calendarFin.Sensitive = enabled;
        }

        /// <summary>
        /// Actualiza el texto de estado.
        /// </summary>
        private void UpdateStatus(string message, bool isError = false)
        {
            _lblStatus.Text = message;
            
            _lblStatus.AddCssClass(isError ? "error" : "success");
        }

        /// <summary>
        /// Muestra un cuadro de diálogo con un mensaje.
        /// </summary>
        private void ShowMessage(string title, string message, MessageType type = MessageType.Info)
        {
            using (var dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                type,
                ButtonsType.Ok,
                message))
            {
                dialog.Title = title;
                dialog.Run();
                dialog.Destroy();
            }
        }
    }
}