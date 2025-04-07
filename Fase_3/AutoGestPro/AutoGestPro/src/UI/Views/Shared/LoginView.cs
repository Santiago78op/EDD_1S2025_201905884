using AutoGestPro.Core.Blockchain;
using Gtk;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Services;
using AutoGestPro.UI.Extensions;

namespace AutoGestPro.UI.Views.Shared
{
    /// <summary>
    /// Vista para el inicio de sesión de usuarios.
    /// </summary>
    public class LoginView : VBox
    {
        private readonly Entry _correoEntry;
        private readonly Entry _claveEntry;
        private readonly Button _loginButton;
        private readonly Label _mensajeLabel;
        
        private readonly ServicioUsuarios _servicio = Estructuras.Clientes;

        /// <summary>
        /// Evento que se dispara cuando el login es exitoso.
        /// </summary>
        public event EventHandler LoginExitoso;

        /// <summary>
        /// Constructor de la vista de login.
        /// </summary>
        public LoginView() : base(false, 10)
        {
            Margin = 50;

            var titulo = new Label("Iniciar Sesión") { Xalign = 0 };
            titulo.AddCssClass("label-titulo");
            PackStart(titulo, false, false, 0);

            _correoEntry = new Entry { PlaceholderText = "Correo electrónico" };
            _correoEntry.AddCssClass("entry");
            _claveEntry = new Entry { PlaceholderText = "Contraseña", Visibility = false };
            _claveEntry.AddCssClass("entry");
            _loginButton = new Button("Ingresar");
            _loginButton.AddCssClass("boton");
            _loginButton.AddCssClass("boton-login");
            _mensajeLabel = new Label();

            _loginButton.Clicked += OnLoginClicked;

            PackStart(_correoEntry, false, false, 0);
            PackStart(_claveEntry, false, false, 0);
            PackStart(_loginButton, false, false, 10);
            PackStart(_mensajeLabel, false, false, 0);
        }

        /// <summary>
        /// Manejador del evento de login.
        /// </summary>
        private void OnLoginClicked(object? sender, EventArgs e)
        {
            try
            {
                string correoLogin = _correoEntry.Text.Trim();
                string claveLogin = _claveEntry.Text.Trim();

                if (string.IsNullOrEmpty(correoLogin) || string.IsNullOrEmpty(claveLogin))
                {
                    MostrarMensaje("Por favor, complete todos los campos.");
                    return;
                }
                
                // Valida si el Usuario es admin y solo actuliza el Usuario Actual y regresa.
                if (correoLogin == "admin@usac.com" && claveLogin == "admint123")
                {
                    Sesion.UsuarioActual = new Usuario(Guid.NewGuid(), "Admin", "Admin", correoLogin, 0, claveLogin);
                    MostrarMensaje("Autenticación exitosa: Administrador");
                   LoginExitoso?.Invoke(this, EventArgs.Empty);
                   return;
                }

                // Verifica autenticación
                Usuario usuarioAutenticado = _servicio.Autenticar(correoLogin, claveLogin);
                if (usuarioAutenticado != null)
                {
                    MostrarMensaje($"Autenticación exitosa: {usuarioAutenticado.Nombres} {usuarioAutenticado.Apellidos}");
                }
                else
                {
                    MostrarMensaje("Autenticación fallida: credenciales incorrectas");
                } 
                
                // Verificar integridad de la blockchain
                bool esValida = _servicio.VerificarIntegridad();
                Console.WriteLine($"Integridad de la blockchain: {(esValida ? "Válida" : "Inválida")}");
                
                Sesion.UsuarioActual = usuarioAutenticado;
                LoginExitoso?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        /// Muestra un mensaje de error o información.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        private void MostrarMensaje(string mensaje)
        {
            _mensajeLabel.Text = mensaje;
            _mensajeLabel.ModifyFg(StateType.Normal, new Gdk.Color(255, 0, 0));
        }
    }
}