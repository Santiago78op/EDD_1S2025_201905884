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
        comboEntidades.Changed += OnComboEntidadesChanged;

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
    
    private void OnComboEntidadesChanged(object sender, EventArgs e)
    {
        string entidadSeleccionada = comboEntidades.ActiveText;
        CrearColumnasTreeView(entidadSeleccionada);
    }

    private void CrearColumnasTreeView(string entidad)
    {
        listStore = entidad switch
        {
            "Usuarios" => new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(string)),
            "Vehículos" => new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(string)),
            "Repuestos" => new ListStore(typeof(int), typeof(string), typeof(string), typeof(double)),
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
            treeView.AppendColumn("Apellido", new CellRendererText(), "text", 2);
            treeView.AppendColumn("Correo", new CellRendererText(), "text", 3);
            treeView.AppendColumn("Contraseña", new CellRendererText(), "text", 4);
        }
        else if (entidad == "Vehículos")
        {
            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("Usuario", new CellRendererText(), "text", 1);
            treeView.AppendColumn("Marca", new CellRendererText(), "text", 2);
            treeView.AppendColumn("Modelo", new CellRendererText(), "text", 3);
            treeView.AppendColumn("Placa", new CellRendererText(), "text", 4);
        }
        else if (entidad == "Repuestos")
        {
            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("Repuesto", new CellRendererText(), "text", 1);
            treeView.AppendColumn("Detalle", new CellRendererText(), "text", 2);
            treeView.AppendColumn("Costo", new CellRendererText(), "text", 3);
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
                var clientes = cargaMasivaService.CargarCleintesDesdeCSV(rutaArchivo);
                // funcion para mostrar datos en el treeview
                
                MostrarDatosEnTreeViewCliente(clientes);
            }
            else if (entidadSeleccionada == "Vehículos")
            {
                var vehiculos = cargaMasivaService.CargarVehiculosDesdeCSV(rutaArchivo);
                MostrarDatosEnTreeViewVehiculo(vehiculos);
            }
            else if (entidadSeleccionada == "Repuestos")
            {
                var repuestos = cargaMasivaService.CargarRepuestosDesdeCSV(rutaArchivo);
                MostrarDatosEnTreeViewRepuesto(repuestos);
            }
        }

        fileChooser.Destroy();
    }
    
    public void MostrarDatosEnTreeViewCliente(Linked_List<Cliente> clientes)
    {
        listStore.Clear();

        for (int i = 0; i < clientes.Length; i++)
        {
            if (clientes.GetNode(i)->_data is Cliente cliente)
            {
                listStore.AppendValues(cliente.Id, cliente.Nombre, cliente.Apellido, cliente.Correo, cliente.Contrasenia);
            }
            else
            {
                throw new Exception("Error al mostrar datos en el TreeView");
            }
        }
    }
    
    public void MostrarDatosEnTreeViewVehiculo(Double_List<Vehiculo> vehiculos)
    {
        listStore.Clear();

        for (int i = 0; i < vehiculos.Length; i++)
        {
            if (vehiculos.GetNode(i)->_data is Vehiculo vehiculo)
            {
                listStore.AppendValues(vehiculo.Id, vehiculo.Id_Usuario, vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
            }
            else
            {
                throw new Exception("Error al mostrar datos en el TreeView");
            }
        }
    }
    
    public void MostrarDatosEnTreeViewRepuesto(RingList<Repuesto> repuestos)
    {
        listStore.Clear();

        for (int i = 0; i < repuestos.Length; i++)
        {
            if (repuestos.GetNode(i)->_data is Repuesto repuesto)
            {
                listStore.AppendValues(repuesto.Id, repuesto.Repuesto1, repuesto.Detalle ,repuesto.Costo);
            }
            else
            {
                throw new Exception("Error al mostrar datos en el TreeView");
            }
        }
    }
}