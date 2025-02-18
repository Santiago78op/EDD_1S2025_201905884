using AutoGestPro.Core.Services;
using Gtk;
using System;
using AutoGestPro.Core.Models;

namespace AutoGestPro.UI.Windows;

public unsafe class GenerarServicio : Window
{
    private Entry entryUsuarioId, entryVehiculoId, entryDescripcion, entryRepuestoId, entryCantidad;
    private ComboBoxText comboTipoServicio;
    private ServicioService servicioService;
    private UsuarioService usuarioService;
    private VehiculoService vehiculoService;
    private RepuestoService repuestoService;

    public GenerarServicio() : base("Generar Servicio")
    {
        servicioService = new ServicioService();
        usuarioService = new UsuarioService();
        vehiculoService = new VehiculoService();
        repuestoService = new RepuestoService();

        SetDefaultSize(500, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide(); // Oculta en vez de cerrar

        VBox vbox = new VBox(false, 5);

        Label labelTitulo = new Label("<b>Registro de Servicio</b>") { UseMarkup = true };
        vbox.PackStart(labelTitulo, false, false, 5);

        // ID Usuario
        HBox hboxUsuario = new HBox(false, 5);
        Label labelUsuario = new Label("ID Usuario:");
        entryUsuarioId = new Entry();
        hboxUsuario.PackStart(labelUsuario, false, false, 5);
        hboxUsuario.PackStart(entryUsuarioId, true, true, 5);
        vbox.PackStart(hboxUsuario, false, false, 5);

        // ID Vehículo
        HBox hboxVehiculo = new HBox(false, 5);
        Label labelVehiculo = new Label("ID Vehículo:");
        entryVehiculoId = new Entry();
        hboxVehiculo.PackStart(labelVehiculo, false, false, 5);
        hboxVehiculo.PackStart(entryVehiculoId, true, true, 5);
        vbox.PackStart(hboxVehiculo, false, false, 5);

        // Tipo de Servicio
        HBox hboxTipoServicio = new HBox(false, 5);
        Label labelTipoServicio = new Label("Tipo de Servicio:");
        comboTipoServicio = new ComboBoxText();
        comboTipoServicio.AppendText("Mantenimiento");
        comboTipoServicio.AppendText("Reparación");
        comboTipoServicio.AppendText("Diagnóstico");
        comboTipoServicio.Active = 0;
        hboxTipoServicio.PackStart(labelTipoServicio, false, false, 5);
        hboxTipoServicio.PackStart(comboTipoServicio, true, true, 5);
        vbox.PackStart(hboxTipoServicio, false, false, 5);

        // Descripción del Servicio
        HBox hboxDescripcion = new HBox(false, 5);
        Label labelDescripcion = new Label("Descripción:");
        entryDescripcion = new Entry();
        hboxDescripcion.PackStart(labelDescripcion, false, false, 5);
        hboxDescripcion.PackStart(entryDescripcion, true, true, 5);
        vbox.PackStart(hboxDescripcion, false, false, 5);

        // ID Repuesto
        HBox hboxRepuesto = new HBox(false, 5);
        Label labelRepuesto = new Label("ID Repuesto:");
        entryRepuestoId = new Entry();
        hboxRepuesto.PackStart(labelRepuesto, false, false, 5);
        hboxRepuesto.PackStart(entryRepuestoId, true, true, 5);
        vbox.PackStart(hboxRepuesto, false, false, 5);

        // Cantidad de Repuestos
        HBox hboxCantidad = new HBox(false, 5);
        Label labelCantidad = new Label("Cantidad:");
        entryCantidad = new Entry();
        hboxCantidad.PackStart(labelCantidad, false, false, 5);
        hboxCantidad.PackStart(entryCantidad, true, true, 5);
        vbox.PackStart(hboxCantidad, false, false, 5);

        // Botón para generar servicio
        Button btnGenerarServicio = new Button("Generar Servicio");
        btnGenerarServicio.Clicked += OnGenerarServicioClicked;
        vbox.PackStart(btnGenerarServicio, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    // ✅ Genera el servicio validando usuario, vehículo y repuesto
    private void OnGenerarServicioClicked(object sender, EventArgs e)
    {
        int usuarioId, vehiculoId, repuestoId, cantidad;

        if (!int.TryParse(entryUsuarioId.Text, out usuarioId) ||
            !int.TryParse(entryVehiculoId.Text, out vehiculoId) ||
            !int.TryParse(entryRepuestoId.Text, out repuestoId) ||
            !int.TryParse(entryCantidad.Text, out cantidad))
        {
            MostrarMensaje("Error", "Ingrese valores numéricos válidos.");
            return;
        }
        
        if (CargaMasivaService.clientes.SearchNode(usuarioId)->_data == null)
        {
            MostrarMensaje("Error", "El usuario no existe.");
            return;
        }

        if (CargaMasivaService.vehiculos.searchNode(vehiculoId)->_data == null)
        {
            MostrarMensaje("Error", "El vehículo no existe.");
            return;
        }

        Repuesto repuesto = repuestoService.ObtenerRepuestoPorId(repuestoId);
        if (CargaMasivaService.repuestos == null)
        {
            MostrarMensaje("Error", "El repuesto no existe.");
            return;
        }

        decimal costoRepuesto = repuesto.Costo * cantidad;

        // Crear el servicio
        Servicio nuevoServicio = new Servicio
        {
            ID = servicioService.ObtenerNuevoId(),
            UsuarioID = usuarioId,
            VehiculoID = vehiculoId,
            Tipo = comboTipoServicio.ActiveText,
            Descripcion = entryDescripcion.Text,
            RepuestoID = repuestoId,
            Cantidad = cantidad
        };

        servicioService.RegistrarServicio(nuevoServicio, costoRepuesto);
        MostrarMensaje("Éxito", "Servicio y factura generados correctamente.");
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