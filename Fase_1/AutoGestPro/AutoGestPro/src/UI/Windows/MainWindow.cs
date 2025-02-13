namespace AutoGestPro.UI.Windows;

using System;
using Gtk;
public class MainWindow : Window
{
    public MainWindow() : base("Menú Principal")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        Button salirButton = new Button("Cerrar Sesión");
        salirButton.Clicked += (sender, e) =>
        {
            Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        };

        VBox vbox = new VBox { BorderWidth = 10 };
        vbox.PackStart(new Label("Bienvenido al sistema AutoGest Pro"), false, false, 10);
        vbox.PackStart(salirButton, false, false, 10);

        Add(vbox);
        ShowAll();
    }
}