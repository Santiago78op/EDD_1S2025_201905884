namespace AutoGestPro.UI.Windows;
using MenuButton = AutoGestPro.UI.Components.MenuButton;

using System;
using Gtk;

public class MainWindow : Window
{
    public MainWindow() : base("Menú Principal")
    {
        SetDefaultSize(400, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox { BorderWidth = 10 };

        Label titulo = new Label("<b>Menú</b>") { UseMarkup = true };
        titulo.SetAlignment(0.5f, 0.5f);

        // 📌 Usamos los botones desde Components/
        vbox.PackStart(new MenuButton("Cargas Masivas", OnCargasMasivasClicked), false, false, 5);
        /**
        vbox.PackStart(new MenuButton("Ingreso Individual", OnIngresoIndividualClicked), false, false, 5);
        vbox.PackStart(new MenuButton("Gestión de Usuarios", OnGestionUsuariosClicked), false, false, 5);
        vbox.PackStart(new MenuButton("Generar Servicio", OnGenerarServicioClicked), false, false, 5);
        vbox.PackStart(new MenuButton("Cancelar Factura", OnCancelarFacturaClicked), false, false, 5);
        vbox.PackStart(new MenuButton("Generación de Reportes", OnGenerarReportesClicked), false, false, 5);
        */
        
        // 📌 Botón para cerrar sesión
        Button btnCerrarSesion = new Button("Cerrar Sesión");
        btnCerrarSesion.ModifyBg(StateType.Normal, new Gdk.Color(255, 0, 0)); // Color rojo
        btnCerrarSesion.Clicked += OnCerrarSesionClicked;

        vbox.PackStart(btnCerrarSesion, false, false, 10);
        
        Add(vbox);
        ShowAll();
    }

    private void OnCargasMasivasClicked(object sender, EventArgs e) => new CargasMasivas().Show();
    /**
    private void OnIngresoIndividualClicked(object sender, EventArgs e) => new IngresoIndividual().Show();
    private void OnGestionUsuariosClicked(object sender, EventArgs e) => new GestionUsuarios().Show();
    private void OnGenerarServicioClicked(object sender, EventArgs e) => new GenerarServicio().Show();
    private void OnCancelarFacturaClicked(object sender, EventArgs e) => new CancelarFactura().Show();
    private void OnGenerarReportesClicked(object sender, EventArgs e) => new Reportes().Show();
    */
    
    // 📌 Función para cerrar sesión
    private void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        MessageDialog confirmDialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Question,
            ButtonsType.YesNo,
            "¿Seguro que deseas cerrar sesión?"
        );

        if (confirmDialog.Run() == (int)ResponseType.Yes)
        {
            confirmDialog.Destroy();
            Hide(); // Oculta la ventana actual
            LoginWindow login = new LoginWindow();
            login.Show();
        }
        else
        {
            confirmDialog.Destroy();
        }
    }
}