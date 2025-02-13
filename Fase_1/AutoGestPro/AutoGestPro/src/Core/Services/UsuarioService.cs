using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

/**
 * Servicio que se encarga de gestionar los usuarios
 * Se encarga de la autenticación y de la gestion del usuario root
 */
public unsafe class UsuarioService
{
    private readonly Linked_List<Usuario> _usuarios;
        
    public UsuarioService()
    {
        _usuarios = new Linked_List<Usuario>(new Usuario("root@usac.com", "root"));
    }
    
    public bool ValidarCredenciales(string nombreUsuario, string contrasena)
    {
        NodeLinked<Usuario>* current = _usuarios.Head;
        
        while (current != null)
        {
            if (current->_data.NombreUsuario == nombreUsuario && current->_data.Contrasenia == contrasena)
            {
                return true;
            }
            current = current->_next;
        }
        return false;
    }
}