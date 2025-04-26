using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.UI.Extensions;
using Gtk;

namespace AutoGestPro.UI.Views.Admin;

/// <summary>
/// Clase para la visualización de repuestos en una ventana.
/// </summary>
public class VisualizacionRepuestos : Window
{
    private TreeView _treeViewRepuestos;
    private ListStore _listStore;
    private ComboBoxText _comboOrden;
    private Button _btnActualizar;
    
    public VisualizacionRepuestos() : base("Visualización de Repuestos")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide();

        ApplyStyles(); // Cambio aquí: usado el mismo nombre de método que en MainWindow
        CreateUI();
        ShowAll();
    }
    
    /// <summary>
    /// Aplica estilos CSS a la ventana.
    /// </summary>
    private void ApplyStyles()
    {
        var cssProvider = new CssProvider();
        string cssPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "../../../src/UI/Assets/Styles/style.css");
        cssProvider.LoadFromPath(cssPath);
        StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssProvider, 800);
    }
    
    /// <summary>
    /// Crea la interfaz de usuario para la visualización de repuestos.
    /// </summary>
    private void CreateUI()
    {
        VBox vbox = new VBox(false, 10)
        {
            BorderWidth = 15
        };

        // ComboBox para seleccionar el orden
        HBox hboxOrden = new HBox(false, 5);
        Label lblOrden = new Label("Selecciona el orden:");
        lblOrden.AddCssClass("form-label");

        _comboOrden = new ComboBoxText();
        _comboOrden.AppendText("Pre-orden");
        _comboOrden.AppendText("In-orden");
        _comboOrden.AppendText("Post-orden");
        _comboOrden.Active = 1;
        _comboOrden.AddCssClass("entry");

        hboxOrden.PackStart(lblOrden, false, false, 5);
        hboxOrden.PackStart(_comboOrden, false, false, 5);
        vbox.PackStart(hboxOrden, false, false, 5);

        // Botón para actualizar
        _btnActualizar = new Button("🔄 Actualizar Vista");
        _btnActualizar.Clicked += OnActualizarClicked;
        _btnActualizar.AddCssClass("boton");
        vbox.PackStart(_btnActualizar, false, false, 10);

        // Tabla de repuestos
        _treeViewRepuestos = new TreeView();
        CrearColumnas();

        ScrolledWindow scroll = new ScrolledWindow();
        scroll.Add(_treeViewRepuestos);
        scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        scroll.BorderWidth = 10;
        vbox.PackStart(scroll, true, true, 0);

        Add(vbox);
        ActualizarLista();
    }
    
    /// <summary>
    /// Crea las columnas del TreeView para mostrar los repuestos.
    /// </summary>
    private void CrearColumnas()
    {
        _listStore = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
        _treeViewRepuestos.Model = _listStore;

        _treeViewRepuestos.AppendColumn("ID", new CellRendererText(), "text", 0);
        _treeViewRepuestos.AppendColumn("Nombre", new CellRendererText(), "text", 1);
        _treeViewRepuestos.AppendColumn("Detalle", new CellRendererText(), "text", 2);
        _treeViewRepuestos.AppendColumn("Costo", new CellRendererText(), "text", 3);
    }
    
    /// <summary>
    /// Actualiza la lista de repuestos en el TreeView según el método de orden seleccionado.
    /// </summary>
    private void ActualizarLista()
    {
        _listStore.Clear();

        string metodo = _comboOrden.ActiveText;
        if (metodo == "Pre-orden")
        {
            Estructuras.Repuestos.PreOrder(AgregarRepuesto);
        }
        else if (metodo == "In-orden")
        {
            Estructuras.Repuestos.InOrder(AgregarRepuesto);
        }
        else if (metodo == "Post-orden")
        {
            Estructuras.Repuestos.PostOrder(AgregarRepuesto);
        }
    }

    /// <summary>
    /// Agrega un repuesto al ListStore del TreeView.
    /// </summary>
    /// <param name="repuesto">Objeto del tipo Repuesto a agregar.</param>
    private void AgregarRepuesto(object repuesto)
    {
        if (repuesto is Repuesto r)
        {
            _listStore.AppendValues(r.Id, r.Repuesto1, r.Detalles, r.Costo.ToString());
        }
    }
    
    /// <summary>
    /// Evento que se ejecuta al hacer clic en el botón de actualizar.
    /// </summary>
    /// <param name="sender">Objeto que dispara el evento.</param>
    /// <param name="e">Argumentos del evento.</param>
    private void OnActualizarClicked(object? sender, EventArgs e)
    {
        ActualizarLista();
    }
}