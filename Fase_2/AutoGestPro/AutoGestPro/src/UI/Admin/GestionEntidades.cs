using System.Text;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Interfaces;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;
using Gtk;

namespace AutoGestPro.UI.Admin;

public class GestionEntidades : Window
{
    private TreeView _treeView;
    private ListStore _listStore;
    private ComboBoxText _comboEntidades;
    private Button _btnEliminar, _btnBuscar, _btnModificar;
    private Entry _entryBusqueda;
    private Entry _entryNombre, _entryApellido, _entryCorreo, _entryEdad, _entryClave;
    private Entry _entryMarca, _entryModelo, _entryPlaca;
    private HBox _hboxDatosUsuario, _hboxDatosVehiculo;


    public GestionEntidades() : base("Gestión de Entidades")
    {
        SetDefaultSize(650, 500);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Hide(); };

        VBox vbox = new VBox { BorderWidth = 10 };

        // 🔹 ComboBox para seleccionar la entidad a gestionar
        Label lblSeleccion = new Label("Selecciona la entidad:");
        _comboEntidades = new ComboBoxText();
        _comboEntidades.AppendText("Usuarios");
        _comboEntidades.AppendText("Vehículos");
        _comboEntidades.Active = 0;
        _comboEntidades.Changed += OnComboEntidadesChanged;

        // 🔹 Entrada y Botón de Búsqueda
        HBox hboxBusqueda = new HBox(false, 5);
        _entryBusqueda = new Entry { PlaceholderText = "ID a buscar" };
        _btnBuscar = new Button("Buscar");
        _btnBuscar.Clicked += OnBuscarClicked;
        hboxBusqueda.PackStart(_entryBusqueda, true, true, 5);
        hboxBusqueda.PackStart(_btnBuscar, false, false, 5);

        // 🔹 Entrada de Datos para Modificación (Usuarios)
        _hboxDatosUsuario = new HBox(false, 5);
        _entryNombre = new Entry { PlaceholderText = "Nombre" };
        _entryApellido = new Entry { PlaceholderText = "Apellido" };
        _entryCorreo = new Entry { PlaceholderText = "Correo" };
        _entryEdad = new Entry { PlaceholderText = "Edad" };
        _entryClave = new Entry { PlaceholderText = "Contraseña", Visibility = false };

        _hboxDatosUsuario.PackStart(_entryNombre, true, true, 5);
        _hboxDatosUsuario.PackStart(_entryApellido, true, true, 5);
        _hboxDatosUsuario.PackStart(_entryCorreo, true, true, 5);
        _hboxDatosUsuario.PackStart(_entryEdad, true, true, 5);
        _hboxDatosUsuario.PackStart(_entryClave, true, true, 5);

        // 🔹 Entrada de Datos para Modificación (Vehículos)
        _hboxDatosVehiculo = new HBox(false, 5);
        _entryMarca = new Entry { PlaceholderText = "Marca" };
        _entryModelo = new Entry { PlaceholderText = "Modelo" };
        _entryPlaca = new Entry { PlaceholderText = "Placa" };

        _hboxDatosVehiculo.PackStart(_entryMarca, true, true, 5);
        _hboxDatosVehiculo.PackStart(_entryModelo, true, true, 5);
        _hboxDatosVehiculo.PackStart(_entryPlaca, true, true, 5);

        // 🔹 Botones de Acción
        HBox hboxBotones = new HBox(false, 5);
        _btnModificar = new Button("Modificar");
        _btnEliminar = new Button("Eliminar");

        _btnModificar.Clicked += OnModificarClicked;
        _btnEliminar.Clicked += OnEliminarClicked;

        hboxBotones.PackStart(_btnModificar, false, false, 5);
        hboxBotones.PackStart(_btnEliminar, false, false, 5);

        // 🔹 Tabla para mostrar datos
        _treeView = new TreeView();
        CrearColumnasTreeView("Usuarios");

        ScrolledWindow scrollWindow = new ScrolledWindow();
        scrollWindow.Add(_treeView);

        vbox.PackStart(lblSeleccion, false, false, 5);
        vbox.PackStart(_comboEntidades, false, false, 5);
        vbox.PackStart(hboxBusqueda, false, false, 5);
        vbox.PackStart(_hboxDatosUsuario, false, false, 5);
        vbox.PackStart(_hboxDatosVehiculo, false, false, 5);
        vbox.PackStart(scrollWindow, true, true, 5);
        vbox.PackStart(hboxBotones, false, false, 5);

        Add(vbox);
        ShowAll();

        // 📌 Se actualizan los campos visibles
        ActualizarCamposVisibles();
    }

    // 📌 Se actualiza la vista según la entidad seleccionada
    private void OnComboEntidadesChanged(object? sender, EventArgs e)
    {
        string entidadSeleccionada = _comboEntidades.ActiveText;
        CrearColumnasTreeView(entidadSeleccionada);
        ActualizarCamposVisibles();
    }
    
    // 📌 Actualiza los campos visibles según la entidad seleccionada
    private void ActualizarCamposVisibles()
    {
        string entidadSeleccionada = _comboEntidades.ActiveText;

        _hboxDatosUsuario.Visible = entidadSeleccionada == "Usuarios";
        _hboxDatosVehiculo.Visible = entidadSeleccionada == "Vehículos";
    }

    // 📌 Crea las columnas y asigna el modelo adecuado al TreeView
    private void CrearColumnasTreeView(string entidad)
    {
        if (_treeView.Model != null)
        {
            _treeView.Model = null; // Limpiar el modelo antes de actualizar
        }

        _listStore = entidad switch
        {
            "Usuarios" => new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(int)),
            "Vehículos" => new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(string)),
            _ => throw new Exception("Entidad no reconocida")
        };

        _treeView.Model = _listStore;

        // 🔥 Eliminar columnas antes de añadir nuevas
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

            MostrarUsuarios();
        }
        else if (entidad == "Vehículos")
        {
            _treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            _treeView.AppendColumn("Usuario", new CellRendererText(), "text", 1);
            _treeView.AppendColumn("Marca", new CellRendererText(), "text", 2);
            _treeView.AppendColumn("Modelo", new CellRendererText(), "text", 3);
            _treeView.AppendColumn("Placa", new CellRendererText(), "text", 4);

            MostrarVehiculos();
        }
    }

    // 📌 Muestra los Usuarios en el TreeView
    private void MostrarUsuarios()
    {
        _listStore.Clear();
        
        NodeLinked? clientes = Estructuras.Clientes.Head;

        while (clientes != null)
        {
            if (clientes.Data is Cliente cliente)
            {
                _listStore.AppendValues(cliente.Id, cliente.Nombres, cliente.Apellidos, cliente.Correo, cliente.Edad);
            }
            clientes = clientes.Next;
        }
    }

    // 📌 Muestra los Vehículos en el TreeView
    private void MostrarVehiculos()
    {
        _listStore.Clear();
        
        NodeDouble? vehiculos = Estructuras.Vehiculos.Head;
        
        while (vehiculos != null)
        {
            if (vehiculos.Data is Vehiculo vehiculo)
            {
                _listStore.AppendValues(vehiculo.Id, vehiculo.Id_Usuario, vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
            }
            vehiculos = vehiculos.Next;
        }
    }
    
    // 📌 Modificar Usuario o Vehículo
    private void OnModificarClicked(object sender, EventArgs e)
    {
        TreeIter iter;
        ITreeModel model;
        if (_treeView.Selection.GetSelected(out model, out iter))
        {
            int id = (int)model.GetValue(iter, 0);
            string entidadSeleccionada = _comboEntidades.ActiveText;

            if (entidadSeleccionada == "Usuarios")
            {
                Cliente cliente = new Cliente(id, _entryNombre.Text, _entryApellido.Text, _entryCorreo.Text, int.Parse(_entryEdad.Text), _entryClave.Text);
                Estructuras.Clientes.ModifyNode(id, cliente);
                MostrarUsuarios();
                MostrarMensaje("Éxito", "Usuario modificado correctamente.");
            }
            else if (entidadSeleccionada == "Vehículos")
            {
                Vehiculo vehiculo = new Vehiculo(id, int.Parse(_entryBusqueda.Text), _entryMarca.Text, int.Parse(_entryModelo.Text), _entryPlaca.Text);
                Estructuras.Vehiculos.ModifyNode(id, vehiculo);
                MostrarVehiculos();
                MostrarMensaje("Éxito", "Vehículo modificado correctamente.");
            }
        }
        else
        {
            MostrarMensaje("Error", "Seleccione un registro para modificar.");
        }
    }

    // 📌 Buscar Usuario o Vehículo
    private void OnBuscarClicked(object sender, EventArgs e)
    {
        int id;
        if (!int.TryParse(_entryBusqueda.Text, out id))
        {
            MostrarMensaje("Error", "Ingrese un ID válido.");
            return;
        }

        string entidadSeleccionada = _comboEntidades.ActiveText;

        if (entidadSeleccionada == "Usuarios")
        {
            NodeLinked nodeCliente = Estructuras.Clientes.SearchNode(id);
            if (nodeCliente != null)
            {
                Cliente cliente = (Cliente)nodeCliente.Data;
                MostrarMensaje("Usuario Encontrado",
                    $"ID: {cliente.Id}\nNombre: {cliente.Nombres}\nApellido: {cliente.Apellidos}\nCorreo: {cliente.Correo}\nEdad: {cliente.Edad}");
                _entryNombre.Text = cliente.Nombres;
                _entryApellido.Text = cliente.Apellidos;
                _entryCorreo.Text = cliente.Correo;
                _entryEdad.Text = cliente.Edad.ToString();
                _entryClave.Text = cliente.Contrasenia;
            }
            else
            {
                MostrarMensaje("Error", "Usuario no encontrado.");
            }
        }
        else if (entidadSeleccionada == "Vehículos")
        {
            NodeDouble nodeVehiculo = Estructuras.Vehiculos.SearchNode(id);
            if (nodeVehiculo != null)
            {
                Vehiculo vehiculo = (Vehiculo)nodeVehiculo.Data;
                MostrarMensaje("Vehículo Encontrado",
                    $"ID: {vehiculo.Id}\nUsuario: {vehiculo.Id_Usuario}\nMarca: {vehiculo.Marca}\nModelo: {vehiculo.Modelo}\nPlaca: {vehiculo.Placa}");
                
                // 📌 Se actualizan los campos de texto
                _entryMarca.Text = vehiculo.Marca;
                _entryModelo.Text = vehiculo.Modelo.ToString();
                _entryPlaca.Text = vehiculo.Placa;
            }
            else
            {
                MostrarMensaje("Error", "Vehículo no encontrado.");
            }
        }
    }

    // 📌 Eliminar Usuario o Vehículo
    private void OnEliminarClicked(object sender, EventArgs e)
    {
        TreeIter iter;
        ITreeModel model;
        if (_treeView.Selection.GetSelected(out model, out iter))
        {
            int id = (int)model.GetValue(iter, 0);
            string entidadSeleccionada = _comboEntidades.ActiveText;

            if (entidadSeleccionada == "Usuarios")
            {
                // Vehículos asociados al usuario eliminado, también se eliminan
                StringBuilder vehiculosEliminados = new StringBuilder();
                
                if(Estructuras.Vehiculos.Length > 0)
                {
                    NodeDouble? vehiculos = Estructuras.Vehiculos.Head;
                    
                    while (vehiculos != null)
                    {
                        if (vehiculos.Data is Vehiculo vehiculo)
                        {
                            if (vehiculo.Id_Usuario == id)
                            {
                                vehiculosEliminados.Append($"ID: {vehiculo.Id}, Marca: {vehiculo.Marca}, Modelo: {vehiculo.Modelo}, Placa: {vehiculo.Placa}\n");
                                Estructuras.Vehiculos.DeleteNode(vehiculo.Id);
                            }
                        }
                        vehiculos = vehiculos.Next;
                    }
                }
                
                if (Estructuras.Clientes.DeleteNode(id))
                {
                    MostrarUsuarios();
                    MostrarVehiculos();
                    MostrarMensaje("Éxito", $"Usuario eliminado.\nVehículos eliminados:\n{vehiculosEliminados}");
                }
                else
                {
                    MostrarMensaje("Error", "No se pudo eliminar el usuario.");
                }
            }
            else if (entidadSeleccionada == "Vehículos")
            {
                if (Estructuras.Vehiculos.DeleteNode(id))
                {
                    MostrarVehiculos();
                    MostrarMensaje("Éxito", "Vehículo eliminado correctamente.");
                }
                else
                {
                    MostrarMensaje("Error", "No se pudo eliminar el vehículo.");
                }
            }
        }
        else
        {
            MostrarMensaje("Error", "Seleccione un registro para eliminar.");
        }
    }

    // 📌 Muestra un mensaje en pantalla
    private void MostrarMensaje(string titulo, string mensaje)
    {
        MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
        dialog.Title = titulo;
        dialog.Run();
        dialog.Destroy();
    }
}