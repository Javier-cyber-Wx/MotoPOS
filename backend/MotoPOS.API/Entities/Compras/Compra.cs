using MotoPOS.API.Entities.Base;
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Entities.Security;

namespace MotoPOS.API.Entities.Compras
{
    public class Compra : AuditableEntity
    {
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; } = null!;
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public ICollection<DetalleCompra> Detalles { get; set; } = new List<DetalleCompra>();

    }
}
