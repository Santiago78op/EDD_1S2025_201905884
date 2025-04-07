using AutoGestPro.Core.Models;

namespace AutoGestPro.Core.Blockchain;

/// <summary>
/// Servicio para gestionar usuarios en la blockchain
/// </summary>
public class ServicioUsuarios
{
    private BlockchainUsuarios _blockchain; // Cadena de bloques para usuarios
    private Dictionary<string, Usuario> _usuariosPorCorreo;  // Cache para búsquedas rápidas
    
    public ServicioUsuarios()
    {
        _blockchain = new BlockchainUsuarios();
        _usuariosPorCorreo = new Dictionary<string, Usuario>(StringComparer.OrdinalIgnoreCase);
    }
    
    // Registra un nuevo usuario en el sistema
    public Usuario RegistrarUsuario(Guid id, string nombres, string apellidos, string correo, int edad, string contrasenia)
    {
        // Verifica si ya existe un usuario con ese ID
        if (_usuariosPorCorreo.Values.Any(u => u.Id == id))
        {
            throw new InvalidOperationException("Ya existe un usuario con ese ID");
        }
        
        // Verifica si ya existe un usuario con ese correo
        if (_usuariosPorCorreo.ContainsKey(correo))
        {
            throw new InvalidOperationException("Ya existe un usuario con ese correo electrónico");
        }

        // Crea el nuevo usuario
        Usuario nuevoUsuario = new Usuario(id, nombres, apellidos, correo, edad, contrasenia);
            
        // Lo añade a la blockchain
        _blockchain.AgregarUsuario(nuevoUsuario);
            
        // Lo añade al diccionario para búsquedas rápidas
        _usuariosPorCorreo[correo] = nuevoUsuario;
            
        return nuevoUsuario;
    }
    
    // Autentica un usuario
    public Usuario Autenticar(string correo, string contrasenia)
    {
        if (!_usuariosPorCorreo.TryGetValue(correo, out Usuario usuario))
        {
            return null;  // Usuario no encontrado
        }

        return usuario.VerificarContrasenia(contrasenia) ? usuario : null;
    }

    // Verifica la integridad de la blockchain
    public bool VerificarIntegridad()
    {
        return _blockchain.EsValida();
    }
}