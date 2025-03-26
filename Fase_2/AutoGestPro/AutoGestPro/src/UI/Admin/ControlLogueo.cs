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

    // Obtener la ruta absoluta de la carpeta raíz del proyecto
    private static string _rutaProyecto = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

    // Combinar con la carpeta de reportes
    private string _rutaReportes = System.IO.Path.Combine(_rutaProyecto, "Reports");

    public ControlLogueo() : base("Control de Logueo")
    {
        SetDefaultSize(600, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Hide(); };
        // Crear la carpeta "Reportes" si no existe
        if (!Directory.Exists(_rutaReportes))
        {
            Directory.CreateDirectory(_rutaReportes);
        }

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

    public void makeJson(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
                Console.WriteLine($"Archivo JSON creado con éxito en: {path}");
            }
            else
            {
                Console.WriteLine($"El archivo ya existe en: {path}");
                Console.WriteLine("¿Desea reemplazar el archivo existente? (s/n)");
                string respuesta = Console.ReadLine();
                if (respuesta?.ToLower() == "s")
                {
                    File.WriteAllText(path, "[]");
                    Console.WriteLine($"Archivo JSON reemplazado con éxito en: {path}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Error al generar el archivo: {ex.Message}");
        }
    }

    // ✅ Exportar el log de entrada y salida en formato JSON
    private void OnExportarClicked(object? sender, EventArgs e)
    {
        string path = $"{_rutaReportes}/logs.json";

        try
        {
            if (!File.Exists(path))
            {
                makeJson(path);
            }

            string json = File.ReadAllText(path);

            List<Dictionary<string, string>> registros =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json) ??
                new List<Dictionary<string, string>>();

            // Convertir la lista de registros a un formato compatible con el modelo
            NodeLinked? current = _registrosLogueo.Head;

            while (current != null)
            {
                LogEntry log = (LogEntry)current.Data;
                registros.Add(new Dictionary<string, string>
                {
                    { "Usuario", log.Usuario },
                    { "Entrada", log.Entrada },
                    { "Salida", log.Salida }
                });
                current = current.Next;
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(registros, Formatting.Indented));
            MostrarMensaje("Éxito", $"Registros exportados a: {path}");
        }
        catch (UnauthorizedAccessException ex)
        {
            MostrarMensaje("Error", $"Acceso denegado a la ruta: {path}. Detalles: {ex.Message}");
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error", $"Error al exportar los registros: {ex.Message}");
        }
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