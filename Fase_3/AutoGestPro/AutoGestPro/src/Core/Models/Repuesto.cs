namespace AutoGestPro.Core.Models;

/// <summary>
/// Representa un repuesto.
/// </summary>
public class Repuesto
{
    private int _id; // Identificador único del repuesto
    private string _repuesto; // Nombre del repuesto
    private string _detalles; // Detalles del repuesto
    private decimal _costo; // Costo del repuesto

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Repuesto"/>.
    /// </summary>
    /// <param name="id">El identificador del repuesto.</param>
    /// <param name="repuesto">El nombre del repuesto.</param>
    /// <param name="detalles">Los detalles del repuesto.</param>
    /// <param name="costo">El costo del repuesto.</param>
    public Repuesto(int id, string repuesto, string detalles, decimal costo)
    {
        _id = id;
        _repuesto = repuesto ?? throw new ArgumentNullException(nameof(repuesto));
        _detalles = detalles ?? throw new ArgumentNullException(nameof(detalles));
        _costo = costo;
    }

    /// <summary>
    /// Obtiene o establece el identificador del repuesto.
    /// </summary>
    public int Id
    {
        get => _id;
        set => _id = value;
    }

    /// <summary>
    /// Obtiene o establece el nombre del repuesto.
    /// </summary>
    public string Repuesto1
    {
        get => _repuesto;
        set => _repuesto = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Obtiene o establece los detalles del repuesto.
    /// </summary>
    public string Detalles
    {
        get => _detalles;
        set => _detalles = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Obtiene o establece el costo del repuesto.
    /// </summary>
    public decimal Costo
    {
        get => _costo;
        set => _costo = value;
    }
}