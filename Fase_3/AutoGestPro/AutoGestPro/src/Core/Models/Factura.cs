using AutoGestPro.Core.Global;

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
    private DateTime _fechaCreacion; 
    private MetodoPago _metodoPago;
    
    // Generar de forma aleatoria un método de pago
    private Random _random = new Random();

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
        _fechaCreacion = DateTime.Now;
        _metodoPago = MetodoPago.Efectivo; // Valor por defecto
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

    public DateTime FechaCreacion
    {
        get => _fechaCreacion;
        set => _fechaCreacion = value;
    }

    /// <summary>
    /// Obtiene o establece el método de pago.
    /// </summary>
    public MetodoPago MetodoPago
    {
        get => _metodoPago;
        set => _metodoPago = value;
    }
    
    /// <summary>
    /// Establece el método de pago de la factura.
    /// </summary>
    /// <param name="metodoPago">El método de pago a establecer.</param>
    /// <param name="trueMasiva">Indica si es un pago masivo.</param>
    public void EstablecerMetodoPago(MetodoPago metodoPago, bool trueMasiva = false)
    {
        if (!trueMasiva)
        {
            _metodoPago = metodoPago;
        } 
        else
        {
            int randomValue = _random.Next(0, 3);
            _metodoPago = (MetodoPago)randomValue; // Asigna el método de pago basado en el número aleatorio
        }
    }
}