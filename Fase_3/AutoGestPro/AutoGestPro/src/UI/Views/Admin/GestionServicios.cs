using System;
using System.IO;
using System.Threading.Tasks;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using Gtk;

namespace AutoGestPro.UI.Views.Admin;

/// <summary>
/// Ventana para la gestión y generación de servicios en el sistema.
/// Esta ventana pertenece al menú de administración del sistema.
/// </summary>
public class GestionServicios : Window
{
    #region Private Fields

    // Constantes
    private const string CSS_FILE_PATH = "../../../src/UI/Assets/Styles/style.css";
    private const string WINDOW_TITLE = "Gestión de Servicios - Administrador";

    // UI Components
    private Entry _entryId;
    private Entry _entryIdVehiculo;
    private Entry _entryIdRepuesto;
    private Entry _entryDetalle;
    private Entry _entryCosto;
    private ComboBox _cmbMetodoPago;
    private Button _btnGenerarServicio;
    private Button _btnLimpiar;
    private Label _lblStatus;
    private TreeView _treeViewServicios;
    private ListStore _listStore;
    private ProgressBar _progressBar;
    private CssProvider _cssProvider;
    private Stack _stack;
    private Box _mainBox;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor de la ventana de gestión de servicios
    /// </summary>
    public GestionServicios() : base(WINDOW_TITLE)
    {
        InitializeWindow();
        InitializeCSS();
        CreateUI();
        RefrescarServicios();
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

        var logo = new Label("🔧");
        logo.SetSizeRequest(40, 40);

        var titleLabel = new Label("Gestión de Servicios");
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
        CreateServiceTable(rightBox);

        cardBox.PackStart(leftBox, true, true, 0);
        cardBox.PackStart(rightBox, true, true, 0);

        contentBox.PackStart(cardBox, true, true, 0);
        _mainBox.PackStart(contentBox, true, true, 0);
    }

    /// <summary>
    /// Crea el formulario de generación de servicios
    /// </summary>
    private void CreateForm(Box container)
    {
        var formTitle = new Label("Datos del Nuevo Servicio");
        formTitle.StyleContext.AddClass("subtitle");
        formTitle.Xalign = 0;
        container.PackStart(formTitle, false, false, 0);

        var formBox = new Box(Orientation.Vertical, 8);

        var idBox = CreateFormField("ID:", out _entryId);
        formBox.PackStart(idBox, false, false, 8);

        var idVehiculoBox = CreateFormField("ID Vehículo:", out _entryIdVehiculo);
        formBox.PackStart(idVehiculoBox, false, false, 8);

        var idRepuestoBox = CreateFormField("ID Repuesto:", out _entryIdRepuesto);
        formBox.PackStart(idRepuestoBox, false, false, 8);

        var detalleBox = CreateFormField("Detalle:", out _entryDetalle);
        formBox.PackStart(detalleBox, false, false, 8);

        var costoBox = CreateFormField("Costo:", out _entryCosto);
        formBox.PackStart(costoBox, false, false, 8);

        // Crear ComboBox para método de pago
        var metodoPagoBox = new Box(Orientation.Horizontal, 8);
        metodoPagoBox.StyleContext.AddClass("form-field");

        var lblMetodoPago = new Label("Método de Pago:");
        lblMetodoPago.Xalign = 0;
        lblMetodoPago.SetSizeRequest(150, -1);
        lblMetodoPago.StyleContext.AddClass("form-label");

        // Crear el modelo de datos para el ComboBox
        var listStore = new ListStore(typeof(string));
        listStore.AppendValues("Efectivo");
        listStore.AppendValues("Tarjeta De Credito");
        listStore.AppendValues("Tarjeta De Debito");

        // Crear el ComboBox con el modelo de datos
        _cmbMetodoPago = new ComboBox(listStore);

        // Configurar el renderizador para mostrar texto
        var cellRenderer = new CellRendererText();
        _cmbMetodoPago.PackStart(cellRenderer, true);
        _cmbMetodoPago.AddAttribute(cellRenderer, "text", 0);

        // Seleccionar el primer elemento por defecto
        _cmbMetodoPago.Active = 0;
        _cmbMetodoPago.StyleContext.AddClass("form-entry");

        metodoPagoBox.PackStart(lblMetodoPago, false, false, 0);
        metodoPagoBox.PackStart(_cmbMetodoPago, true, true, 0);

        formBox.PackStart(metodoPagoBox, false, false, 8);

        container.PackStart(formBox, true, true, 0);

        // Barra de progreso
        _progressBar = new ProgressBar();
        _progressBar.Fraction = 0.0;
        _progressBar.NoShowAll = true;
        container.PackStart(_progressBar, false, false, 0);

        // Botones
        var botonesBox = new Box(Orientation.Horizontal, 8);

        _btnGenerarServicio = new Button("💾 Generar Servicio");
        _btnGenerarServicio.StyleContext.AddClass("boton-primary");
        _btnGenerarServicio.Clicked += OnGenerarServicioClicked;

        _btnLimpiar = new Button("Limpiar");
        _btnLimpiar.StyleContext.AddClass("boton-secondary");
        _btnLimpiar.Clicked += OnLimpiarClicked;

        botonesBox.PackStart(_btnGenerarServicio, true, true, 0);
        botonesBox.PackStart(_btnLimpiar, true, true, 0);

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
    /// Crea la tabla de servicios
    /// </summary>
    private void CreateServiceTable(Box container)
    {
        var tableTitle = new Label("Servicios Registrados");
        tableTitle.StyleContext.AddClass("subtitle");
        tableTitle.Xalign = 0;
        container.PackStart(tableTitle, false, false, 0);

        _listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string),
            typeof(string), typeof(string));

        _treeViewServicios = new TreeView(_listStore)
        {
            HeadersVisible = true
        };

        _treeViewServicios.StyleContext.AddClass("tree-view");

        AddTreeViewColumn("ID", 0);
        AddTreeViewColumn("ID Usuario", 1);
        AddTreeViewColumn("ID Vehículo", 2);
        AddTreeViewColumn("ID Repuesto", 3);
        AddTreeViewColumn("Detalle", 4);
        AddTreeViewColumn("Costo", 5);
        AddTreeViewColumn("Método Pago", 6);

        var scroll = new ScrolledWindow();
        scroll.ShadowType = ShadowType.EtchedIn;
        scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        scroll.SetSizeRequest(-1, 400);
        scroll.Add(_treeViewServicios);

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
        _treeViewServicios.AppendColumn(column);
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Manejador de evento para el botón Generar Servicio
    /// </summary>
    private async void OnGenerarServicioClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_entryId.Text) ||
                string.IsNullOrWhiteSpace(_entryIdVehiculo.Text) ||
                string.IsNullOrWhiteSpace(_entryIdRepuesto.Text) ||
                string.IsNullOrWhiteSpace(_entryDetalle.Text) ||
                string.IsNullOrWhiteSpace(_entryCosto.Text))
            {
                UpdateStatus("Todos los campos son obligatorios", true);
                return;
            }

            if (!int.TryParse(_entryId.Text, out int id) || id <= 0)
            {
                UpdateStatus("El ID debe ser un número entero positivo", true);
                return;
            }

            if (!int.TryParse(_entryIdVehiculo.Text, out int idVehiculo) || idVehiculo <= 0)
            {
                UpdateStatus("El ID Vehículo debe ser un número entero positivo", true);
                return;
            }

            if (!int.TryParse(_entryIdRepuesto.Text, out int idRepuesto) || idRepuesto <= 0)
            {
                UpdateStatus("El ID Repuesto debe ser un número entero positivo", true);
                return;
            }

            if (!decimal.TryParse(_entryCosto.Text, out decimal costo) || costo <= 0)
            {
                UpdateStatus("El costo debe ser un valor decimal positivo", true);
                return;
            }

            // Obtener método de pago seleccionado
            TreeIter iter;
            if (!_cmbMetodoPago.GetActiveIter(out iter))
            {
                UpdateStatus("Seleccione un método de pago", true);
                return;
            }

            string metodoPagoText = (string)_cmbMetodoPago.Model.GetValue(iter, 0);
            MetodoPago metodoPagoFactura = MetodoPago.Efectivo; // Valor por defecto

            switch (metodoPagoText)
            {
                case "Efectivo":
                    metodoPagoFactura = MetodoPago.Efectivo;
                    break;
                case "TarjetaDeCredito":
                    metodoPagoFactura = MetodoPago.TarjetaDeCredito;
                    break;
                case "TarjetaDeDebito":
                    metodoPagoFactura = MetodoPago.TarjetaDeDebito;
                    break;
            }

            // Mostrar progreso
            _progressBar.Show();
            UpdateStatus("Generando servicio...", false);
            _btnGenerarServicio.Sensitive = false;

            // Animación de progreso
            await AnimateProgress();

            // Verificar si el servicio ya existe
            var servicioExistente = Estructuras.Servicios.Search(id);
            if (servicioExistente != null)
            {
                UpdateStatus("Error: El servicio ya existe.", true);
                _progressBar.Hide();
                _btnGenerarServicio.Sensitive = true;
                return;
            }

            // Verificar si el vehículo y repuesto existen
            var nodoVehiculo = Estructuras.Vehiculos.SearchNode(idVehiculo);
            if (nodoVehiculo == null)
            {
                UpdateStatus("Error: El vehículo no existe.", true);
                _progressBar.Hide();
                _btnGenerarServicio.Sensitive = true;
                return;
            }
            
            // Crea nodo de tipo vehículo, para el grafo no dirigido
            Vehiculo dataVehiculo = (Vehiculo)nodoVehiculo.Data;

            var nodoRepuesto = Estructuras.Repuestos.Search(idRepuesto);
            if (nodoRepuesto == null)
            {
                UpdateStatus("Error: El repuesto no existe.", true);
                _progressBar.Hide();
                _btnGenerarServicio.Sensitive = true;
                return;
            }
            
            // Crea nodo de tipo repuesto, para el grafo no dirigido
            Repuesto dataRepuesto = nodoRepuesto;
    
            // Crear nuevo servicio con método de pago
            Servicio nuevoServicio = new Servicio(id,idRepuesto, idVehiculo, _entryDetalle.Text.Trim(), costo)
            {
                IdUsuario = dataVehiculo.IdUsuario
            };
            
            // Insertar en el árbol AVL global
            Estructuras.Servicios.Insert(id, nuevoServicio);

            // Agregar el servicio al usuario correspondiente
            Usuario usuario = Estructuras.Clientes.BuscarUsuarioPorId(dataVehiculo.IdUsuario);
            if (usuario != null)
            {
                Estructuras.Servicios.Insert(id, nuevoServicio);

                // Creación de la factura
                decimal costoTotal = costo + dataRepuesto.Costo;

                // Crear objeto factura
                Factura nuevaFactura = new Factura(id, dataVehiculo.IdUsuario, id, costoTotal);
                
                // Establecer método de pago
                nuevaFactura.EstablecerMetodoPago((AutoGestPro.Core.Global.MetodoPago)metodoPagoFactura);
                
                // Insertar factura en la lista de facturas del cliente
                // Guardar la factura en el Árbol de Merkle 
                Estructuras.Facturas.Insert(nuevaFactura.Id, nuevaFactura);

                // Establecimeinto de la relación en grafo no dirigido entre id vehículo y id repuesto.
                // Agregar relación entre vehículo y repuesto en el grafo no dirigido
                Estructuras.Gestor.RegistrarCompatibilidad("V"+dataVehiculo.Id, "R"+dataRepuesto.Id);   
                
                // Actualizar interfaz
                _listStore.AppendValues(
                    id.ToString(),
                    dataVehiculo.IdUsuario.ToString(),
                    idVehiculo.ToString(),
                    idRepuesto.ToString(),
                    _entryDetalle.Text.Trim(),
                    costo.ToString(),
                    metodoPagoFactura.ToString()
                );

                UpdateStatus("Servicio y factura generados correctamente", false);
                _btnGenerarServicio.StyleContext.RemoveClass("boton-primary");
                _btnGenerarServicio.StyleContext.AddClass("boton-primary-success");
                LimpiarFormulario();
            }
            else
            {
                UpdateStatus("Error: Usuario no encontrado.", true);
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
            _btnGenerarServicio.Sensitive = true;
        }
    }

    /// <summary>
    /// Manejador de evento para el botón Limpiar
    /// </summary>
    private void OnLimpiarClicked(object sender, EventArgs e)
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
    /// Método para actualizar la visualización de los servicios
    /// </summary>
    private void RefrescarServicios()
    {
        _listStore.Clear();

        try
        {
            var serviciosList = Estructuras.Servicios.InOrder();
            if (serviciosList != null && serviciosList.Count > 0)
            {
                foreach (var item in serviciosList)
                {
                    if (item != null)
                    {
                        Servicio servicio = (Servicio)item;
                        // Buscar Factura
                        Factura factura = Estructuras.Facturas.Search(servicio.Id) as Factura;
                        _listStore.AppendValues(
                            servicio.Id.ToString(),
                            servicio.IdUsuario.ToString(),
                            servicio.IdVehiculo.ToString(),
                            servicio.IdRepuesto.ToString(),
                            servicio.Detalles,
                            servicio.Costo.ToString(),
                            factura.MetodoPago.ToString()
                        );
                    }
                }

                UpdateStatus($"Se cargaron {serviciosList.Count} servicios", false);
            }
            else
            {
                UpdateStatus("No hay servicios registrados en el sistema", false);
            }
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error al cargar servicios: {ex.Message}", true);
        }
    }

    /// <summary>
    /// Limpia los campos del formulario
    /// </summary>
    private void LimpiarFormulario()
    {
        _entryId.Text = "";
        _entryIdVehiculo.Text = "";
        _entryIdRepuesto.Text = "";
        _entryDetalle.Text = "";
        _entryCosto.Text = "";
        _cmbMetodoPago.Active = 0;

        // Restaurar estilo del botón guardar
        _btnGenerarServicio.StyleContext.RemoveClass("boton-primary-success");
        _btnGenerarServicio.StyleContext.AddClass("boton-primary");
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

/// <summary>
/// Representa los métodos de pago disponibles.
/// </summary>
public enum MetodoPago
{
    Efectivo,
    TarjetaDeCredito,
    TarjetaDeDebito
}