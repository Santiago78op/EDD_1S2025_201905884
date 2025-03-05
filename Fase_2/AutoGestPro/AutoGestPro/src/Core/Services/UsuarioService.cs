using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

/**
 * Servicio que se encarga de gestionar los usuarios
 * Se encarga de la autenticación y de la gestion del usuario root
 */
public class UsuarioService
{
    // Lista enlazada de usuarios - readonly para evitar modificaciones
    readonly LinkedList _usuarios;
        
    public UsuarioService()
    {   
        _usuarios = new LinkedList();
        _usuarios.Append(new Usuario("root@gmail.com", "root123"));
    }
    
    public bool ValidarCredenciales(string nombreUsuario, string contrasena)
    {
        NodeLinked? current = _usuarios.Head;
        
        while (current != null)
        {
            Usuario usuario = (Usuario)current.Data;
            if (usuario.NombreUsuario == nombreUsuario && usuario.Contrasenia == contrasena)
            {
                return true;
            }
            current = current.Next;
        }
        
        return false;
    }
}