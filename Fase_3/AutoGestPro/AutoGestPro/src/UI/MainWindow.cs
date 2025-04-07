using AutoGestPro.Core.Services;
using AutoGestPro.UI.Views.Admin;
using AutoGestPro.UI.Views.Shared;
using AutoGestPro.UI.Views.User;

namespace AutoGestPro.UI;

using System;
using Gtk;

/// <summary>
/// Ventana principal que contiene el menú de navegación y la lógica de inicio de sesión.
/// </summary>
public class MainWindow : Window
{
    private VBox _mainVBox;
    private MenuBar _menuBar;
    private HBox _menuActionsBox;
    private Label _welcomeLabel;

    public MainWindow() : base("AutoGestPro - Sistema de Taller")
    {
        SetDefaultSize(600, 300);
        SetPosition(WindowPosition.Center);
        ApplyStyles();
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
        _menuActionsBox = new HBox(false, 5); // Inicializar _menuActionsBox
        _menuBar = CreateMenuBar();

        _welcomeLabel = new Label
        {
            Text = $"Bienvenido {Sesion.UsuarioActual?.Nombres} a AutoGestPro",
            Justify = Justification.Center,
            Valign = Align.Start,
            Halign = Align.Center
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

        // Añadir botones al menú de acciones según rol
        foreach (var c in _menuActionsBox.Children)
        {
            _menuActionsBox.Remove(c);
        }

        if (Sesion.UsuarioActual != null)
        {
            if (Sesion.UsuarioActual.EsAdmin())
            {
                var adminActions = new MenuAdministrador();
                _menuActionsBox.PackStart(CreateActionButton("Gest. Usuarios", adminActions.OnGestionUsuarios), false,
                    false, 5);
                _menuActionsBox.PackStart(CreateActionButton("Gest. Vehículos", adminActions.OnGestionVehiculos), false,
                    false, 5);
                _menuActionsBox.PackStart(CreateActionButton("Gest. Repuestos", adminActions.OnGestionRepuestos), false,
                    false, 5);
                _menuActionsBox.PackStart(CreateActionButton("Servicios", adminActions.OnGestionServicios), false,
                    false, 5);
                _menuActionsBox.PackStart(CreateActionButton("Facturas", adminActions.OnGestionFacturas), false, false,
                    5);
                _menuActionsBox.PackStart(CreateActionButton("Reportes", adminActions.OnGenerarReportes), false, false,
                    5);
                _menuActionsBox.PackStart(CreateActionButton("Bitácora", adminActions.OnMostrarBitacora), false, false,
                    5);
            }
            else
            {
                var userActions = new MenuUsuario();
                /*
                _menuActionsBox.PackStart(CreateActionButton("Mis Vehículos", userActions.OnMisVehiculos), false, false, 5);
                _menuActionsBox.PackStart(CreateActionButton("Servicios", userActions.OnMisServicios), false, false, 5);
                _menuActionsBox.PackStart(CreateActionButton("Facturas", userActions.OnMisFacturas), false, false, 5);
            */
            }
        }

        return menuBar;
    }

    /// <summary>
    /// Crea un botón de acción con una etiqueta y un manejador de eventos.
    /// </summary>
    /// <param name="label">Etiqueta del botón.</param>
    /// <param name="handler">Manejador de eventos para el botón.</param>
    private Button CreateActionButton(string label, EventHandler handler)
    {
        var button = new Button(label);
        button.Clicked += handler;
        return button;
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
    /// Aplica estilos CSS a la ventana.
    /// </summary>
    private void ApplyStyles()
    {
        var cssProvider = new CssProvider();
        string cssPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "../../../src/UI/Assets/Styles/login.css");
        cssProvider.LoadFromPath(cssPath);
        StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssProvider, 800);
    }

    /// <summary>
    /// Evento de cierre de la ventana.
    /// </summary>
    private void OnDeleteEvent(object sender, DeleteEventArgs args)
    {
        Application.Quit();
    }
}