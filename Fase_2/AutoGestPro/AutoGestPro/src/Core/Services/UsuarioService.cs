using AutoGestPro.Core.Global;
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
    
    public bool ValidarCredencialesUsuario(string nombreUsuario, string contrasena)
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
    
    public bool ValidarCredencialesCliente(string correoUsuario, string contrasena)
    {
        // 📌 Recorre la lista de clientes de la estructura global
        NodeLinked? current = Estructuras.Clientes.Head;
        
        while (current != null)
        {
            Cliente cliente = (Cliente)current.Data;
            if (cliente.Correo == correoUsuario && cliente.Contrasenia == contrasena)
            {
                return true;
            }
            current = current.Next;
        }
        
        return false;
    }
}