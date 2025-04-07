using Gtk;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.UI.Extensions;
using AutoGestPro.UI.Views.Admin;

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
            titulo.AddCssClass("form-title");
            PackStart(titulo, false, false, 0);

            _correoEntry = new Entry { PlaceholderText = "Correo electrónico" };
            _claveEntry = new Entry { PlaceholderText = "Contraseña", Visibility = false };
            _loginButton = new Button("Ingresar");
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
        private void OnLoginClicked(object sender, EventArgs e)
        {
            string correo = _correoEntry.Text.Trim();
            string clave = _claveEntry.Text.Trim();

            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(clave))
            {
                MostrarMensaje("Por favor, complete todos los campos.");
                return;
            }

            // Buscar usuario en la lista global (esto puede cambiarse por una BD real)
            var actual = Estructuras.Clientes.Head;
            while (actual != null)
            {
                if (actual.Data is Cliente cliente)
                {
                    if (cliente.Correo == correo && cliente.Clave == clave)
                    {
                        Sesion.UsuarioActual = cliente;
                        LoginExitoso?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                }

                actual = actual.Next;
            }

            MostrarMensaje("Credenciales incorrectas. Inténtelo de nuevo.");
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