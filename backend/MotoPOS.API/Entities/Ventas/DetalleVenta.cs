using MotoPOS.API.Entities.Base;
using MotoPOS.API.Entities.Catalogos;

namespace MotoPOS.API.Entities.Ventas;

public class DetalleVenta : BaseEntity
{
    public int VentaId { get; set; }

    public Venta Venta { get; set; } = null!;

    public int ProductoId { get; set; }

    public Producto Producto { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }
}