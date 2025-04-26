using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Services;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.User
{
    /// <summary>
    /// Clase para la visualización de servicios de un usuario específico.
    /// </summary>
    public class VisualizacionServicios : Window
    {
        private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/style.css";

        private TreeView _treeViewServicios;
        private ListStore _listStore;
        private ComboBoxText _comboOrden;
        private Button _btnActualizar;
        private CssProvider _cssProvider;
        
        /// <summary>
        /// Constructor de la ventana de visualización de servicios.
        /// </summary>
        public VisualizacionServicios() : base("Mis Servicios")
        {
            SetDefaultSize(800, 600);
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
        /// Crea la interfaz de usuario para la visualización de servicios.
        /// </summary>
        private void CreateUI()
        {
            VBox vbox = new VBox(false, 10)
            {
                BorderWidth = 15
            };

            // Título de la ventana
            Label lblTitulo = new Label($"Servicios para {Sesion.UsuarioActual?.Nombres} {Sesion.UsuarioActual?.Apellidos}");
            lblTitulo.AddCssClass("label-titulo");
            vbox.PackStart(lblTitulo, false, false, 5);

            // ComboBox para seleccionar el orden
            HBox hboxOrden = new HBox(false, 5);
            Label lblOrden = new Label("Selecciona el orden:");
            lblOrden.AddCssClass("form-label");

            _comboOrden = new ComboBoxText();
            _comboOrden.AppendText("Pre-orden");
            _comboOrden.AppendText("In-orden");
            _comboOrden.AppendText("Post-orden");
            _comboOrden.Active = 1; // Por defecto, In-orden
            _comboOrden.AddCssClass("entry");

            hboxOrden.PackStart(lblOrden, false, false, 5);
            hboxOrden.PackStart(_comboOrden, false, false, 5);
            vbox.PackStart(hboxOrden, false, false, 5);

            // Botón para actualizar
            _btnActualizar = new Button("🔄 Actualizar Vista");
            _btnActualizar.Clicked += OnActualizarClicked;
            _btnActualizar.AddCssClass("boton");
            vbox.PackStart(_btnActualizar, false, false, 10);

            // Tabla de servicios
            _treeViewServicios = new TreeView();
            _treeViewServicios.Selection.Mode = SelectionMode.None; // Desactivar selección
            CrearColumnas();

            ScrolledWindow scroll = new ScrolledWindow();
            scroll.Add(_treeViewServicios);
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.BorderWidth = 10;
            vbox.PackStart(scroll, true, true, 0);

            Add(vbox);
            ActualizarLista();
        }
        
        /// <summary>
        /// Crea las columnas del TreeView para mostrar los servicios.
        /// </summary>
        private void CrearColumnas()
        {
            _listStore = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(string));
            _treeViewServicios.Model = _listStore;

            _treeViewServicios.AppendColumn("ID", new CellRendererText(), "text", 0);
            _treeViewServicios.AppendColumn("Vehículo", new CellRendererText(), "text", 1);
            _treeViewServicios.AppendColumn("Repuesto", new CellRendererText(), "text", 2);
            _treeViewServicios.AppendColumn("Detalles", new CellRendererText(), "text", 3);
            _treeViewServicios.AppendColumn("Costo", new CellRendererText(), "text", 4);
        }
        
        /// <summary>
        /// Actualiza la lista de servicios en el TreeView según el método de orden seleccionado.
        /// </summary>
        private void ActualizarLista()
        {
            _listStore.Clear();

            // Obtener los servicios según el orden seleccionado
            string metodo = _comboOrden.ActiveText;
            List<object> servicios = new List<object>();
            
            if (metodo == "Pre-orden")
            {
                servicios = Estructuras.Servicios.PreOrder();
            }
            else if (metodo == "In-orden")
            {
                servicios = Estructuras.Servicios.InOrder();
            }
            else if (metodo == "Post-orden")
            {
                servicios = Estructuras.Servicios.PostOrder();
            }

            // Filtrar los servicios por usuario actual
            if (Sesion.UsuarioActual != null)
            {
                foreach (var servicioObj in servicios)
                {
                    if (servicioObj is Servicio servicio)
                    {
                        // Verificar si el servicio pertenece a un vehículo del usuario actual
                        bool esVehiculoDeUsuario = false;
                        
                        // Recorrer los vehículos para encontrar coincidencias
                        var current = Estructuras.Vehiculos.Head;
                        while (current != null)
                        {
                            if (current.Data is Vehiculo vehiculo && 
                                vehiculo.IdUsuario == Sesion.UsuarioActual.Id && 
                                vehiculo.Id == servicio.IdVehiculo)
                            {
                                esVehiculoDeUsuario = true;
                                AgregarServicio(servicio, vehiculo.Marca);
                                break;
                            }
                            current = current.Next;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Agrega un servicio al ListStore del TreeView con información adicional del vehículo.
        /// </summary>
        /// <param name="servicio">Objeto del tipo Servicio a agregar.</param>
        /// <param name="marcaVehiculo">Marca del vehículo asociado al servicio.</param>
        private void AgregarServicio(Servicio servicio, string marcaVehiculo)
        {
            // Intentar obtener información del repuesto
            string nombreRepuesto = "Repuesto #" + servicio.IdRepuesto;
            var repuesto = Estructuras.Repuestos.Search(servicio.IdRepuesto);
            if (repuesto != null && repuesto is Repuesto r)
            {
                nombreRepuesto = r.Repuesto1;
            }

            _listStore.AppendValues(
                servicio.Id, 
                marcaVehiculo, 
                nombreRepuesto, 
                servicio.Detalles, 
                $"Q{servicio.Costo}"
            );
        }
        
        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón de actualizar.
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void OnActualizarClicked(object sender, EventArgs e)
        {
            ActualizarLista();
        }
    }
}