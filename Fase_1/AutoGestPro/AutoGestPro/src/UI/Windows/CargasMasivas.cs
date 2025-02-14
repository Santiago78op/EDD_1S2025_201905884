using AutoGestPro.Core.Structures;

namespace AutoGestPro.UI.Windows;

using System;
using System.Collections.Generic;
using Gtk;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Services;

public unsafe class CargasMasivas : Window
{
    private CargaMasivaService cargaMasivaService;
    private ListStore listStore;
    private TreeView treeView;
    private ComboBoxText comboEntidades;

    public CargasMasivas() : base("Cargas Masivas")
    {
        cargaMasivaService = new CargaMasivaService();

        SetDefaultSize(500, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Hide(); };

        VBox vbox = new VBox { BorderWidth = 10 };

        Label lblSeleccion = new Label("Selecciona la entidad a cargar:");
        comboEntidades = new ComboBoxText();
        comboEntidades.AppendText("Usuarios");
        comboEntidades.AppendText("Vehículos");
        comboEntidades.AppendText("Repuestos");
        comboEntidades.Active = 0; // Predeterminado en "Usuarios"

        Button btnSeleccionarArchivo = new Button("Seleccionar Archivo CSV");
        btnSeleccionarArchivo.Clicked += OnSeleccionarArchivoClicked;

        treeView = new TreeView();
        CrearColumnasTreeView("Usuarios");

        ScrolledWindow scrollWindow = new ScrolledWindow();
        scrollWindow.Add(treeView);

        vbox.PackStart(lblSeleccion, false, false, 5);
        vbox.PackStart(comboEntidades, false, false, 5);
        vbox.PackStart(btnSeleccionarArchivo, false, false, 5);
        vbox.PackStart(scrollWindow, true, true, 5);

        Add(vbox);
        ShowAll();
    }

    private void CrearColumnasTreeView(string entidad)
    {
        listStore = entidad switch
        {
            "Usuarios" => new ListStore(typeof(int), typeof(string), typeof(string)),
            "Vehículos" => new ListStore(typeof(int), typeof(string), typeof(string)),
            "Repuestos" => new ListStore(typeof(int), typeof(string), typeof(decimal)),
            _ => throw new Exception("Entidad no reconocida")
        };

        treeView.Model = listStore;

        // 🔥 Eliminar columnas correctamente
        while (treeView.Columns.Length > 0)
        {
            treeView.RemoveColumn(treeView.Columns[0]);
        }

        if (entidad == "Usuarios")
        {
            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("Nombre", new CellRendererText(), "text", 1);
            treeView.AppendColumn("Rol", new CellRendererText(), "text", 2);
        }
        else if (entidad == "Vehículos")
        {
            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("Marca", new CellRendererText(), "text", 1);
            treeView.AppendColumn("Modelo", new CellRendererText(), "text", 2);
        }
        else if (entidad == "Repuestos")
        {
            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("Nombre", new CellRendererText(), "text", 1);
            treeView.AppendColumn("Precio", new CellRendererText(), "text", 2);
        }
    }


    private void OnSeleccionarArchivoClicked(object sender, EventArgs e)
    {
        string entidadSeleccionada = comboEntidades.ActiveText;
        FileChooserDialog fileChooser = new FileChooserDialog(
            "Seleccionar Archivo CSV",
            this,
            FileChooserAction.Open,
            "Cancelar", ResponseType.Cancel,
            "Abrir", ResponseType.Accept
        );

        if (fileChooser.Run() == (int)ResponseType.Accept)
        {
            string rutaArchivo = fileChooser.Filename;

            if (entidadSeleccionada == "Usuarios")
            {
                Linked_List<Cliente> clientes = cargaMasivaService.CargarUsuariosDesdeCSV(rutaArchivo);
                MostrarDatosEnTreeView(clientes);
            }
            else if (entidadSeleccionada == "Vehículos")
            {
                Linked_List<Cliente> clientes = cargaMasivaService.CargarUsuariosDesdeCSV(rutaArchivo);
                MostrarDatosEnTreeView(clientes);
            }
            else if (entidadSeleccionada == "Repuestos")
            {
                Linked_List<Cliente> clientes = cargaMasivaService.CargarUsuariosDesdeCSV(rutaArchivo);
                MostrarDatosEnTreeView(clientes);
            }
        }

        fileChooser.Destroy();
    }

    private void MostrarDatosEnTreeView<T>(Linked_List<T> lista) where T : class
    {
        listStore.Clear();

        // 🔥 Modificamos el bucle para iterar sobre Linked_List<T>
        var nodoActual = lista.Head;  // Acceder al primer nodo
        while (nodoActual != null)
        {
            T item = nodoActual->_data;

            if (item is Cliente cliente)
                listStore.AppendValues(cliente.Id, cliente.Nombre, cliente.Apellido);
            else if (item is Vehiculo vehiculo)
                listStore.AppendValues(vehiculo.Id, vehiculo.Marca, vehiculo.Modelo);
            else if (item is Repuesto repuesto)
                listStore.AppendValues(repuesto.Id, repuesto.Repuesto1, repuesto.Costo);

            nodoActual = nodoActual->_next;  // Avanzar al siguiente nodo
        }
    }
}