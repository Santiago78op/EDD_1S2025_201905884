using AutoGestPro.Core.Models;
using AutoGestPro.Core.Services;
using Gtk;

namespace AutoGestPro.UI.User;

public class InsertarVehiculo : Window
{
    private Entry entryMarca, entryModelo, entryPlaca;
    private Button btnRegistrar;
    private int usuarioActualId; // ID del usuario autenticado

    public InsertarVehiculo(int idUsuario) : base("Registrar Vehículo")
    {
        usuarioActualId = idUsuario;

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide();

        VBox vbox = new VBox(false, 5) { BorderWidth = 10 };

        Label lblMarca = new Label("Marca:");
        entryMarca = new Entry();

        Label lblModelo = new Label("Modelo:");
        entryModelo = new Entry();

        Label lblPlaca = new Label("Placa:");
        entryPlaca = new Entry();

        btnRegistrar = new Button("Registrar Vehículo");
        btnRegistrar.Clicked += OnRegistrarVehiculoClicked;

        vbox.PackStart(lblMarca, false, false, 5);
        vbox.PackStart(entryMarca, false, false, 5);
        vbox.PackStart(lblModelo, false, false, 5);
        vbox.PackStart(entryModelo, false, false, 5);
        vbox.PackStart(lblPlaca, false, false, 5);
        vbox.PackStart(entryPlaca, false, false, 5);
        vbox.PackStart(btnRegistrar, false, false, 10);

        Add(vbox);
        ShowAll();
    }

    // ✅ Método para registrar un vehículo
    private void OnRegistrarVehiculoClicked(object sender, EventArgs e)
    {
        string marca = entryMarca.Text.Trim();
        string modelo = entryModelo.Text.Trim();
        string placa = entryPlaca.Text.Trim();

        if (string.IsNullOrEmpty(marca) || string.IsNullOrEmpty(modelo) || string.IsNullOrEmpty(placa))
        {
            MostrarMensaje("Error", "Todos los campos son obligatorios.");
            return;
        }

        // 🔥 Crear un nuevo vehículo
        Vehiculo nuevoVehiculo =
            new Vehiculo(CargaMasivaService.vehiculos.Length + 1, usuarioActualId, marca, modelo, placa);

        // 🔥 Insertar en la lista de vehículos
        CargaMasivaService.vehiculos.Append(nuevoVehiculo);

        MostrarMensaje("Éxito", "Vehículo registrado correctamente.");
        Hide();
    }

    // ✅ Muestra un mensaje de alerta
    private void MostrarMensaje(string titulo, string mensaje)
    {
        MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
        dialog.Title = titulo;
        dialog.Run();
        dialog.Destroy();
    }
}