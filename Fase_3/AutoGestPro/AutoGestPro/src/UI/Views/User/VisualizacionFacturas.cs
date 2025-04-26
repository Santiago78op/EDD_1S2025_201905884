using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Services;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.User
{
    /// <summary>
    /// Clase para la visualización de facturas de un usuario específico.
    /// </summary>
    public class VisualizacionFacturas : Window
    {
        private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/style.css";

        private TreeView _treeViewFacturas;
        private ListStore _listStore;
        private CssProvider _cssProvider;
        
        /// <summary>
        /// Constructor de la ventana de visualización de facturas.
        /// </summary>
        public VisualizacionFacturas() : base("Mis Facturas")
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
        /// Crea la interfaz de usuario para la visualización de facturas.
        /// </summary>
        private void CreateUI()
        {
            VBox vbox = new VBox(false, 10)
            {
                BorderWidth = 15
            };

            // Título de la ventana
            Label lblTitulo = new Label($"Facturas de {Sesion.UsuarioActual?.Nombres} {Sesion.UsuarioActual?.Apellidos}");
            lblTitulo.AddCssClass("label-titulo");
            vbox.PackStart(lblTitulo, false, false, 5);

            // Espacio para el título
            vbox.PackStart(new Label(""), false, false, 5);

            // Tabla de facturas
            _treeViewFacturas = new TreeView();
            _treeViewFacturas.Selection.Mode = SelectionMode.None; // Desactivar selección
            CrearColumnas();

            ScrolledWindow scroll = new ScrolledWindow();
            scroll.Add(_treeViewFacturas);
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.BorderWidth = 10;
            vbox.PackStart(scroll, true, true, 0);

            Add(vbox);
            ActualizarLista();
        }
        
        /// <summary>
        /// Crea las columnas del TreeView para mostrar las facturas.
        /// </summary>
        private void CrearColumnas()
        {
            _listStore = new ListStore(
                typeof(int),       // ID de factura
                typeof(string),    // Fecha de creación
                typeof(string),    // Servicio
                typeof(string),    // Vehículo
                typeof(string),    // Total
                typeof(string)     // Método de pago
            );
            _treeViewFacturas.Model = _listStore;

            _treeViewFacturas.AppendColumn("ID", new CellRendererText(), "text", 0);
            _treeViewFacturas.AppendColumn("Fecha", new CellRendererText(), "text", 1);
            _treeViewFacturas.AppendColumn("Servicio", new CellRendererText(), "text", 2);
            _treeViewFacturas.AppendColumn("Vehículo", new CellRendererText(), "text", 3);
            _treeViewFacturas.AppendColumn("Total", new CellRendererText(), "text", 4);
            _treeViewFacturas.AppendColumn("Método de Pago", new CellRendererText(), "text", 5);
        }
        
        /// <summary>
        /// Actualiza la lista de facturas en el TreeView mostrando solo las del usuario actual.
        /// </summary>
        private void ActualizarLista()
        {
            _listStore.Clear();
            
            if (Sesion.UsuarioActual == null) return;
            
            int usuarioId = Sesion.UsuarioActual.Id;
            List<object> facturas = new List<object>();
            
            // Acción para recopilar facturas del usuario actual
            Action<object> recopilarFactura = (facturaObj) => {
                if (facturaObj is Factura factura && factura.IdUsuario == usuarioId)
                {
                    facturas.Add(factura);
                }
            };
            
            // Recorrer el árbol Merkle para obtener todas las facturas
            Estructuras.Facturas.InOrder(recopilarFactura);
            
            // Agregar las facturas encontradas al ListStore
            foreach (var facturaObj in facturas)
            {
                if (facturaObj is Factura factura)
                {
                    AgregarFactura(factura);
                }
            }
        }

        /// <summary>
        /// Agrega una factura al ListStore del TreeView con información adicional del servicio y vehículo.
        /// </summary>
        /// <param name="factura">Objeto del tipo Factura a agregar.</param>
        private void AgregarFactura(Factura factura)
        {
            // Obtener información del servicio asociado
            string servicioInfo = "Servicio #" + factura.IdServicio;
            string vehiculoInfo = "No disponible";
            
            var servicio = Estructuras.Servicios.Search(factura.IdServicio);
            if (servicio != null && servicio is Servicio s)
            {
                servicioInfo = s.Detalles;
                
                // Obtener información del vehículo asociado al servicio
                var current = Estructuras.Vehiculos.Head;
                while (current != null)
                {
                    if (current.Data is Vehiculo v && v.Id == s.IdVehiculo)
                    {
                        vehiculoInfo = $"{v.Marca} - {v.Modelo} - {v.Placa}";
                        break;
                    }
                    current = current.Next;
                }
            }
            
            // Formatear la fecha
            string fechaFormateada = factura.FechaCreacion.ToString("dd/MM/yyyy HH:mm");
            
            // Agregar la factura a la tabla
            _listStore.AppendValues(
                factura.Id,
                fechaFormateada,
                servicioInfo,
                vehiculoInfo,
                $"Q{factura.Total}",
                factura.MetodoPago.ToString()
            );
        }
    }
}