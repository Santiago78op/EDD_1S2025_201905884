using System.Text;
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
    
    /// <summary>
    /// Genera una representación del grafo en formato DOT para visualización con Graphviz.
    /// </summary>
    /// <returns>String en formato DOT</returns>
    public string GenerarDotUsuarios()
    {
        StringBuilder dot = new StringBuilder();
        dot.AppendLine("digraph Blockchain {");
        dot.AppendLine("    node [shape=record];");
        dot.AppendLine("    graph [rankdir=LR];");
        dot.AppendLine("    subgraph cluster_0 {");
        dot.AppendLine("        label=\"Blockchain de Usuarios\";");
    
        if (_blockchain.Cadena == null || _blockchain.Cadena.Count == 0)
        {
            dot.AppendLine("        empty [label=\"Cadena vacía\"];");
        }
        else
        {
            for (int i = 0; i < _blockchain.Cadena.Count; i++)
            {
                Bloque bloque = _blockchain.Cadena[i];
                string hashDisplay = bloque.Hash.Length >= 8 ? bloque.Hash.Substring(0, 8) + "..." : bloque.Hash;
                string prevHashDisplay = bloque.HashPrevio.Length >= 8 ? bloque.HashPrevio.Substring(0, 8) + "..." : bloque.HashPrevio;
                
                // Convierte el bloqueDatos a un formato legible para Graphviz
                string bloqueDatos = JsonConvert.SerializeObject(bloque.Datos);
                // Eliminar las comillas dobles, las llaves y \ del JSON
                bloqueDatos = bloqueDatos.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("\\", "");
                 
                string nodeLabel = $"\"Bloque {bloque.Indice}\\nTimeStamp: {bloque.Timestamp}\\nData: {bloqueDatos}\\nHash: {hashDisplay}\\nPrevHash: {prevHashDisplay}\\nNonce: {bloque.Nonce}\"";
                dot.AppendLine($"        block{bloque.Indice} [label={nodeLabel}];");
            
                // Añadir la conexión al siguiente bloque si no es el último
                if (i < _blockchain.Cadena.Count - 1)
                {
                    dot.AppendLine($"        block{bloque.Indice} -> block{_blockchain.Cadena[i + 1].Indice};");
                }
            }
        }
    
        dot.AppendLine("    }");
        dot.AppendLine("}");
        return dot.ToString();
    }
}