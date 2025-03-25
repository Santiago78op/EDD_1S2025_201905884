using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Services;
using Gtk;

namespace AutoGestPro.UI.User;

public class InsertarVehiculo : Window
{
    private Entry _entryId , _entryMarca, _entryModelo, _entryPlaca;
    private Button _btnRegistrar;
    private int _usuarioActualId; // ID del usuario autenticado

    public InsertarVehiculo(int idUsuario) : base("Registrar Vehículo")
    {
        _usuarioActualId = idUsuario;

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide();

        VBox vbox = new VBox(false, 5) { BorderWidth = 10 };

        Label idVehiculo = new Label("ID:");
        _entryId = new Entry();

        Label lblMarca = new Label("Marca:");
        _entryMarca = new Entry();

        Label lblModelo = new Label("Modelo:");
        _entryModelo = new Entry();

        Label lblPlaca = new Label("Placa:");
        _entryPlaca = new Entry();

        _btnRegistrar = new Button("Registrar Vehículo");
        _btnRegistrar.Clicked += OnRegistrarVehiculoClicked;

        vbox.PackStart(idVehiculo, false, false, 5);
        vbox.PackStart(_entryId, false, false, 5);
        vbox.PackStart(lblMarca, false, false, 5);
        vbox.PackStart(_entryMarca, false, false, 5);
        vbox.PackStart(lblModelo, false, false, 5);
        vbox.PackStart(_entryModelo, false, false, 5);
        vbox.PackStart(lblPlaca, false, false, 5);
        vbox.PackStart(_entryPlaca, false, false, 5);
        vbox.PackStart(_btnRegistrar, false, false, 10);

        Add(vbox);
        ShowAll();
    }

    // ✅ Método para registrar un vehículo
    private void OnRegistrarVehiculoClicked(object? sender, EventArgs e)
    {
        string marca = _entryMarca.Text.Trim();
        string placa = _entryPlaca.Text.Trim();

        if ( int.TryParse(_entryId.Text, out _) == false
             || string.IsNullOrEmpty(marca)
             || int.TryParse(_entryModelo.Text, out _) == false
             || string.IsNullOrEmpty(placa))
        {
            MostrarMensaje("Error", "Todos los campos son obligatorios, Id y Modelo deben ser numéricos.");
            return;
        }

        int id = int.Parse(_entryId.Text.Trim());
        int modelo = int.Parse(_entryModelo.Text.Trim());

        // Error si los datos numéricos son negativos
        if (id < 0 || modelo < 0)
        {
            MostrarMensaje("Error", "Los campos numéricos deben ser positivos.");
            return;
        }

        // Validar que el ID no exista
        NodeDouble? current = Estructuras.Vehiculos.Head;

        while (current != null)
        {
            Vehiculo vehiculo = (Vehiculo)current.Data;
            if (vehiculo.Id == id)
            {
                MostrarMensaje("Error", "El ID ya existe.");
                return;
            }
            current = current.Next;
        }

        // Crear el vehículo
        // 🔥 Crear un nuevo vehículo
        Vehiculo nuevoVehiculo =
            new Vehiculo(id, _usuarioActualId, marca, modelo, placa);

        // 🔥 Insertar en la lista de vehículos
        Estructuras.Vehiculos.Append(nuevoVehiculo);

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

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _entryId?.Dispose();
            _entryMarca?.Dispose();
            _entryModelo?.Dispose();
            _entryPlaca?.Dispose();
            _btnRegistrar?.Dispose();
        }
        base.Dispose(disposing);
    }
}