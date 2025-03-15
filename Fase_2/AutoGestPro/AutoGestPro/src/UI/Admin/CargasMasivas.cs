

using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Services;
using Gtk;

namespace AutoGestPro.UI.Admin;

public class CargasMasivas : Window
{
    private CargaMasivaService _cargaMasivaService;
    private ListStore _listStore;
    readonly TreeView _treeView;
    readonly ComboBoxText _comboEntidades;

    public CargasMasivas() : base("Cargas Masivas")
    {
        _cargaMasivaService = new CargaMasivaService();

        SetDefaultSize(500, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Hide(); };

        VBox vbox = new VBox { BorderWidth = 10 };

        Label lblSeleccion = new Label("Selecciona la entidad a cargar:");
        _comboEntidades = new ComboBoxText();
        _comboEntidades.AppendText("Usuarios");
        _comboEntidades.AppendText("Vehículos");
        _comboEntidades.AppendText("Repuestos");
        _comboEntidades.Active = 0; // Predeterminado en "Usuarios"
        _comboEntidades.Changed += OnComboEntidadesChanged;

        Button btnSeleccionarArchivo = new Button("Seleccionar Archivo CSV");
        btnSeleccionarArchivo.Clicked += OnSeleccionarArchivoClicked;

        _treeView = new TreeView();
        CrearColumnasTreeView("Usuarios");

        ScrolledWindow scrollWindow = new ScrolledWindow();
        scrollWindow.Add(_treeView);

        vbox.PackStart(lblSeleccion, false, false, 5);
        vbox.PackStart(_comboEntidades, false, false, 5);
        vbox.PackStart(btnSeleccionarArchivo, false, false, 5);
        vbox.PackStart(scrollWindow, true, true, 5);

        Add(vbox);
        ShowAll();
    }
    
    private void OnComboEntidadesChanged(object? sender, EventArgs e)
    {
        string entidadSeleccionada = _comboEntidades.ActiveText;
        CrearColumnasTreeView(entidadSeleccionada);
    }

    private void CrearColumnasTreeView(string entidad)
    {
        _listStore = entidad switch
        {
            "Usuarios" => new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(int), typeof(string)),
            "Vehículos" => new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(string)),
            "Repuestos" => new ListStore(typeof(int), typeof(string), typeof(string), typeof(string)),
            _ => throw new Exception("Entidad no reconocida")
        };

        _treeView.Model = _listStore;

        // 🔥 Eliminar columnas correctamente
        while (_treeView.Columns.Length > 0)
        {
            _treeView.RemoveColumn(_treeView.Columns[0]);
        }

        if (entidad == "Usuarios")
        {
            _treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            _treeView.AppendColumn("Nombre", new CellRendererText(), "text", 1);
            _treeView.AppendColumn("Apellido", new CellRendererText(), "text", 2);
            _treeView.AppendColumn("Correo", new CellRendererText(), "text", 3);
            _treeView.AppendColumn("Edad", new CellRendererText(), "text", 4);
            _treeView.AppendColumn("Contraseña", new CellRendererText(), "text", 5);
        }
        else if (entidad == "Vehículos")
        {
            _treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            _treeView.AppendColumn("Usuario", new CellRendererText(), "text", 1);
            _treeView.AppendColumn("Marca", new CellRendererText(), "text", 2);
            _treeView.AppendColumn("Modelo", new CellRendererText(), "text", 3);
            _treeView.AppendColumn("Placa", new CellRendererText(), "text", 4);
        }
        else if (entidad == "Repuestos")
        {
            _treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            _treeView.AppendColumn("Repuesto", new CellRendererText(), "text", 1);
            _treeView.AppendColumn("Detalle", new CellRendererText(), "text", 2);
            _treeView.AppendColumn("Costo", new CellRendererText(), "text", 3);
        }
    }


    private void OnSeleccionarArchivoClicked(object? sender, EventArgs e)
    {
        string entidadSeleccionada = _comboEntidades.ActiveText;
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
                _cargaMasivaService.CargarClientesJson(rutaArchivo);
                // funcion para mostrar datos en el treeview
                
                MostrarDatosEnTreeViewCliente();
            }
            else if (entidadSeleccionada == "Vehículos")
            {
                _cargaMasivaService.CargarVehiculosJson(rutaArchivo);
                MostrarDatosEnTreeViewVehiculo();
            }
            else if (entidadSeleccionada == "Repuestos")
            {
                _cargaMasivaService.CargarRepuestosJson(rutaArchivo);
                MostrarDatosEnTreeViewRepuesto();
            }
        }

        fileChooser.Destroy();
    }
    
    public void MostrarDatosEnTreeViewCliente()
    {
        try
        {
            _listStore.Clear();

            NodeLinked? current = Estructuras.Clientes.Head;

            while (current != null)
            {
                Cliente cliente = (Cliente)current.Data;
                _listStore.AppendValues(cliente.Id, cliente.Nombres, cliente.Apellidos, cliente.Correo,
                    cliente.Edad, cliente.Contrasenia);

                current = current.Next;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error al mostrar los datos en el TreeView");
        }
    }
    
    public void MostrarDatosEnTreeViewVehiculo()
    {
        try
        {
            _listStore.Clear();

            NodeDouble? current = Estructuras.Vehiculos.Head;

            while (current != null)
            {
                Vehiculo vehiculo = (Vehiculo)current.Data;
                _listStore.AppendValues(vehiculo.Id, vehiculo.Id_Usuario, vehiculo.Marca, vehiculo.Modelo,
                    vehiculo.Placa);

                current = current.Next;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error al mostrar los datos en el TreeView");
        }
    }
    
    public void MostrarDatosEnTreeViewRepuesto()
    {
        try
        {
            _listStore.Clear();
            
            // 🔥 Recorrer el árbol AVL de repuestos, mostrar los datos en el TreeView
            Estructuras.Repuestos.InOrder((repuesto) =>
            {
                if (repuesto != null)
                {
                    Repuesto r = (Repuesto)repuesto;
                    _listStore.AppendValues(r.Id, r.Repuesto1, r.Detalles, r.Costo.ToString());
                }
            });
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error al mostrar los datos en el TreeView");
        }
    }
    
}