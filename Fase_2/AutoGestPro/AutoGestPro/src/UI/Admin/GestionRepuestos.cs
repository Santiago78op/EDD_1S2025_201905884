using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using Gtk;

namespace AutoGestPro.UI.Admin;

public class GestionRepuestos : Window
{
    private TreeView _treeViewRepuestos;
    private ListStore _listStore;
    private Entry _entryId, _entryNombre, _entryDetalle, _entryCosto;
    private Button _btnBuscar, _btnActualizar;

    public GestionRepuestos() : base("Gestión de Repuestos")
    {
        SetDefaultSize(600, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide(); // Oculta en vez de cerrar la ventana
        VBox vbox = new VBox(false, 5);

        // 🔹 Crear tabla de repuestos
        _treeViewRepuestos = new TreeView();
        CrearColumnasRepuestos();
        RefrescarRepuestos();
        ScrolledWindow scrolledWindow = new ScrolledWindow();
        scrolledWindow.Add(_treeViewRepuestos);
        vbox.PackStart(scrolledWindow, true, true, 5);

        // 🔹 Sección de búsqueda y edición
        HBox hboxEditar = new HBox(false, 5);
        _entryId = new Entry { PlaceholderText = "ID Repuesto" };
        _entryNombre = new Entry { PlaceholderText = "Nombre" };
        _entryDetalle = new Entry { PlaceholderText = "Detalle" };
        _entryCosto = new Entry { PlaceholderText = "Costo" };
        hboxEditar.PackStart(_entryId, false, false, 5);
        hboxEditar.PackStart(_entryNombre, false, false, 5);
        hboxEditar.PackStart(_entryDetalle, false, false, 5);
        hboxEditar.PackStart(_entryCosto, false, false, 5);
        vbox.PackStart(hboxEditar, false, false, 5);

        // 🔹 Botones
        HBox hboxBotones = new HBox(false, 5);
        _btnBuscar = new Button("🔍 Buscar Repuesto");
        _btnActualizar = new Button("✏️ Actualizar Repuesto");
        _btnBuscar.Clicked += OnBuscarRepuestoClicked;
        _btnActualizar.Clicked += OnActualizarRepuestoClicked;
        hboxBotones.PackStart(_btnBuscar, true, true, 5);
        hboxBotones.PackStart(_btnActualizar, true, true, 5);
        vbox.PackStart(hboxBotones, false, false, 5);
        Add(vbox);
        ShowAll();

        // Evento para auto rellenar los campos
        _treeViewRepuestos.RowActivated += OnTreeViewRepuestosRowActivated;
    }

    // ✅ Crea las columnas para la tabla de repuestos
    private void CrearColumnasRepuestos()
    {
        _listStore = new ListStore(typeof(int), typeof(string), typeof(string), typeof(double));
        _treeViewRepuestos.Model = _listStore;
        _treeViewRepuestos.AppendColumn("ID", new CellRendererText(), "text", 0);
        _treeViewRepuestos.AppendColumn("Nombre", new CellRendererText(), "text", 1);
        _treeViewRepuestos.AppendColumn("Detalle", new CellRendererText(), "text", 2);
        _treeViewRepuestos.AppendColumn("Costo", new CellRendererText(), "text", 3);
    }

    // ✅ Refresca la lista de repuestos en el TreeView
    private void RefrescarRepuestos()
    {
        _listStore.Clear();
        // 🔥 Recorrer el árbol AVL de repuestos, mostrar los datos en el TreeView
        Estructuras.Repuestos.InOrder((repuesto) =>
        {
            if (repuesto != null)
            {
                Repuesto r = (Repuesto)repuesto;
                _listStore.AppendValues(r.Id, r.Repuesto1, r.Detalles, r.Costo);
            }
        });
    }

    // ✅ Evento para auto rellenar los campos con los datos del repuesto seleccionado
    private void OnTreeViewRepuestosRowActivated(object sender, RowActivatedArgs args)
    {
        TreeIter iter;
        if (_treeViewRepuestos.Model.GetIter(out iter, args.Path))
        {
            _entryId.Text = _treeViewRepuestos.Model.GetValue(iter, 0).ToString();
            _entryNombre.Text = _treeViewRepuestos.Model.GetValue(iter, 1).ToString();
            _entryDetalle.Text = _treeViewRepuestos.Model.GetValue(iter, 2).ToString();
            _entryCosto.Text = _treeViewRepuestos.Model.GetValue(iter, 3).ToString();
        }
    }

    // ✅ Buscar repuesto por ID
    private void OnBuscarRepuestoClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(_entryId.Text, out int id))
        {
            MostrarMensaje("Error", "Ingrese un ID válido.");
            return;
        }

        var repuesto = Estructuras.Repuestos.Search(id);
        if (repuesto != null)
        {
            Repuesto r = (Repuesto)repuesto;
            _entryNombre.Text = r.Repuesto1;
            _entryDetalle.Text = r.Detalles;
            _entryCosto.Text = r.Costo.ToString();
        }
        else
        {
            MostrarMensaje("Error", "Repuesto no encontrado.");
        }
    }

    // ✅ Actualizar repuesto
    private void OnActualizarRepuestoClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(_entryId.Text, out int id))
        {
            MostrarMensaje("Error", "Ingrese un ID válido.");
            return;
        }

        if (!double.TryParse(_entryCosto.Text, out double costo))
        {
            MostrarMensaje("Error", "Ingrese un costo válido.");
            return;
        }

        var repuestoActualizado = new Repuesto(id, _entryNombre.Text, _entryDetalle.Text, costo);
        if (Estructuras.Repuestos.Modify(id, repuestoActualizado))
        {
            MostrarMensaje("Éxito", "Repuesto actualizado correctamente.");
            RefrescarRepuestos();
        }
        else
        {
            MostrarMensaje("Error", "No se encontró el repuesto para actualizar.");
        }
    }

    // ✅ Muestra un mensaje de error o éxito
    private void MostrarMensaje(string titulo, string mensaje)
    {
        MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
        dialog.Title = titulo;
        dialog.Run();
        dialog.Destroy();
    }
}