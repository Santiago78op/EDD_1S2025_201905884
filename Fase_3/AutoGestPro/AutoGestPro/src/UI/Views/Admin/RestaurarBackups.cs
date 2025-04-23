using AutoGestPro.Core.Services;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.Admin;

/// <summary>
/// Ventana para la restauración de backups del sistema
/// </summary>
public class RestaurarBackups : Window
{
    private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/style.css";
    private const string WINDOW_TITLE = "Restaurar Backups - Administrador";

    private ComboBox _cmbUsuariosBackup;
    private ComboBox _cmbVehiculosBackup;
    private ComboBox _cmbRepuestosBackup;
    private Button _btnRestaurarUsuarios;
    private Button _btnRestaurarVehiculos;
    private Button _btnRestaurarRepuestos;
    private Button _btnRestaurarTodo;
    private ProgressBar _progressBar;
    private Label _lblStatus;
    private CssProvider _cssProvider;

    private readonly RestoreService _restoreService;
    private readonly string _backupPath;

    public RestaurarBackups() : base(WINDOW_TITLE)
    {
        InitializeWindow();
        InitializeCSS();

        // Inicializar el servicio de restauración
        _restoreService = new RestoreService();
        _backupPath = BackupService.GetBackupPath();

        CreateUI();
        LoadBackupFiles();
        ShowAll();
    }

    private void InitializeWindow()
    {
        SetDefaultSize(700, 500);
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
        var titleLabel = new Label("Restauración de Backups");
        titleLabel.AddCssClass("label-titulo");
        titleLabel.Xalign = 0;
        mainBox.PackStart(titleLabel, false, false, 0);

        // Descripción
        var descriptionLabel = new Label(
            "Este módulo permite restaurar copias de seguridad de las entidades del sistema.\n" +
            "Seleccione los archivos de backup que desea restaurar."
        );
        descriptionLabel.Xalign = 0;
        descriptionLabel.LineWrap = true;
        mainBox.PackStart(descriptionLabel, false, false, 8);

        // Ruta de la carpeta de backups
        var backupPathLabel = new Label($"Carpeta de backups: {_backupPath}");
        backupPathLabel.Xalign = 0;
        mainBox.PackStart(backupPathLabel, false, false, 8);

        // Panel de restauración
        var frame = new Frame("Selección de archivos de backup");
        var frameBox = new Box(Orientation.Vertical, 16);
        frameBox.BorderWidth = 16;

        // Sección de Usuarios
        var usuariosBox = new Box(Orientation.Horizontal, 8);
        var usuariosLabel = new Label("Backup de Usuarios:");
        usuariosLabel.SetSizeRequest(150, -1);
        _cmbUsuariosBackup = new ComboBox(new ListStore(typeof(string), typeof(string)));
        var usuariosCell = new CellRendererText();
        _cmbUsuariosBackup.PackStart(usuariosCell, true);
        _cmbUsuariosBackup.AddAttribute(usuariosCell, "text", 0);
        _cmbUsuariosBackup.SetSizeRequest(350, -1);

        _btnRestaurarUsuarios = new Button("Restaurar");
        _btnRestaurarUsuarios.AddCssClass("boton-primary");
        _btnRestaurarUsuarios.Clicked += OnRestaurarUsuariosClicked;

        usuariosBox.PackStart(usuariosLabel, false, false, 0);
        usuariosBox.PackStart(_cmbUsuariosBackup, true, true, 0);
        usuariosBox.PackStart(_btnRestaurarUsuarios, false, false, 0);

        frameBox.PackStart(usuariosBox, false, false, 0);

        // Sección de Vehículos
        var vehiculosBox = new Box(Orientation.Horizontal, 8);
        var vehiculosLabel = new Label("Backup de Vehículos:");
        vehiculosLabel.SetSizeRequest(150, -1);
        _cmbVehiculosBackup = new ComboBox(new ListStore(typeof(string), typeof(string)));
        var vehiculosCell = new CellRendererText();
        _cmbVehiculosBackup.PackStart(vehiculosCell, true);
        _cmbVehiculosBackup.AddAttribute(vehiculosCell, "text", 0);
        _cmbVehiculosBackup.SetSizeRequest(350, -1);

        _btnRestaurarVehiculos = new Button("Restaurar");
        _btnRestaurarVehiculos.AddCssClass("boton-primary");
        _btnRestaurarVehiculos.Clicked += OnRestaurarVehiculosClicked;

        vehiculosBox.PackStart(vehiculosLabel, false, false, 0);
        vehiculosBox.PackStart(_cmbVehiculosBackup, true, true, 0);
        vehiculosBox.PackStart(_btnRestaurarVehiculos, false, false, 0);

        frameBox.PackStart(vehiculosBox, false, false, 0);

        // Sección de Repuestos
        var repuestosBox = new Box(Orientation.Horizontal, 8);
        var repuestosLabel = new Label("Backup de Repuestos:");
        repuestosLabel.SetSizeRequest(150, -1);
        _cmbRepuestosBackup = new ComboBox(new ListStore(typeof(string), typeof(string)));
        var repuestosCell = new CellRendererText();
        _cmbRepuestosBackup.PackStart(repuestosCell, true);
        _cmbRepuestosBackup.AddAttribute(repuestosCell, "text", 0);
        _cmbRepuestosBackup.SetSizeRequest(350, -1);

        _btnRestaurarRepuestos = new Button("Restaurar");
        _btnRestaurarRepuestos.AddCssClass("boton-primary");
        _btnRestaurarRepuestos.Clicked += OnRestaurarRepuestosClicked;

        repuestosBox.PackStart(repuestosLabel, false, false, 0);
        repuestosBox.PackStart(_cmbRepuestosBackup, true, true, 0);
        repuestosBox.PackStart(_btnRestaurarRepuestos, false, false, 0);

        frameBox.PackStart(repuestosBox, false, false, 0);

        // Botón de restauración completa
        var restoreAllBox = new Box(Orientation.Horizontal, 8);
        var spacerLabel = new Label("");
        spacerLabel.SetSizeRequest(150, -1);

        _btnRestaurarTodo = new Button("Restaurar Todo");
        _btnRestaurarTodo.AddCssClass("boton-primary-success");
        _btnRestaurarTodo.Clicked += OnRestaurarTodoClicked;

        restoreAllBox.PackStart(spacerLabel, false, false, 0);
        restoreAllBox.PackStart(_btnRestaurarTodo, true, true, 0);

        frameBox.PackStart(restoreAllBox, false, false, 16);

        frame.Add(frameBox);
        mainBox.PackStart(frame, false, false, 0);

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

    private void LoadBackupFiles()
    {
        try
        {
            if (!Directory.Exists(_backupPath))
            {
                UpdateStatus("La carpeta de backups no existe. No hay backups disponibles.");
                return;
            }

            // Cargar archivos de backup de usuarios
            var usuariosModel = (ListStore)_cmbUsuariosBackup.Model;
            usuariosModel.Clear();

            var usuariosFiles = Directory.GetFiles(_backupPath, "Usuarios_*.json")
                .OrderByDescending(f => new FileInfo(f).LastWriteTime);

            foreach (var file in usuariosFiles)
            {
                string displayName = $"{System.IO.Path.GetFileName(file)} - {File.GetLastWriteTime(file)}";
                usuariosModel.AppendValues(displayName, file);
            }

            if (usuariosFiles.Any())
                _cmbUsuariosBackup.Active = 0;

            // Cargar archivos de backup de vehículos
            var vehiculosModel = (ListStore)_cmbVehiculosBackup.Model;
            vehiculosModel.Clear();

            var vehiculosFiles = Directory.GetFiles(_backupPath, "Vehiculos_*.edd")
                .OrderByDescending(f => new FileInfo(f).LastWriteTime);

            foreach (var file in vehiculosFiles)
            {
                string displayName = $"{System.IO.Path.GetFileName(file)} - {File.GetLastWriteTime(file)}";
                vehiculosModel.AppendValues(displayName, file);
            }

            if (vehiculosFiles.Any())
                _cmbVehiculosBackup.Active = 0;

            // Cargar archivos de backup de repuestos
            var repuestosModel = (ListStore)_cmbRepuestosBackup.Model;
            repuestosModel.Clear();

            var repuestosFiles = Directory.GetFiles(_backupPath, "Repuestos_*.edd")
                .OrderByDescending(f => new FileInfo(f).LastWriteTime);

            foreach (var file in repuestosFiles)
            {
                string displayName = $"{System.IO.Path.GetFileName(file)} - {File.GetLastWriteTime(file)}";
                repuestosModel.AppendValues(displayName, file);
            }

            if (repuestosFiles.Any())
                _cmbRepuestosBackup.Active = 0;

            // Habilitar o deshabilitar botones según disponibilidad de backups
            _btnRestaurarUsuarios.Sensitive = usuariosFiles.Any();
            _btnRestaurarVehiculos.Sensitive = vehiculosFiles.Any();
            _btnRestaurarRepuestos.Sensitive = repuestosFiles.Any();
            _btnRestaurarTodo.Sensitive = usuariosFiles.Any() || vehiculosFiles.Any() || repuestosFiles.Any();

            UpdateStatus(
                $"Se encontraron {usuariosFiles.Count()} backups de usuarios, {vehiculosFiles.Count()} de vehículos y {repuestosFiles.Count()} de repuestos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar archivos de backup: {ex.Message}");
            UpdateStatus($"Error al cargar archivos: {ex.Message}");
        }
    }

    private async void OnRestaurarUsuariosClicked(object sender, EventArgs e)
    {
        await RestaurarBackup(TipoBackup.Usuarios);
    }

    private async void OnRestaurarVehiculosClicked(object sender, EventArgs e)
    {
        await RestaurarBackup(TipoBackup.Vehiculos);
    }

    private async void OnRestaurarRepuestosClicked(object sender, EventArgs e)
    {
        await RestaurarBackup(TipoBackup.Repuestos);
    }

    private async void OnRestaurarTodoClicked(object sender, EventArgs e)
    {
        await RestaurarBackup(TipoBackup.Todo);
    }

    private enum TipoBackup
    {
        Usuarios,
        Vehiculos,
        Repuestos,
        Todo
    }

    private async Task RestaurarBackup(TipoBackup tipo)
    {
        try
        {
            _progressBar.Show();
            SetButtonsEnabled(false);

            string filePath = null;
            string tipoName = tipo.ToString();

            if (tipo != TipoBackup.Todo)
            {
                ComboBox comboBox;
                switch (tipo)
                {
                    case TipoBackup.Usuarios:
                        comboBox = _cmbUsuariosBackup;
                        break;
                    case TipoBackup.Vehiculos:
                        comboBox = _cmbVehiculosBackup;
                        break;
                    case TipoBackup.Repuestos:
                        comboBox = _cmbRepuestosBackup;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(tipo), tipo, null);
                }

                TreeIter iter;
                if (comboBox.GetActiveIter(out iter))
                {
                    filePath = (string)comboBox.Model.GetValue(iter, 1);
                }
                else
                {
                    UpdateStatus($"Debe seleccionar un archivo de backup de {tipoName}");
                    _progressBar.Hide();
                    SetButtonsEnabled(true);
                    return;
                }
            }

            UpdateStatus($"Restaurando backup de {tipoName}...");
            await AnimateProgress(0, 0.5);

            int count = 0;

            // Ejecutar la restauración según el tipo
            switch (tipo)
            {
                case TipoBackup.Usuarios:
                    count = await Task.Run(() => _restoreService.RestoreUsuarios(filePath));
                    break;
                case TipoBackup.Vehiculos:
                    count = await Task.Run(() => _restoreService.RestoreVehiculos(filePath));
                    break;
                case TipoBackup.Repuestos:
                    count = await Task.Run(() => _restoreService.RestoreRepuestos(filePath));
                    break;
                case TipoBackup.Todo:
                    var results = await Task.Run(() => _restoreService.RestoreAll());
                    int totalCount = results.Values.Sum();
                    string summary = string.Join(", ", results.Select(r => $"{r.Key}: {r.Value}"));

                    await AnimateProgress(0.5, 1.0);
                    UpdateStatus($"Restauración completa: {totalCount} elementos restaurados ({summary})");

                    ShowMessage("Restauración completa",
                        $"Se han restaurado un total de {totalCount} elementos:\n{summary}");

                    _progressBar.Hide();
                    SetButtonsEnabled(true);
                    return;
            }

            await AnimateProgress(0.5, 1.0);
            UpdateStatus($"Restauración de {tipoName} completada: {count} elementos restaurados");

            ShowMessage("Restauración completada",
                $"Se han restaurado {count} {tipoName.ToLower()} desde el archivo:\n{System.IO.Path.GetFileName(filePath)}");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error al restaurar: {ex.Message}");
            ShowMessage("Error de restauración",
                $"Se produjo un error al restaurar los datos:\n{ex.Message}",
                MessageType.Error);
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
        _btnRestaurarUsuarios.Sensitive = enabled && ((ListStore)_cmbUsuariosBackup.Model).IterNChildren() > 0;
        _btnRestaurarVehiculos.Sensitive = enabled && ((ListStore)_cmbVehiculosBackup.Model).IterNChildren() > 0;
        _btnRestaurarRepuestos.Sensitive = enabled && ((ListStore)_cmbRepuestosBackup.Model).IterNChildren() > 0;

        _btnRestaurarTodo.Sensitive = enabled && (
            ((ListStore)_cmbUsuariosBackup.Model).IterNChildren() > 0 ||
            ((ListStore)_cmbVehiculosBackup.Model).IterNChildren() > 0 ||
            ((ListStore)_cmbRepuestosBackup.Model).IterNChildren() > 0
        );
    }

    private void UpdateStatus(string message)
    {
        _lblStatus.Text = message;
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