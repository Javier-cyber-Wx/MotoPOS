using MotoPOS.API.Entities.Base;
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Entities.Security;

namespace MotoPOS.API.Entities.Ventas;

public class Venta : AuditableEntity
{
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public decimal Total { get; set; }

    public int ClienteId { get; set; }

    public Cliente Cliente { get; set; } = null!;

    public int UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;

    public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}