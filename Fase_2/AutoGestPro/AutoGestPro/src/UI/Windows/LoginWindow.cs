
using AutoGestPro.Core.Services;
using AutoGestPro.UI.Admin;
using Gtk;
using EventHandler = AutoGestPro.UI.Handlers.EventHandler;

namespace AutoGestPro.UI.Windows;

public class LoginWindow:Window
{
    private static Entry? _usuarioEntry;
    private Entry _contrasenaEntry;
    private UsuarioService _usuarioService;

    public static Entry? UsuarioEntry
    {
        get => _usuarioEntry;
        set => _usuarioEntry = value ?? throw new ArgumentNullException(nameof(value));
    }

    public LoginWindow() : base("Inicio de Sesión")
    {
        _usuarioService = new UsuarioService();

        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox { BorderWidth = 10 };

        Label lblUsuario = new Label("Usuario:");
        _usuarioEntry = new Entry();
        Label lblContrasena = new Label("Contraseña:");
        _contrasenaEntry = new Entry { Visibility = false };

        Button btnLogin = new Button("Iniciar Sesión");
        btnLogin.Clicked += OnLoginClicked;

        vbox.PackStart(lblUsuario, false, false, 5);
        vbox.PackStart(_usuarioEntry, false, false, 5);
        vbox.PackStart(lblContrasena, false, false, 5);
        vbox.PackStart(_contrasenaEntry, false, false, 5);
        vbox.PackStart(btnLogin, false, false, 10);

        Add(vbox);
        ShowAll();
    }

    private void OnLoginClicked(object? sender, EventArgs e)
    {
        string usuario = _usuarioEntry.Text;
        string contrasena = _contrasenaEntry.Text;

        if (_usuarioService.ValidarCredencialesUsuario(usuario, contrasena))
        {
            // 🔥 Registra la entrada
            ControlLogueo.RegistrarEntrada(usuario);
            EventHandler.MostrarMensaje($"Bienvenido Inicio de sesión exitoso para {usuario}");
            Console.WriteLine("✅ Inicio de sesión exitoso");

            // 📌 Cierra la ventana de Login
            Hide();

            // 📌 Abre la ventana principal del Administrador
            MainWindowAdmin mainWindowAdmin = new MainWindowAdmin();
            mainWindowAdmin.Show();
        } else if (_usuarioService.ValidarCredencialesCliente(usuario, contrasena))
        {
            
            // 🔥 Registra la entrada
            ControlLogueo.RegistrarEntrada(usuario);
            EventHandler.MostrarMensaje($"Bienvenido Inicio de sesión exitoso para {usuario}");
            Console.WriteLine("✅ Inicio de sesión exitoso");

            // 📌 Cierra la ventana de Login
            Hide();

            // 📌 Abre la ventana principal del Cliente
            MainWindowUser mainWindowUser = new MainWindowUser();
            mainWindowUser.Show();
        }
        else
        {
            // 📌 Usa el manejador para mostrar mensaje de error
            EventHandler.MostrarMensaje("❌ Usuario o contraseña incorrectos");
        }
    }
}