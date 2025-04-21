namespace AutoGestPro.Core.Nodes;

/// <summary>
/// Representa un nodo que conecta un vehículo con un repuesto
/// </summary>
public class NodoVehiculoRepuesto : IEquatable<NodoVehiculoRepuesto>
{
    /// <summary>
    /// Identificador único del vehículo
    /// </summary>
    public string IdVehiculo { get; }

    /// <summary>
    /// Identificador único del repuesto
    /// </summary>
    public string IdRepuesto { get; }

    /// <summary>
    /// Inicializa un nuevo nodo que relaciona un vehículo con un repuesto
    /// </summary>
    /// <param name="idVehiculo">Identificador del vehículo</param>
    /// <param name="idRepuesto">Identificador del repuesto</param>
    /// <exception cref="ArgumentNullException">Se lanza si alguno de los identificadores es nulo</exception>
    public NodoVehiculoRepuesto(string idVehiculo, string idRepuesto)
    {
        IdVehiculo = idVehiculo ?? throw new ArgumentNullException(nameof(idVehiculo));
        IdRepuesto = idRepuesto ?? throw new ArgumentNullException(nameof(idRepuesto));
    }

    /// <summary>
    /// Determina si este nodo es igual a otro objeto
    /// </summary>
    public override bool Equals(object obj) => Equals(obj as NodoVehiculoRepuesto);

    /// <summary>
    /// Determina si este nodo es igual a otro nodo
    /// </summary>
    public bool Equals(NodoVehiculoRepuesto other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return string.Equals(IdVehiculo, other.IdVehiculo, StringComparison.Ordinal) &&
               string.Equals(IdRepuesto, other.IdRepuesto, StringComparison.Ordinal);
    }

    /// <summary>
    /// Genera un código hash único para este nodo
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(IdVehiculo, IdRepuesto);

    /// <summary>
    /// Representación en texto del nodo
    /// </summary>
    public override string ToString() => $"Vehículo: {IdVehiculo}, Repuesto: {IdRepuesto}";
}