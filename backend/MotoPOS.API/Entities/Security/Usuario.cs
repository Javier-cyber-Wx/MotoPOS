using MotoPOS.API.Entities.Base;
using MotoPOS.API.Entities.Compras;
using MotoPOS.API.Entities.Inventario;
using MotoPOS.API.Entities.Ventas;
namespace MotoPOS.API.Entities.Security

{
    public class Usuario : AuditableEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string UsuarioLogin { get; set; } = string.Empty;
        public string ContrasenaHash { get; set; } = string.Empty;

        //relación con Rol
        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
    }
}
