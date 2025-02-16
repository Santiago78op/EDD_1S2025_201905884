using System;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Services;
using Gtk;

namespace AutoGestPro.UI.Windows;

public class IngresoIndividual : Window
{
    private Notebook notebook;
    
    public IngresoIndividual() : base("Ingreso Individual")
    {
        SetDefaultSize(500, 400);
        SetPosition(WindowPosition.Center);

        notebook = new Notebook();

        notebook.AppendPage(CrearFormularioClientes(), new Label("Usuarios"));
        notebook.AppendPage(CrearFormularioVehiculos(), new Label("Vehículos"));
        notebook.AppendPage(CrearFormularioRepuestos(), new Label("Repuestos"));
        notebook.AppendPage(CrearFormularioServicios(), new Label("Servicios"));
        

        Add(notebook);
        ShowAll();
    }
    
    private VBox CrearFormularioClientes()
    {
        VBox vbox = new VBox();

        Entry txtId = new Entry { PlaceholderText = "ID" };
        Entry txtNombre = new Entry { PlaceholderText = "Nombre" };
        Entry txtApellido = new Entry { PlaceholderText = "Apellido" };
        Entry txtCorreo = new Entry { PlaceholderText = "Correo Electrónico" };
        Entry txtContrasenia = new Entry { PlaceholderText = "Contraseña" };
        Button btnGuardar = new Button("Guardar Usuario");
        
        btnGuardar.Clicked += (sender, e) =>
        {
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtCorreo.Text))
            {
                MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Todos los campos son obligatorios.");
                errorDialog.Run();
                errorDialog.Destroy();
                return;
            }

            Cliente cliente = new Cliente
            (
                int.Parse(txtId.Text),
                txtNombre.Text,
                txtApellido.Text,
                txtCorreo.Text,
                txtContrasenia.Text
            );

            CargaMasivaService.clientes.append(cliente);
            Console.WriteLine($"✅ Usuario {cliente.Nombre} agregado correctamente.");
        };
        
        vbox.PackStart(txtId, false, false, 5);
        vbox.PackStart(txtNombre, false, false, 5);
        vbox.PackStart(txtApellido, false, false, 5);
        vbox.PackStart(txtCorreo, false, false, 5);
        vbox.PackStart(txtContrasenia, false, false, 5);
        vbox.PackStart(btnGuardar, false, false, 5);

        return vbox;
    }

    public VBox CrearFormularioVehiculos()
    {
        VBox vbox = new VBox();

        Entry txtId = new Entry { PlaceholderText = "ID" };
        Entry txtUsuario = new Entry { PlaceholderText = "ID Usuario" };
        Entry txtMarca = new Entry { PlaceholderText = "Marca" };
        Entry txtModelo = new Entry { PlaceholderText = "Modelo" };
        Entry txtPlaca = new Entry { PlaceholderText = "Placa" };
        Button btnGuardar = new Button("Guardar Vehículo");
        
        btnGuardar.Clicked += (sender, e) =>
        {
            if (string.IsNullOrEmpty(txtMarca.Text) || string.IsNullOrEmpty(txtPlaca.Text))
            {
                MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Todos los campos son obligatorios.");
                errorDialog.Run();
                errorDialog.Destroy();
                return;
            }

            Vehiculo vehiculo = new Vehiculo
            (
                int.Parse(txtId.Text),
                int.Parse(txtUsuario.Text),
                txtMarca.Text,
                txtModelo.Text,
                txtPlaca.Text
            );

            CargaMasivaService.vehiculos.append(vehiculo);
            Console.WriteLine($"✅ Vehículo {vehiculo.Marca} agregado correctamente.");
        };
        
        vbox.PackStart(txtId, false, false, 5);
        vbox.PackStart(txtUsuario, false, false, 5);
        vbox.PackStart(txtMarca, false, false, 5);
        vbox.PackStart(txtModelo, false, false, 5);
        vbox.PackStart(txtPlaca, false, false, 5);
        vbox.PackStart(btnGuardar, false, false, 5);
        
        return vbox;
    }
    
    public VBox CrearFormularioRepuestos()
    {
        VBox vbox = new VBox();

        Entry txtId = new Entry { PlaceholderText = "ID" };
        Entry txtNombre = new Entry { PlaceholderText = "Nombre" };
        Entry txtDescripcion = new Entry { PlaceholderText = "Descripción" };
        Entry txtPrecio = new Entry { PlaceholderText = "Precio" };
        Button btnGuardar = new Button("Guardar Repuesto");
        
        btnGuardar.Clicked += (sender, e) =>
        {
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtDescripcion.Text))
            {
                MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Todos los campos son obligatorios.");
                errorDialog.Run();
                errorDialog.Destroy();
                return;
            }

            Repuesto repuesto = new Repuesto
            (
                int.Parse(txtId.Text),
                txtNombre.Text,
                txtDescripcion.Text,
                double.Parse(txtPrecio.Text)
            );

            CargaMasivaService.repuestos.append(repuesto);
            Console.WriteLine($"✅ Repuesto {repuesto.Repuesto1} agregado correctamente.");
        };
        
        vbox.PackStart(txtId, false, false, 5);
        vbox.PackStart(txtNombre, false, false, 5);
        vbox.PackStart(txtDescripcion, false, false, 5);
        vbox.PackStart(txtPrecio, false, false, 5);
        vbox.PackStart(btnGuardar, false, false, 5);
        
        return vbox;
    }
    
    public VBox CrearFormularioServicios()
    {
        VBox vbox = new VBox();

        Entry txtId = new Entry { PlaceholderText = "ID" };
        Entry txtRepuesto = new Entry { PlaceholderText = "ID Repuesto" };
        Entry txtVehiculo = new Entry { PlaceholderText = "ID Vehículo" };
        Entry txtDetalle = new Entry { PlaceholderText = "Detalle" };
        Entry txtCosto = new Entry { PlaceholderText = "Costo" };
        Button btnGuardar = new Button("Guardar Servicio");
        
        btnGuardar.Clicked += (sender, e) =>
        {
            if (string.IsNullOrEmpty(txtVehiculo.Text) || string.IsNullOrEmpty(txtRepuesto.Text))
            {
                MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Todos los campos son obligatorios.");
                errorDialog.Run();
                errorDialog.Destroy();
                return;
            }

            Servicio servicio = new Servicio
            (
                int.Parse(txtId.Text),
                int.Parse(txtRepuesto.Text),
                int.Parse(txtVehiculo.Text),
                txtDetalle.Text,
                double.Parse(txtCosto.Text)
            );

            CargaMasivaService.servicios.enqueue(servicio);
            Console.WriteLine($"✅ Servicio {servicio.Id} agregado correctamente.");
        };
        
        vbox.PackStart(txtId, false, false, 5);
        vbox.PackStart(txtRepuesto, false, false, 5);
        vbox.PackStart(txtVehiculo, false, false, 5);
        vbox.PackStart(txtDetalle, false, false, 5);
        vbox.PackStart(txtCosto, false, false, 5);
        vbox.PackStart(btnGuardar, false, false, 5);
        
        return vbox;
    }

}