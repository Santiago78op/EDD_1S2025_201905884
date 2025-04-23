using AutoGestPro.Core.Services;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.Admin;

/// <summary>
/// Ventana para la gestión de backups del sistema
/// </summary>
public class GestionBackups : Window
{
    private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/style.css";
    private const string WINDOW_TITLE = "Gestión de Backups - Administrador";

    private Button _btnBackupUsuarios;
    private Button _btnBackupVehiculos;
    private Button _btnBackupRepuestos;
    private Button _btnBackupCompleto;
    private ProgressBar _progressBar;
    private Label _lblStatus;
    private TreeView _treeViewBackups;
    private ListStore _listStore;
    private CssProvider _cssProvider;

    private readonly BackupService _backupService;
    private readonly string _backupPath;

    public GestionBackups() : base(WINDOW_TITLE)
    {
        InitializeWindow();
        InitializeCSS();

        // Inicializar el servicio de backup
        _backupService = new BackupService();
        _backupPath = BackupService.GetBackupPath();

        CreateUI();
        LoadBackupHistory();
        ShowAll();
    }

    private void InitializeWindow()
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) =>
        {
            args.RetVal = true; // Permite el cierre de la ventana
            Hide(); // Solo oculta esta ventana, no la destruye
        };
    }

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

    private void CreateUI()
    {
        // Contenedor principal
        var mainBox = new Box(Orientation.Vertical, 16);
        mainBox.BorderWidth = 16;

        // Título
        var titleLabel = new Label("Gestión de Backups del Sistema");
        titleLabel.AddCssClass("label-titulo");
        titleLabel.Xalign = 0;
        mainBox.PackStart(titleLabel, false, false, 0);

        // Descripción
        var descriptionLabel = new Label(
            "Este módulo permite crear copias de seguridad de las entidades del sistema.\n" +
            "• Usuarios: Backup en formato JSON sin comprimir.\n" +
            "• Vehículos y Repuestos: Backup comprimido con algoritmo Huffman en formato .edd."
        );
        descriptionLabel.Xalign = 0;
        descriptionLabel.LineWrap = true;
        mainBox.PackStart(descriptionLabel, false, false, 8);

        // Panel de botones
        var buttonsBox = new Box(Orientation.Horizontal, 16);

        _btnBackupUsuarios = new Button("Backup Usuarios");
        _btnBackupUsuarios.AddCssClass("boton-primary");
        _btnBackupUsuarios.Clicked += OnBackupUsuariosClicked;

        _btnBackupVehiculos = new Button("Backup Vehículos");
        _btnBackupVehiculos.AddCssClass("boton-primary");
        _btnBackupVehiculos.Clicked += OnBackupVehiculosClicked;

        _btnBackupRepuestos = new Button("Backup Repuestos");
        _btnBackupRepuestos.AddCssClass("boton-primary");
        _btnBackupRepuestos.Clicked += OnBackupRepuestosClicked;

        _btnBackupCompleto = new Button("Backup Completo");
        _btnBackupCompleto.AddCssClass("boton-primary-success");
        _btnBackupCompleto.Clicked += OnBackupCompletoClicked;

        buttonsBox.PackStart(_btnBackupUsuarios, true, true, 0);
        buttonsBox.PackStart(_btnBackupVehiculos, true, true, 0);
        buttonsBox.PackStart(_btnBackupRepuestos, true, true, 0);
        buttonsBox.PackStart(_btnBackupCompleto, true, true, 0);

        mainBox.PackStart(buttonsBox, false, false, 0);

        // Barra de progreso
        _progressBar = new ProgressBar();
        _progressBar.Fraction = 0.0;
        _progressBar.NoShowAll = true;
        mainBox.PackStart(_progressBar, false, false, 0);

        // Etiqueta de estado
        _lblStatus = new Label("");
        _lblStatus.Xalign = 0;
        mainBox.PackStart(_lblStatus, false, false, 0);

        // Ruta de la carpeta de backups
        var backupPathLabel = new Label($"Carpeta de backups: {_backupPath}");
        backupPathLabel.Xalign = 0;
        mainBox.PackStart(backupPathLabel, false, false, 8);

        // Historial de backups
        var historyLabel = new Label("Historial de Backups");
        historyLabel.AddCssClass("subtitle");
        historyLabel.Xalign = 0;
        mainBox.PackStart(historyLabel, false, false, 8);

        // TreeView para el historial
        _listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));

        _treeViewBackups = new TreeView(_listStore)
        {
            HeadersVisible = true
        };

        _treeViewBackups.AppendColumn("Fecha", new CellRendererText(), "text", 0);
        _treeViewBackups.AppendColumn("Tipo", new CellRendererText(), "text", 1);
        _treeViewBackups.AppendColumn("Archivo", new CellRendererText(), "text", 2);
        _treeViewBackups.AppendColumn("Tamaño", new CellRendererText(), "text", 3);

        var scrollWindow = new ScrolledWindow();
        scrollWindow.ShadowType = ShadowType.EtchedIn;
        scrollWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        scrollWindow.Add(_treeViewBackups);

        mainBox.PackStart(scrollWindow, true, true, 0);

        Add(mainBox);
    }

    private async void OnBackupUsuariosClicked(object sender, EventArgs e)
    {
        await GenerarBackup(async () => await Task.Run(() => _backupService.BackupUsuarios()), "Usuarios");
    }

    private async void OnBackupVehiculosClicked(object sender, EventArgs e)
    {
        await GenerarBackup(async () => await Task.Run(() => _backupService.BackupVehiculos()), "Vehículos");
    }

    private async void OnBackupRepuestosClicked(object sender, EventArgs e)
    {
        await GenerarBackup(async () => await Task.Run(() => _backupService.BackupRepuestos()), "Repuestos");
    }

    private async void OnBackupCompletoClicked(object sender, EventArgs e)
    {
        try
        {
            _progressBar.Show();
            SetButtonsEnabled(false);
            UpdateStatus("Generando backup completo...");

            // Animar la barra de progreso
            await AnimateProgress(0, 0.3);

            // Generar todos los backups en paralelo
            var results = await Task.Run(() => _backupService.GenerarTodosLosBackups());

            await AnimateProgress(0.3, 0.9);

            // Actualizar la lista de backups
            await LoadBackupHistoryAsync();

            _progressBar.Fraction = 1.0;
            UpdateStatus($"Backup completo generado exitosamente. Archivos: {string.Join(", ", results.Keys)}");
            await Task.Delay(500);
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error al generar backup completo: {ex.Message}");
        }
        finally
        {
            _progressBar.Hide();
            SetButtonsEnabled(true);
        }
    }

    private async Task GenerarBackup(Func<Task<string>> backupAction, string tipo)
    {
        try
        {
            _progressBar.Show();
            SetButtonsEnabled(false);
            UpdateStatus($"Generando backup de {tipo}...");

            // Animar la barra de progreso
            await AnimateProgress(0, 0.7);

            // Generar el backup
            string filePath = await backupAction();

            await AnimateProgress(0.7, 0.9);

            // Actualizar la lista de backups
            await LoadBackupHistoryAsync();

            _progressBar.Fraction = 1.0;
            UpdateStatus($"Backup de {tipo} generado exitosamente: {System.IO.Path.GetFileName(filePath)}");

            // Mostrar mensaje de confirmación
            ShowMessage("Backup generado", $"El backup de {tipo} se ha generado correctamente en:\n{filePath}");

            await Task.Delay(500);
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error al generar backup de {tipo}: {ex.Message}");
            ShowMessage("Error", $"Error al generar backup de {tipo}: {ex.Message}", MessageType.Error);
        }
        finally
        {
            _progressBar.Hide();
            SetButtonsEnabled(true);
        }
    }

    private async Task AnimateProgress(double start, double end)
    {
        for (double i = start; i <= end; i += 0.05)
        {
            _progressBar.Fraction = i;
            await Task.Delay(50);
        }
    }

    private void SetButtonsEnabled(bool enabled)
    {
        _btnBackupUsuarios.Sensitive = enabled;
        _btnBackupVehiculos.Sensitive = enabled;
        _btnBackupRepuestos.Sensitive = enabled;
        _btnBackupCompleto.Sensitive = enabled;
    }

    private void UpdateStatus(string message)
    {
        _lblStatus.Text = message;
    }

    private void LoadBackupHistory()
    {
        _listStore.Clear();

        try
        {
            if (!Directory.Exists(_backupPath))
                return;

            DirectoryInfo dir = new DirectoryInfo(_backupPath);
            var files = dir.GetFiles("*.*").OrderByDescending(f => f.LastWriteTime);

            foreach (var file in files)
            {
                string tipo;
                if (file.Name.StartsWith("Usuarios_"))
                    tipo = "Usuarios (JSON)";
                else if (file.Name.StartsWith("Vehiculos_"))
                    tipo = "Vehículos (Huffman)";
                else if (file.Name.StartsWith("Repuestos_"))
                    tipo = "Repuestos (Huffman)";
                else
                    tipo = "Desconocido";

                string fecha = file.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss");
                string nombre = file.Name;
                string tamaño = FormatFileSize(file.Length);

                _listStore.AppendValues(fecha, tipo, nombre, tamaño);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar historial de backups: {ex.Message}");
            UpdateStatus($"Error al cargar historial: {ex.Message}");
        }
    }

    private async Task LoadBackupHistoryAsync()
    {
        await Task.Run(() => Application.Invoke(delegate { LoadBackupHistory(); }));
    }

    private string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int counter = 0;
        decimal number = bytes;

        while (Math.Round(number / 1024) >= 1)
        {
            number = number / 1024;
            counter++;
        }

        return string.Format("{0:n2} {1}", number, suffixes[counter]);
    }

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