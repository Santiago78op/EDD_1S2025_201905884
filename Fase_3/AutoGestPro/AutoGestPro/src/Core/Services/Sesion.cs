using AutoGestPro.Core.Models;

namespace AutoGestPro.Core.Services;

/// <summary>
/// Clase que mantiene la información de la sesión actual del usuario.
/// </summary>
public static class Sesion
{
    private static readonly LogService _logService = new LogService();

    /// <summary>
    /// Usuario autenticado en la sesión actual.
    /// </summary>
    private static Usuario? _usuarioActual;

    /// <summary>
    /// Usuario autenticado en la sesión actual.
    /// </summary>
    public static Usuario? UsuarioActual
    {
        get => _usuarioActual;
        set
        {
            // Si hay un usuario actual diferente al nuevo, registrar su salida
            if (_usuarioActual != null && _usuarioActual != value)
            {
                _logService.RegistrarSalida(_usuarioActual.Correo);
            }

            // Si se está asignando un nuevo usuario, registrar su entrada
            if (value != null && _usuarioActual != value)
            {
                _logService.RegistrarEntrada(value.Correo);
            }

            _usuarioActual = value;
        }
    }
    
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
        if (UsuarioActual != null)
        {
            // Registrar la salida del usuario
            _logService.RegistrarSalida(UsuarioActual.Correo);
            _usuarioActual = null;
        }
    }

    /// <summary>
    /// Obtiene el servicio de logs para operaciones avanzadas.
    /// </summary>
    /// <returns>Instancia del servicio de logs.</returns>
    public static LogService ObtenerServicioLogs()
    {
        return _logService;
    }
}