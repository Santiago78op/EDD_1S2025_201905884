using AutoGestPro.Core.Blockchain;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Global;

/// <summary>
/// Clase estática para almacenar estructuras globales.
/// </summary>
public static class Estructuras
{
    private static readonly Lazy<ServicioUsuarios> _clientes = new Lazy<ServicioUsuarios>(() => new ServicioUsuarios());
    private static readonly Lazy<DoubleList> _vehiculos = new Lazy<DoubleList>(() => new DoubleList());
    public static ServicioUsuarios Clientes => _clientes.Value;
    public static DoubleList Vehiculos => _vehiculos.Value;
}