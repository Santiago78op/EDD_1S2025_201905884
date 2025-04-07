using AutoGestPro.UI.Views.Admin;

namespace AutoGestPro.UI.Views.Shared;

using System;
using Gtk;

/// <summary>
/// Vista principal de menú con bienvenida al usuario.
/// </summary>
public class MenuPrincipal : VBox
{
    private readonly Label _labelBienvenida;

    /// <summary>
    /// Constructor del menú principal.
    /// </summary>
    public MenuPrincipal() : base(false, 10)
    {
        Margin = 20;

        _labelBienvenida = new Label
        {
            Text = $"Bienvenido, {Sesion.UsuarioActual?.Nombres} {Sesion.UsuarioActual?.Apellidos}",
            Justify = Justification.Center,
            Wrap = true
        };
        _labelBienvenida.AddCssClass("header-label");

        PackStart(_labelBienvenida, false, false, 0);

        var descripcion = new Label("Desde este menú puedes acceder a todas las funcionalidades del sistema dependiendo de tu rol.")
        {
            Justify = Justification.Center,
            Wrap = true,
            MarginTop = 10
        };

        PackStart(descripcion, false, false, 0);
    }
}