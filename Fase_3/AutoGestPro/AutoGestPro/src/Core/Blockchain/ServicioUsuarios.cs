using AutoGestPro.Core.Models;
using Newtonsoft.Json;

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
    public Usuario RegistrarUsuario(int id, string nombres, string apellidos, string correo, int edad, string contrasenia)
    {
        // Tabla de verdad - Validaciones: id y correo únicos
        /*
         *  clienteTrue | clienteCorreoTrue | Resultado
         *  -------------------------------------------
         *  null        | null              | true   -> Se puede agregar
         *  null        | !null             | false  -> No se puede agregar
         *  !null       | null              | false  -> No se puede agregar
         *  !null       | !null             | false  -> No se puede agregar
         *  -------------------------------------------
         */
        // Verifica si ya existe un usuario con ese ID
        if (_usuariosPorCorreo.Values.Any(u => u.Id == id))
        {
            Console.WriteLine("Ya existe un usuario con ese ID");
            return null;
        }
        
        // Verifica si ya existe un usuario con ese correo
        if (_usuariosPorCorreo.ContainsKey(correo))
        {
            Console.WriteLine("Ya existe un usuario con ese correo");
            return null;
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
    
    // Busca un usuario por su ID
    public Usuario BuscarUsuarioPorId(int id)
    {
        // Busca en el diccionario de usuarios por correo
        return _usuariosPorCorreo.Values.FirstOrDefault(u => u.Id == id);
    }

    // Verifica la integridad de la blockchain
    public bool VerificarIntegridad()
    {
        return _blockchain.EsValida();
    }
    
    // Obtiene todos los usuarios registrados
    public List<Usuario> ObtenerTodos()
    {
        // Devuelve una lista con todos los usuarios en el diccionario
        return _usuariosPorCorreo.Values.ToList();
    }

    // Propiedad que devuelve el número de usuarios registrados
    public int Count => _usuariosPorCorreo.Count;
}