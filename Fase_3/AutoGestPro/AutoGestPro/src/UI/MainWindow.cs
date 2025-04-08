using AutoGestPro.Core.Services;
using AutoGestPro.UI.Extensions;
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
    private VBox _menuActionsBox;
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
        _mainVBox = new VBox(false, 10); // Aumentar espacio vertical

        // Bienvenida
        _welcomeLabel = new Label
        {
            Text = $"Bienvenido {Sesion.UsuarioActual?.Nombres} a AutoGestPro",
            Justify = Justification.Center,
            Valign = Align.Start,
            Halign = Align.Center
        };
        _welcomeLabel.AddCssClass("label-titulo");

        // Crea el contenedor de botones de acción
        _menuActionsBox = CreateActionButtons();

        // Organizar la interfaz
        _mainVBox.PackStart(_welcomeLabel, false, false, 15); // Más espacio para el título
        _mainVBox.PackStart(_menuActionsBox, true, true, 0);
        Add(_mainVBox);
        ShowAll();
    }

    /// <summary>
    /// Crea el contenedor de botones de acción con mejor diseño.
    /// </summary>
    private VBox CreateActionButtons()
    {
        var actionBox = new VBox(false, 10);

        // Contenedor centrado para los botones
        var centeringBox = new HBox(false, 0);
        centeringBox.PackStart(new Label(""), true, true, 0); // Espaciador izquierdo

        if (Sesion.UsuarioActual != null)
        {
            if (Sesion.UsuarioActual.EsAdmin())
            {
                // Crear un grid para organizar los botones de administrador
                var grid = new Grid();
                grid.RowSpacing = 15;
                grid.ColumnSpacing = 15;
                grid.ColumnHomogeneous = true;

                var adminActions = new MenuAdministrador();

                // Primera fila
                grid.Attach(CreateActionButton("Gestión de\nUsuarios", adminActions.OnGestionUsuarios), 0, 0, 1, 1);
                grid.Attach(CreateActionButton("Gestión de\nVehículos", adminActions.OnGestionVehiculos), 1, 0, 1, 1);
                grid.Attach(CreateActionButton("Gestión de\nRepuestos", adminActions.OnGestionRepuestos), 2, 0, 1, 1);

                // Segunda fila
                grid.Attach(CreateActionButton("Servicios", adminActions.OnGestionServicios), 0, 1, 1, 1);
                grid.Attach(CreateActionButton("Facturas", adminActions.OnGestionFacturas), 1, 1, 1, 1);
                grid.Attach(CreateActionButton("Reportes", adminActions.OnGenerarReportes), 2, 1, 1, 1);

                // Tercera fila
                grid.Attach(CreateActionButton("Bitácora", adminActions.OnMostrarBitacora), 1, 2, 1, 1);

                centeringBox.PackStart(grid, false, false, 0);
            }
            else
            {
                // Grid para botones de usuario normal
                var grid = new Grid();
                grid.RowSpacing = 15;
                grid.ColumnSpacing = 15;
                grid.ColumnHomogeneous = true;

                var userActions = new MenuUsuario();

                // Descomentar y modificar según necesites
                /*
                grid.Attach(CreateActionButton("Mis\nVehículos", userActions.OnMisVehiculos), 0, 0, 1, 1);
                grid.Attach(CreateActionButton("Mis\nServicios", userActions.OnMisServicios), 1, 0, 1, 1);
                grid.Attach(CreateActionButton("Mis\nFacturas", userActions.OnMisFacturas), 0, 1, 1, 1);
                */

                centeringBox.PackStart(grid, false, false, 0);
            }
        }

        centeringBox.PackStart(new Label(""), true, true, 0); // Espaciador derecho
        actionBox.PackStart(centeringBox, true, true, 0);

        // Agregar botones de sesión en la parte inferior
        var sessionButtonsBox = new HBox(true, 15);
        sessionButtonsBox.PackStart(new Label(""), true, false, 0); // Espaciador

        var logoutButton = new Button("Cerrar Sesión");
        logoutButton.Clicked += OnCerrarSesionClicked; // Usar el nuevo manejador
        logoutButton.AddCssClass("boton-sesion");
        logoutButton.WidthRequest = 120;

        var exitButton = new Button("Salir");
        exitButton.Clicked += OnSalirClicked; // Usar el nuevo manejador
        exitButton.AddCssClass("boton-sesion");
        exitButton.WidthRequest = 120;

        sessionButtonsBox.PackStart(logoutButton, false, false, 0);
        sessionButtonsBox.PackStart(exitButton, false, false, 0);
        sessionButtonsBox.PackStart(new Label(""), true, false, 0); // Espaciador

        actionBox.PackStart(sessionButtonsBox, false, false, 20);

        return actionBox;
    }

    /// <summary>
    /// Crea un botón de acción con una etiqueta y un manejador de eventos.
    /// </summary>
    /// <param name="label">Etiqueta del botón.</param>
    /// <param name="handler">Manejador de eventos para el botón.</param>
    private Button CreateActionButton(string label, EventHandler handler)
    {
        var button = new Button(label)
        {
            WidthRequest = 150, // Más ancho
            HeightRequest = 80 // Más alto para acomodar texto multilinea
        };

        button.Clicked += handler;

        // Diferente estilo según el rol
        if (Sesion.UsuarioActual.EsAdmin())
        {
            button.AddCssClass("boton-admin");
        }
        else
        {
            button.AddCssClass("boton-usuario");
        }

        return button;
    }
    
    /// <summary>
    /// Manejador de evento para el botón "Cerrar Sesión" con confirmación.
    /// </summary>
    private void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        // Crear un cuadro de diálogo de confirmación
        var dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Question,
            ButtonsType.YesNo,
            "¿Está seguro que desea cerrar sesión?")
        {
            Title = "Confirmar cierre de sesión"
        };
    
        ResponseType response = (ResponseType)dialog.Run();
        dialog.Destroy();
    
        if (response == ResponseType.Yes)
        {
            ReiniciarAplicacion();
        }
    }

    /// <summary>
    /// Manejador de evento para el botón "Salir" con confirmación.
    /// </summary>
    private void OnSalirClicked(object sender, EventArgs e)
    {
        // Crear un cuadro de diálogo de confirmación
        var dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Question,
            ButtonsType.YesNo,
            "¿Está seguro que desea salir de la aplicación?")
        {
            Title = "Confirmar salida"
        };
    
        ResponseType response = (ResponseType)dialog.Run();
        dialog.Destroy();
    
        if (response == ResponseType.Yes)
        {
            Application.Quit();
        }
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
            "../../../src/UI/Assets/Styles/style.css");
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