using AutoGestPro.Core.Models;

namespace AutoGestPro.Core.Services;

/// <summary>
/// Clase que mantiene la información de la sesión actual del usuario.
/// </summary>
public static class Sesion
{
    /// <summary>
    /// Usuario autenticado en la sesión actual.
    /// </summary>
    public static Usuario? UsuarioActual { get; set; }
    
    /// <summary>
    /// Verifica si el usuario tiene permisos de administrador.
    /// </summary>
    /// <returns>True si el usuario es administrador, false en caso contrario.</returns>
    public static bool EsAdministrador => UsuarioActual?.EsAdmin() ?? false;
    
    /// <summary>
    /// Verifica si un usuario a iniciado sesión.
    /// </summary>
    /// <returns>True si hay un usuario autenticado, false en caso contrario.</returns>
    public static bool EstaAutenticado => UsuarioActual != null;
    
    /// <summary>
    /// Cierra la sesión actual, limpiando la información del usuario.
    /// </summary>
    public static void CerrarSesion()
    {
        UsuarioActual = null;
    }
}