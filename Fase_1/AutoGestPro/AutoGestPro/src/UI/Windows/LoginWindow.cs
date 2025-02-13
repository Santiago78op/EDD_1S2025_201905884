using AutoGestPro.Core.Services;

namespace AutoGestPro.UI.Windows;

using System;
using Gtk;

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

        Label usuarioLabel = new Label("Usuario:");
        usuarioEntry = new Entry();

        Label contrasenaLabel = new Label("Contraseña:");
        contrasenaEntry = new Entry { Visibility = false };

        Button loginButton = new Button("Iniciar Sesión");
        loginButton.Clicked += OnLoginClicked;

        vbox.PackStart(usuarioLabel, false, false, 5);
        vbox.PackStart(usuarioEntry, false, false, 5);
        vbox.PackStart(contrasenaLabel, false, false, 5);
        vbox.PackStart(contrasenaEntry, false, false, 5);
        vbox.PackStart(loginButton, false, false, 10);

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
            Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
        else
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "❌ Usuario o contraseña incorrectos");
            dialog.Run();
            dialog.Destroy();
        }
    }
}