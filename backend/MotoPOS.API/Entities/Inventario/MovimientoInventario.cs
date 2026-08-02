using MotoPOS.API.Entities.Base;    
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Entities.Security;
using MotoPOS.API.Enums;
namespace MotoPOS.API.Entities.Inventario
{
    public class MovimientoInventario : BaseEntity
    {
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public int ProductoId { get; set; }

        public Producto Producto { get; set; } = null!;

        public int Cantidad { get; set; }

        public TipoMovimientoInventario TipoMovimiento { get; set; }
        public string Referencia { get; set; } = string.Empty;

        public int ReferenciaId { get; set; }

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; } = null!;

        public string? Observaciones { get; set; }    }
}
