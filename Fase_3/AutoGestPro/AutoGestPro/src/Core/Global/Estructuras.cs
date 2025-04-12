using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Global;

/// <summary>
/// Clase estática para almacenar estructuras globales.
/// </summary>
public static class Estructuras
{
    private static readonly Lazy<ServicioUsuarios> _clientes = new Lazy<ServicioUsuarios>(() => new ServicioUsuarios());
    private static readonly Lazy<DoubleList> _vehiculos = new Lazy<DoubleList>(() => new DoubleList());
    private static readonly Lazy<TreeAvl<Repuesto>> _repuestos = new Lazy<TreeAvl<Repuesto>>(() => new TreeAvl<Repuesto>());
    private static readonly Lazy<TreeBinary> _servicios = new Lazy<TreeBinary>(() => new TreeBinary());
    private static readonly Lazy<MerkleTree> _facturas = new Lazy<MerkleTree>(() => new MerkleTree());
    
    public static ServicioUsuarios Clientes => _clientes.Value;
    public static DoubleList Vehiculos => _vehiculos.Value;
    public static TreeAvl<Repuesto> Repuestos => _repuestos.Value;
    public static TreeBinary Servicios => _servicios.Value;
    public static MerkleTree Facturas => _facturas.Value;
}