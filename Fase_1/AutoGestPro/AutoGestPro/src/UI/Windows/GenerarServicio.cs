using AutoGestPro.Core.Services;
using Gtk;
using System;
using AutoGestPro.Core.Models;

namespace AutoGestPro.UI.Windows;

public unsafe class GenerarServicio : Window
{
    private Entry entryServicioId, entryRepuestoId, entryVehiculoId, entryDescripcion, entryCosto;
    private ServicioService servicioService;

    public GenerarServicio() : base("Generar Servicio")
    {
        servicioService = new ServicioService();

        SetDefaultSize(500, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide(); // Oculta en vez de cerrar

        VBox vbox = new VBox(false, 5);

        Label labelTitulo = new Label("<b>Registro de Servicio</b>") { UseMarkup = true };
        vbox.PackStart(labelTitulo, false, false, 5);
        
        // ID Servicio
        HBox hboxServicio = new HBox(false, 5);
        Label labelServicio = new Label("ID Servicio:");
        entryServicioId = new Entry();
        hboxServicio.PackStart(labelServicio, false, false, 5);
        hboxServicio.PackStart(entryServicioId, true, true, 5);
        vbox.PackStart(hboxServicio, false, false, 5);
        
        // ID Repuesto
        HBox hboxRepuesto = new HBox(false, 5);
        Label labelRepuesto = new Label("ID Repuesto:");
        entryRepuestoId = new Entry();
        hboxRepuesto.PackStart(labelRepuesto, false, false, 5);
        hboxRepuesto.PackStart(entryRepuestoId, true, true, 5);
        vbox.PackStart(hboxRepuesto, false, false, 5);

        // ID Vehículo
        HBox hboxVehiculo = new HBox(false, 5);
        Label labelVehiculo = new Label("ID Vehículo:");
        entryVehiculoId = new Entry();
        hboxVehiculo.PackStart(labelVehiculo, false, false, 5);
        hboxVehiculo.PackStart(entryVehiculoId, true, true, 5);
        vbox.PackStart(hboxVehiculo, false, false, 5);

        // Descripción del Servicio
        HBox hboxDescripcion = new HBox(false, 5);
        Label labelDescripcion = new Label("Descripción:");
        entryDescripcion = new Entry();
        hboxDescripcion.PackStart(labelDescripcion, false, false, 5);
        hboxDescripcion.PackStart(entryDescripcion, true, true, 5);
        vbox.PackStart(hboxDescripcion, false, false, 5);
        
        // Costo del Servicio
        HBox hboxCosto = new HBox(false, 5);
        Label labelCosto = new Label("Costo Servicio:");
        entryCosto = new Entry();
        hboxCosto.PackStart(labelCosto, false, false, 5);
        hboxCosto.PackStart(entryCosto, true, true, 5);
        vbox.PackStart(hboxCosto, false, false, 5);
        
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
        try
        {
            int servicioId, repuestoId, vehiculoId;
            decimal costoServicio;

            if (string.IsNullOrWhiteSpace(entryServicioId.Text) ||
                string.IsNullOrWhiteSpace(entryVehiculoId.Text) ||
                string.IsNullOrWhiteSpace(entryRepuestoId.Text) ||
                string.IsNullOrWhiteSpace(entryDescripcion.Text) ||
                string.IsNullOrWhiteSpace(entryCosto.Text))
            {
                MostrarMensaje("Error", "Todos los campos deben estar completos.");
                return;
            }

            if (!int.TryParse(entryServicioId.Text, out servicioId) ||
                !int.TryParse(entryVehiculoId.Text, out vehiculoId) ||
                !int.TryParse(entryRepuestoId.Text, out repuestoId))
            {
                MostrarMensaje("Error", "Ingrese valores numéricos válidos.");
                return;
            }

            if (!decimal.TryParse(entryCosto.Text, out costoServicio))
            {
                MostrarMensaje("Error", "Ingrese un costo de servicio válido.");
                return;
            }

            if (CargaMasivaService.vehiculos.searchNode(vehiculoId) == null)
            {
                MostrarMensaje("Error", "El vehículo no existe.");
                return;
            }

            Repuesto repuesto;
            if (CargaMasivaService.repuestos.searchNode(repuestoId) == null)
            {
                MostrarMensaje("Error", "El repuesto no existe.");
                return;
            }
            else
            {
                repuesto = CargaMasivaService.repuestos.searchNode(repuestoId)->_data;
            }


            // Crear el servicio
            Servicio nuevoServicio = new Servicio
            (
                servicioId,
                repuestoId,
                vehiculoId,
                entryDescripcion.Text,
                Convert.ToDouble(costoServicio)
            );

            servicioService.RegistrarServicio(nuevoServicio, repuesto.Costo);
            MostrarMensaje("Éxito", "Servicio y factura generados correctamente.");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
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