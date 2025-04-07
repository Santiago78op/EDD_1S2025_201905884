using AutoGestPro.UI.Views.Admin;
using AutoGestPro.UI.Views.Shared;

namespace AutoGestPro.UI;

using System;
using Gtk;
using AutoGestPro.UI.Admin;
using AutoGestPro.UI.User;
using AutoGestPro.Core.Global;
using AutoGestPro.Shared;

/// <summary>
/// Ventana principal que contiene el menú de navegación y la lógica de inicio de sesión.
/// </summary>
public class MainWindow : Window
{
    private VBox _mainVBox;
    private MenuBar _menuBar;
    private Label _welcomeLabel;

    public MainWindow() : base("AutoGestPro - Sistema de Taller")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += OnDeleteEvent;

        BuildLoginUI();
    }

    /// <summary>
    /// Construye la interfaz de logeo inicial.
    /// </summary>
    private void BuildLoginUI()
    {
        _mainVBox = new VBox(false, 5);
        var loginView = new LoginView();
        loginView.LoginExitoso += OnLoginExitoso;
        _mainVBox.PackStart(loginView, true, true, 0);

        Add(_mainVBox);
        ShowAll();
    }

    /// <summary>
    /// Callback cuando el login es exitoso.
    /// </summary>
    private void OnLoginExitoso(object sender, EventArgs e)
    {
        _mainVBox.Destroy();
        BuildUI();
    }

    /// <summary>
    /// Construye la interfaz gráfica de la ventana principal.
    /// </summary>
    private void BuildUI()
    {
        _mainVBox = new VBox(false, 5);
        _menuBar = CreateMenuBar();

        _welcomeLabel = new Label
        {
            Text = $"Bienvenido {Sesion.UsuarioActual?.Nombres} a AutoGestPro",
            Justify = Justification.Center,
            MarginTop = 50
        };

        _mainVBox.PackStart(_menuBar, false, false, 0);
        _mainVBox.PackStart(_welcomeLabel, true, true, 0);
        Add(_mainVBox);
        ShowAll();
    }

    /// <summary>
    /// Crea la barra de menú principal con opciones según el tipo de usuario.
    /// </summary>
    private MenuBar CreateMenuBar()
    {
        var menuBar = new MenuBar();

        var archivoItem = new MenuItem("Archivo");
        var archivoMenu = new Menu();
        archivoItem.Submenu = archivoMenu;

        var cerrarSesionItem = new MenuItem("Cerrar Sesión");
        cerrarSesionItem.Activated += (s, e) => ReiniciarAplicacion();
        archivoMenu.Append(cerrarSesionItem);

        var salirItem = new MenuItem("Salir");
        salirItem.Activated += (s, e) => Application.Quit();
        archivoMenu.Append(salirItem);
        menuBar.Append(archivoItem);

        if (Sesion.UsuarioActual != null)
        {
            if (Sesion.UsuarioActual.EsAdmin)
            {
                var adminMenuItem = new MenuItem("Administrador");
                var adminMenu = new Menu();
                adminMenuItem.Submenu = adminMenu;

                var adminActions = new MenuAdministrador();

                adminMenu.Append(CreateMenuItem("Gest. Usuarios", adminActions.OnGestionUsuarios));
                adminMenu.Append(CreateMenuItem("Gest. Vehículos", adminActions.OnGestionVehiculos));
                adminMenu.Append(CreateMenuItem("Gest. Repuestos", adminActions.OnGestionRepuestos));
                adminMenu.Append(CreateMenuItem("Servicios", adminActions.OnGestionServicios));
                adminMenu.Append(CreateMenuItem("Facturas", adminActions.OnGestionFacturas));
                adminMenu.Append(CreateMenuItem("Reportes", adminActions.OnGenerarReportes));
                adminMenu.Append(CreateMenuItem("Bitácora", adminActions.OnMostrarBitacora));

                menuBar.Append(adminMenuItem);
            }
            else
            {
                var userMenuItem = new MenuItem("Usuario");
                var userMenu = new Menu();
                userMenuItem.Submenu = userMenu;

                var userActions = new MenuUsuario();

                userMenu.Append(CreateMenuItem("Mis Vehículos", userActions.OnMisVehiculos));
                userMenu.Append(CreateMenuItem("Servicios", userActions.OnMisServicios));
                userMenu.Append(CreateMenuItem("Facturas", userActions.OnMisFacturas));

                menuBar.Append(userMenuItem);
            }
        }

        return menuBar;
    }

    /// <summary>
    /// Crea un ítem de menú con una acción asociada.
    /// </summary>
    /// <param name="text">Texto del menú.</param>
    /// <param name="handler">Acción a ejecutar.</param>
    private MenuItem CreateMenuItem(string text, EventHandler handler)
    {
        var item = new MenuItem(text);
        item.Activated += handler;
        return item;
    }

    /// <summary>
    /// Reinicia la aplicación para volver al login.
    /// </summary>
    private void ReiniciarAplicacion()
    {
        _mainVBox.Destroy();
        Sesion.UsuarioActual = null;
        BuildLoginUI();
    }

    /// <summary>
    /// Evento de cierre de la ventana.
    /// </summary>
    private void OnDeleteEvent(object sender, DeleteEventArgs args)
    {
        Application.Quit();
    }
}