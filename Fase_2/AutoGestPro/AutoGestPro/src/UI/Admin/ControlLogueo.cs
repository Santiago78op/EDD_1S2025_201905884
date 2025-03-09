using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;
using Gtk;
using Newtonsoft.Json;

namespace AutoGestPro.UI.Admin;

public class ControlLogueo : Window
{
    private TreeView _treeViewLogs;
    private ListStore _listStore;
    private Button _btnExportar;
    private static LinkedList _registrosLogueo = new LinkedList();

    public ControlLogueo() : base("Control de Logueo")
    {
        SetDefaultSize(600, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Hide(); };

        VBox vbox = new VBox(false, 5);

        // 🔹 Tabla para mostrar los registros de logueo
        _treeViewLogs = new TreeView();
        CrearColumnas();
        RefrescarLogs();

        ScrolledWindow scrollWindow = new ScrolledWindow();
        scrollWindow.Add(_treeViewLogs);
        vbox.PackStart(scrollWindow, true, true, 5);

        // 🔹 Botón para exportar a JSON
        _btnExportar = new Button("Exportar a JSON");
        _btnExportar.Clicked += OnExportarClicked;
        vbox.PackStart(_btnExportar, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    // ✅ Crear columnas para la tabla de logueo
    private void CrearColumnas()
    {
        _listStore = new ListStore(typeof(string), typeof(string), typeof(string));
        _treeViewLogs.Model = _listStore;

        _treeViewLogs.AppendColumn("Usuario", new CellRendererText(), "text", 0);
        _treeViewLogs.AppendColumn("Entrada", new CellRendererText(), "text", 1);
        _treeViewLogs.AppendColumn("Salida", new CellRendererText(), "text", 2);
    }

    // ✅ Refrescar la tabla con los registros de logueo
    private void RefrescarLogs()
    {
        _listStore.Clear();
        
        NodeLinked? current = _registrosLogueo.Head;
        
        while (current != null)
        {
            LogEntry log = (LogEntry)current.Data;
            _listStore.AppendValues(log.Usuario, log.Entrada, log.Salida);
            current = current.Next;
        }
    }

    // ✅ Registrar la entrada de un usuario
    public static void RegistrarEntrada(string usuario)
    {
        _registrosLogueo.Append(new LogEntry
        {
            Usuario = usuario,
            Entrada = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Salida = "En sesión"
        });
    }

    // ✅ Registrar la salida de un usuario
    public static void RegistrarSalida(string usuario)
    {
        NodeLinked? current = _registrosLogueo.Head;
        
        while (current != null)
        {
            LogEntry log = (LogEntry)current.Data;
            if (log.Usuario == usuario && log.Salida == "En sesión")
            {
                log.Salida = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                break;
            }
            current = current.Next;
        }
    }

    // ✅ Exportar el log de entrada y salida en formato JSON
    private void OnExportarClicked(object? sender, EventArgs e)
    {
        FileChooserDialog fileChooser = new FileChooserDialog(
            "Guardar Logueo en JSON",
            this,
            FileChooserAction.Save,
            "Cancelar", ResponseType.Cancel,
            "Guardar", ResponseType.Accept
        );

        fileChooser.SetFilename("Logueo.json");

        if (fileChooser.Run() == (int)ResponseType.Accept)
        {
            string rutaArchivo = fileChooser.Filename;
            string json = JsonConvert.SerializeObject(_registrosLogueo, Formatting.Indented);
            File.WriteAllText(rutaArchivo, json);

            MostrarMensaje("Éxito", "Logueo exportado exitosamente.");
        }

        fileChooser.Destroy();
    }

    // ✅ Mostrar un mensaje en pantalla
    private void MostrarMensaje(string titulo, string mensaje)
    {
        MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
        dialog.Title = titulo;
        dialog.Run();
        dialog.Destroy();
    }

    // ✅ Modelo de datos para el registro de logueo
    public class LogEntry
    {
        public string Usuario { get; set; }
        public string Entrada { get; set; }
        public string Salida { get; set; }
    }
}