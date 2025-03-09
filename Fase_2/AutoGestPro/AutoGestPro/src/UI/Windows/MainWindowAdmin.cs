using AutoGestPro.UI.Admin;
using Gtk;
using MenuButton = AutoGestPro.UI.Components.MenuButton;

namespace AutoGestPro.UI.Windows;

public class MainWindowAdmin : Window
{
    public MainWindowAdmin() : base("Menú Principal")
    {
        SetDefaultSize(400, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Box vbox = new Box(Orientation.Vertical, 10) { BorderWidth = 10 };

        Label titulo = new Label("<b>Menú</b>") { UseMarkup = true };
        titulo.Halign = Align.Center;
        titulo.Valign = Align.Center;

        // 📌 Usamos los botones desde Components/
        vbox.PackStart(new MenuButton("Cargas Masivas", OnCargasMasivasClicked, "00FF00", "Black"), false, false, 5);
        vbox.PackStart(new MenuButton("Gestión de Usuarios", OnGestionUsuariosClicked, "00FF00", "Black"), false, false, 5);
        /*
        
        vbox.PackStart(new MenuButton("Generar Servicio", OnGenerarServicioClicked), false, false, 5);
        vbox.PackStart(new MenuButton("Cancelar Factura", OnCancelarFacturaClicked), false, false, 5);
        vbox.PackStart(new MenuButton("Generación de Reportes", OnGenerarReportesClicked), false, false, 5);*/


        // 📌 Botón para cerrar sesión 
        Button btnCerrarSesion = new Button("Cerrar Sesión");
        
        // Aplicar un nombre de clase CSS
        btnCerrarSesion.Name = "btnCerrarSesion";
        
        var provider = new CssProvider();
        provider.LoadFromData(@"
            #btnCerrarSesion {
                background-image: none;
                background-color: #FF0000;
                color: black;
                border-radius: 10px;
                padding: 5px;
            }
        ");
        
        StyleContext.AddProviderForScreen(Screen, provider, 800);
        

        btnCerrarSesion.Clicked += OnCerrarSesionClicked;

        vbox.PackStart(btnCerrarSesion, false, false, 10);

        Add(vbox);
        ShowAll();
    }

    private void OnCargasMasivasClicked(object? sender, EventArgs e) => new CargasMasivas().Show();
    private void OnGestionUsuariosClicked(object? sender, EventArgs e) => new GestionEntidadesWindow().Show();
    /*
    private void OnGenerarServicioClicked(object sender, EventArgs e) => new GenerarServicio().Show();
    private void OnCancelarFacturaClicked(object sender, EventArgs e) => new CancelarFactura().Show();
    private void OnGenerarReportesClicked(object sender, EventArgs e) => new GenerarReportes().Show();*/

    // 📌 Función para cerrar sesión
    private void OnCerrarSesionClicked(object? sender, EventArgs e)
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