using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using Gtk;

namespace AutoGestPro.UI.Admin;

public class GenerarServicio : Window
{
    private Entry _entryId, _entryIdVehiculo, _entryIdRepuesto, _entryDetalle, _entryCosto;
    private Button btnGenerarServicio;
    private TreeView treeViewServicios;
    private ListStore listStore;
    
    private int _contadorFactura = 0;

    public GenerarServicio() : base("Generar Servicio")
    {
        SetDefaultSize(600, 500);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide(); // Oculta en vez de cerrar la ventana
        VBox vbox = new VBox(false, 5);

        // 🔹 Campos de entrada
        
        // * Id Servicio
        HBox hboxId = new HBox(false, 5);
        Label lblId = new Label("ID:");
        _entryId = new Entry();
        hboxId.PackStart(lblId, false, false, 5);
        hboxId.PackStart(_entryId, false, false, 5);
        vbox.PackStart(hboxId, false, false, 5);
        
        // * Id Vehículo
        HBox hboxVehiculo = new HBox(false, 5);
        Label lblVehiculo = new Label("ID Vehículo:");
        _entryIdVehiculo = new Entry();
        hboxVehiculo.PackStart(lblVehiculo, false, false, 5);
        hboxVehiculo.PackStart(_entryIdVehiculo, false, false, 5);
        vbox.PackStart(hboxVehiculo, false, false, 5);
        HBox hboxRepuesto = new HBox(false, 5);
        
        // * Id Repuesto
        Label lblRepuesto = new Label("ID Repuesto:");
        _entryIdRepuesto = new Entry();
        hboxRepuesto.PackStart(lblRepuesto, false, false, 5);
        hboxRepuesto.PackStart(_entryIdRepuesto, false, false, 5);
        vbox.PackStart(hboxRepuesto, false, false, 5);
        HBox hboxDetalle = new HBox(false, 5);
        
        // * Detalle
        Label lblDetalle = new Label("Detalle:");
        _entryDetalle = new Entry();
        hboxDetalle.PackStart(lblDetalle, false, false, 5);
        hboxDetalle.PackStart(_entryDetalle, false, false, 5);
        vbox.PackStart(hboxDetalle, false, false, 5);
        HBox hboxCosto = new HBox(false, 5);
        
        // * Costo
        Label lblCosto = new Label("Costo:");
        _entryCosto = new Entry();
        hboxCosto.PackStart(lblCosto, false, false, 5);
        hboxCosto.PackStart(_entryCosto, false, false, 5);
        vbox.PackStart(hboxCosto, false, false, 5);

        // 🔹 Botón para generar servicio
        btnGenerarServicio = new Button("💾 Generar Servicio");
        btnGenerarServicio.Clicked += OnGenerarServicioClicked;
        vbox.PackStart(btnGenerarServicio, false, false, 5);

        // 🔹 Tabla de servicios generados
        treeViewServicios = new TreeView();
        CrearColumnasServicios();
        RefrescarServicios();
        ScrolledWindow scrolledWindow = new ScrolledWindow();
        scrolledWindow.Add(treeViewServicios);
        vbox.PackStart(scrolledWindow, true, true, 5);
        Add(vbox);
        ShowAll();
    }

    // ✅ Crea las columnas para la tabla de servicios
    private void CrearColumnasServicios()
    {
        listStore = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
        treeViewServicios.Model = listStore;
        treeViewServicios.AppendColumn("ID", new CellRendererText(), "text", 0);
        treeViewServicios.AppendColumn("ID Vehículo", new CellRendererText(), "text", 1);
        treeViewServicios.AppendColumn("ID Repuesto", new CellRendererText(), "text", 2);
        treeViewServicios.AppendColumn("Detalle", new CellRendererText(), "text", 3);
        treeViewServicios.AppendColumn("Costo", new CellRendererText(), "text", 4);
    }

    // ✅ Método para actualizar la visualización de los servicios
    private void RefrescarServicios()
    {
        listStore.Clear();
        // 🔥 Recorrer el árbol AVL de repuestos, mostrar los datos en el TreeView// 🔥 Recorrer el árbol AVL de repuestos, mostrar los datos en el TreeView
        var serviciosList = Estructuras.Servicios.InOrder();
        foreach (var servicio in serviciosList)
        {
            if (servicio != null)
            {
                Servicio s = (Servicio)servicio;
                listStore.AppendValues(s.Id, s.IdRepuesto, s.IdRepuesto, s.Detalles, s.Costo.ToString());
            }
        }
    }

    // ✅ Evento para generar servicio
    private void OnGenerarServicioClicked(object sender, EventArgs e)
    {
        int id, idVehiculo, idRepuesto;
        decimal costo;
        if (!int.TryParse(_entryId.Text, out id) ||
            !int.TryParse(_entryIdVehiculo.Text, out idVehiculo) ||
            !int.TryParse(_entryIdRepuesto.Text, out idRepuesto) ||
            !decimal.TryParse(_entryCosto.Text, out costo))
        {
            MostrarMensaje("Error", "Ingrese valores válidos.");
            return;
        }

        string detalle = _entryDetalle.Text;
        
        // Verificar si el servicio ya existe
        var servicioExistente = Estructuras.Servicios.Search(id);
        if (servicioExistente != null)
        {
            MostrarMensaje("Error", "El servicio ya existe.");
            return;
        }
        
        // Verificar si el vehículo y repuesto existen
        NodeDouble? nodoVehiculo =  Estructuras.Vehiculos.SearchNode(idVehiculo);
        if (nodoVehiculo == null)
        {
            MostrarMensaje("Error", "El vehículo no existe.");
            return;
        }
        
        var nodoRepuesto = Estructuras.Repuestos.Search(idRepuesto);
        if (nodoRepuesto == null)
        {
            MostrarMensaje("Error", "El repuesto no existe.");
            return;
        }
        
        // Id del Propietario del vehículo
        Vehiculo v = (Vehiculo)nodoVehiculo.Data;
        int idUsuario = v.Id_Usuario;
        
        // 🔥 Crear nuevo servicio y se inserta en el árbol AVL global
        Servicio nuevoServicio = new Servicio(id , idUsuario ,idVehiculo, idRepuesto, detalle, costo);
        Estructuras.Servicios.Insert(id,nuevoServicio);
        
        // 🔥 El Servicio se agrega a la lista de servicios del propietario
        
        // ✅ Agregar el servicio al usuario correspondiente
        Cliente c = (Cliente)Estructuras.Clientes.SearchNode(idUsuario)?.Data as Cliente;
        if (c != null)
        {
            c.Servicios.Insert(id,nuevoServicio);
            Console.WriteLine($"✅ Servicio agregado al usuario {c.Nombres}");
            
            // 🔥 Cracion de la factura
            Repuesto repuesto = (Repuesto)nodoRepuesto;
            // Calculo del costo total
            decimal costoTotal = costo + repuesto.Costo;
            // Incrementar contador de factura
            _contadorFactura++;
            // Objeto factura
            Factura nuevaFactura = new Factura(_contadorFactura, idUsuario, id, costoTotal);
            
            // Insertar factura en la lista de facturas
            c.Facturas.Insert(_contadorFactura, nuevaFactura);
            
            Estructuras.Facturas.Insert(_contadorFactura, nuevaFactura);
            MostrarMensaje("Éxito", "Factura generada correctamente.");
        }
        else
        {
            Console.WriteLine($"❌ Error: Usuario no encontrado.");
            return;
        }

        
        MostrarMensaje("Éxito", "Servicio generado correctamente.");
        RefrescarServicios();
    }

    // ✅ Muestra un mensaje
    private void MostrarMensaje(string titulo, string mensaje)
    {
        MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
        dialog.Title = titulo;
        dialog.Run();
        dialog.Destroy();
    }
} 