using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Services;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.User
{
    /// <summary>
    /// Clase para la visualización de vehículos de un usuario específico.
    /// </summary>
    public class VisualizacionVehiculos : Window
    {
        private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/style.css";

        private TreeView _treeViewVehiculos;
        private ListStore _listStore;
        private Button _btnVisualizar;
        private CssProvider _cssProvider;
        
        /// <summary>
        /// Constructor de la ventana de visualización de vehículos.
        /// </summary>
        public VisualizacionVehiculos() : base("Mis Vehículos")
        {
            SetDefaultSize(800, 500);
            SetPosition(WindowPosition.Center);
            DeleteEvent += (o, args) => Hide();

            InitializeCSS();
            CreateUI();
            ShowAll();
        }
        
        /// <summary>
        /// Inicializa y aplica el archivo CSS para la ventana.
        /// </summary>
        private void InitializeCSS()
        {
            _cssProvider = new CssProvider();

            try
            {
                var cssPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CSS_FILE_PATH);
                if (System.IO.File.Exists(cssPath))
                {
                    _cssProvider.LoadFromPath(cssPath);
                    StyleContext.AddProviderForScreen(Gdk.Screen.Default, _cssProvider, 800);
                }
                else
                {
                    Console.WriteLine($"Archivo CSS no encontrado: {cssPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar CSS: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Crea la interfaz de usuario para la visualización de vehículos.
        /// </summary>
        private void CreateUI()
        {
            VBox vbox = new VBox(false, 10)
            {
                BorderWidth = 15
            };

            // Título de la ventana
            Label lblTitulo = new Label($"Vehículos registrados para {Sesion.UsuarioActual?.Nombres} {Sesion.UsuarioActual?.Apellidos}");
            lblTitulo.AddCssClass("label-titulo");
            vbox.PackStart(lblTitulo, false, false, 5);

            // Espacio para el título
            vbox.PackStart(new Label(""), false, false, 5);

            // Tabla de vehículos
            _treeViewVehiculos = new TreeView();
            _treeViewVehiculos.Selection.Mode = SelectionMode.None; // Desactivar selección
            CrearColumnas();

            ScrolledWindow scroll = new ScrolledWindow();
            scroll.Add(_treeViewVehiculos);
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.BorderWidth = 10;
            vbox.PackStart(scroll, true, true, 0);

            Add(vbox);
            ActualizarLista();
        }
        
        /// <summary>
        /// Crea las columnas del TreeView para mostrar los vehículos.
        /// </summary>
        private void CrearColumnas()
        {
            _listStore = new ListStore(typeof(int), typeof(string), typeof(int), typeof(string));
            _treeViewVehiculos.Model = _listStore;

            _treeViewVehiculos.AppendColumn("ID", new CellRendererText(), "text", 0);
            _treeViewVehiculos.AppendColumn("Marca", new CellRendererText(), "text", 1);
            _treeViewVehiculos.AppendColumn("Modelo", new CellRendererText(), "text", 2);
            _treeViewVehiculos.AppendColumn("Placa", new CellRendererText(), "text", 3);
        }
        
        /// <summary>
        /// Actualiza la lista de vehículos en el TreeView mostrando solo los del usuario actual.
        /// </summary>
        private void ActualizarLista()
        {
            _listStore.Clear();
            
            if (Sesion.UsuarioActual == null) return;
            
            int usuarioId = Sesion.UsuarioActual.Id;
            NodeDouble current = Estructuras.Vehiculos.Head;
            
            while (current != null)
            {
                if (current.Data is Vehiculo vehiculo && vehiculo.IdUsuario == usuarioId)
                {
                    _listStore.AppendValues(vehiculo.Id, vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
                }
                current = current.Next;
            }
        }
        

    }
}