namespace AutoGestPro.Core.Models;

/// <summary>
/// Representa una factura.
/// </summary>
public class Factura
{
    private int _id;
    private int _idUsuario;
    private int _idServicio;
    private decimal _total;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Factura"/>.
    /// </summary>
    /// <param name="id">El identificador de la factura.</param>
    /// <param name="idUsuario">El identificador del usuario.</param>
    /// <param name="idServicio">El identificador del servicio.</param>
    /// <param name="total">El total de la factura.</param>
    public Factura(int id, int idUsuario, int idServicio, decimal total)
    {
        _id = id;
        _idUsuario = idUsuario;
        _idServicio = idServicio;
        _total = total;
    }

    /// <summary>
    /// Obtiene o establece el identificador de la factura.
    /// </summary>
    public int Id
    {
        get => _id;
        set => _id = value;
    }

    /// <summary>
    /// Obtiene o establece el identificador del usuario.
    /// </summary>
    public int IdUsuario
    {
        get => _idUsuario;
        set => _idUsuario = value;
    }

    /// <summary>
    /// Obtiene o establece el identificador del servicio.
    /// </summary>
    public int IdServicio
    {
        get => _idServicio;
        set => _idServicio = value;
    }

    /// <summary>
    /// Obtiene o establece el total de la factura.
    /// </summary>
    public decimal Total
    {
        get => _total;
        set => _total = value;
    }
}