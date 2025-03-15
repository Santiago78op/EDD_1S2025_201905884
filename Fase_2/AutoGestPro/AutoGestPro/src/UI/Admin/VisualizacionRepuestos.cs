using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Structures;
using Gtk;

namespace AutoGestPro.UI.Admin;

public class VisualizacionRepuestos : Window
{
    private TreeView _treeViewRepuestos;
    private ListStore _listStore;
    private ComboBoxText _comboOrden;
    private Button _btnActualizar;

    public VisualizacionRepuestos() : base("Visualización de Repuestos")
    {
        SetDefaultSize(600, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide(); // Oculta en vez de cerrar la ventana
        VBox vbox = new VBox(false, 5);
        
        // 🔹 ComboBox para seleccionar el orden de visualización
        HBox hboxOrden = new HBox(false, 5);
        Label lblOrden = new Label("Selecciona el orden:");
        _comboOrden = new ComboBoxText();
        _comboOrden.AppendText("Pre-orden");
        _comboOrden.AppendText("In-orden");
        _comboOrden.AppendText("Post-orden");
        _comboOrden.Active = 1; // In-orden por defecto
        hboxOrden.PackStart(lblOrden, false, false, 5);
        hboxOrden.PackStart(_comboOrden, false, false, 5);
        vbox.PackStart(hboxOrden, false, false, 5);
        
        // 🔹 Botón para actualizar la visualización
        _btnActualizar = new Button("🔄 Actualizar Vista");
        _btnActualizar.Clicked += OnActualizarClicked;
        vbox.PackStart(_btnActualizar, false, false, 5);
        
        // 🔹 TreeView para mostrar los repuestos
        _treeViewRepuestos = new TreeView();
        CrearColumnasRepuestos();
        RefrescarRepuestos();
        ScrolledWindow scrolledWindow = new ScrolledWindow();
        scrolledWindow.Add(_treeViewRepuestos);
        vbox.PackStart(scrolledWindow, true, true, 5);
        Add(vbox);
        ShowAll();
    }

    // ✅ Crea las columnas del TreeView
    private void CrearColumnasRepuestos()
    {
        _listStore = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
        _treeViewRepuestos.Model = _listStore;
        _treeViewRepuestos.AppendColumn("ID", new CellRendererText(), "text", 0);
        _treeViewRepuestos.AppendColumn("Nombre", new CellRendererText(), "text", 1);
        _treeViewRepuestos.AppendColumn("Detalle", new CellRendererText(), "text", 2);
        _treeViewRepuestos.AppendColumn("Costo", new CellRendererText(), "text", 3);
    }

    // ✅ Método para actualizar la visualización de los repuestos según el orden seleccionado
    private void RefrescarRepuestos()
    {
        _listStore.Clear();
        string ordenSeleccionado = _comboOrden.ActiveText;
        switch (ordenSeleccionado)
        {
            case "Pre-orden":
                // 🔥 Recorrer el árbol AVL de repuestos, mostrar los datos en el TreeView
                Estructuras.Repuestos.PreOrder((repuesto) =>
                {
                    if (repuesto != null)
                    {
                        Repuesto r = (Repuesto)repuesto;
                        _listStore.AppendValues(r.Id, r.Repuesto1, r.Detalles, r.Costo.ToString());
                    }
                });
                break;
            case "In-orden":
                // 🔥 Recorrer el árbol AVL de repuestos, mostrar los datos en el TreeView
                Estructuras.Repuestos.InOrder((repuesto) =>
                {
                    if (repuesto != null)
                    {
                        Repuesto r = (Repuesto)repuesto;
                        _listStore.AppendValues(r.Id, r.Repuesto1, r.Detalles, r.Costo.ToString());
                    }
                });
                break;
            case "Post-orden":
                // 🔥 Recorrer el árbol AVL de repuestos, mostrar los datos en el TreeView
                Estructuras.Repuestos.PostOrder((repuesto) =>
                {
                    if (repuesto != null)
                    {
                        Repuesto r = (Repuesto)repuesto;
                        _listStore.AppendValues(r.Id, r.Repuesto1, r.Detalles, r.Costo.ToString());
                    }
                });
                break;
        }
    }

    // ✅ Evento para actualizar la visualización al hacer clic en el botón
    private void OnActualizarClicked(object sender, EventArgs e)
    {
        RefrescarRepuestos();
    }
}