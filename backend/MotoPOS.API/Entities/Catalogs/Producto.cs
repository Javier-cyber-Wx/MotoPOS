using MotoPOS.API.Entities.Base;
using MotoPOS.API.Entities.Compras;
using MotoPOS.API.Entities.Inventario;
using MotoPOS.API.Entities.Ventas;

namespace MotoPOS.API.Entities.Catalogos;

public class Producto : AuditableEntity
{
    // Identificación
    public string Codigo { get; set; } = string.Empty;

    public string? CodigoBarras { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    // Inventario
    public int Stock { get; set; }

    public int StockMinimo { get; set; } = 5;

    // Costos
    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    // Relaciones
    public int MarcaId { get; set; }

    public Marca Marca { get; set; } = null!;

    public int CategoriaId { get; set; }

    public Categoria Categoria { get; set; } = null!;
    public ICollection<DetalleCompra> DetallesCompra { get; set; } = new List<DetalleCompra>();

    public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();

    public ICollection<MovimientoInventario> MovimientosInventario { get; set; } = new List<MovimientoInventario>();
}