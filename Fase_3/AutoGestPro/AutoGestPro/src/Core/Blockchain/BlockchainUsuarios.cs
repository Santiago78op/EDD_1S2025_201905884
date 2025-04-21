using AutoGestPro.Core.Models;
using Newtonsoft.Json;

namespace AutoGestPro.Core.Blockchain;

/// <summary>
/// Implementación de la cadena de blockchain para usuarios
/// </summary>
public class BlockchainUsuarios
{
    private List<Bloque> _cadena; // Cadena de bloques
    private int _dificultad; // Dificultad del minado
    
    // Constructor que inicializa la blockchain con el bloque génesis
    public BlockchainUsuarios(int dificultad = 4)
    {
        _cadena = new List<Bloque>();
        _dificultad = dificultad;
        CrearBloqueGenesis();
    }

    public List<Bloque> Cadena
    {
        get => _cadena;
        set => _cadena = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Dificultad
    {
        get => _dificultad;
        set => _dificultad = value;
    }

    // Crea el primer bloque (génesis)
    private void CrearBloqueGenesis()
    {
        Bloque bloqueGenesis = new Bloque(0, DateTime.Now, "0", "Bloque Génesis");
        bloqueGenesis.MinarBloque(_dificultad);
        _cadena.Add(bloqueGenesis);
    }
    
    // Obtiene el último bloque de la cadena
    public Bloque ObtenerUltimoBloque()
    {
        return _cadena[_cadena.Count - 1];
    }
    
    // Añade un nuevo usuario a la blockchain
    public void AgregarUsuario(Usuario usuario)
    {
        // Serializa el usuario a JSON (excluyendo datos sensibles)
        string datosUsuario = JsonConvert.SerializeObject(usuario);
            
        Bloque bloqueAnterior = ObtenerUltimoBloque();
        Bloque nuevoBloque = new Bloque(
            bloqueAnterior.Indice + 1,
            DateTime.Now,
            bloqueAnterior.Hash,
            datosUsuario
        );
            
        // Mina el bloque antes de añadirlo
        nuevoBloque.MinarBloque(_dificultad);
        _cadena.Add(nuevoBloque);
    }
    
    // Valida la integridad de la cadena
    public bool EsValida()
    {
        for (int i = 1; i < _cadena.Count; i++)
        {
            Bloque bloqueActual = _cadena[i];
            Bloque bloqueAnterior = _cadena[i - 1];

            // Verifica que el hash almacenado es correcto
            if (bloqueActual.Hash != bloqueActual.CalcularHash())
            {
                return false;
            }

            // Verifica que el hash previo coincide con el hash del bloque anterior
            if (bloqueActual.HashPrevio != bloqueAnterior.Hash)
            {
                return false;
            }
        }
        return true;
    }
}