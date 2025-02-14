using AutoGestPro.Core.Services;  // 📌 Servicio de autenticación
using AutoGestPro.UI.Handlers;     // 📌 Manejador de eventos
using AutoGestPro.UI.Windows;      // 📌 Importar MainWindow.cs

using System;
using Gtk;
using EventHandler = AutoGestPro.UI.Handlers.EventHandler;

namespace AutoGestPro.UI.Windows;

public class LoginWindow: Window
{
    private Entry usuarioEntry;
    private Entry contrasenaEntry;
    private UsuarioService usuarioService;

    public LoginWindow() : base("Inicio de Sesión")
    {
        usuarioService = new UsuarioService();

        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox { BorderWidth = 10 };

        Label lblUsuario = new Label("Usuario:");
        usuarioEntry = new Entry();
        Label lblContrasena = new Label("Contraseña:");
        contrasenaEntry = new Entry { Visibility = false };

        Button btnLogin = new Button("Iniciar Sesión");
        btnLogin.Clicked += OnLoginClicked;

        vbox.PackStart(lblUsuario, false, false, 5);
        vbox.PackStart(usuarioEntry, false, false, 5);
        vbox.PackStart(lblContrasena, false, false, 5);
        vbox.PackStart(contrasenaEntry, false, false, 5);
        vbox.PackStart(btnLogin, false, false, 10);

        Add(vbox);
        ShowAll();
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        string usuario = usuarioEntry.Text;
        string contrasena = contrasenaEntry.Text;

        if (usuarioService.ValidarCredenciales(usuario, contrasena))
        {
            Console.WriteLine("✅ Inicio de sesión exitoso");

            // 📌 Cierra la ventana de Login
            Hide();

            // 📌 Abre la ventana principal
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
        else
        {
            // 📌 Usa el manejador para mostrar mensaje de error
            EventHandler.MostrarMensaje("❌ Usuario o contraseña incorrectos");
        }
    }
}