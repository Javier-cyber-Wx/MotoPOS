using MotoPOS.API.Entities.Base;    
using MotoPOS.API.Entities.Compras;

namespace MotoPOS.API.Entities.Personas
{
    public class Proveedor : AuditableEntity
    {
        public string Nit { get; set; } = string.Empty;

        public string NombreEmpresa { get; set; } = string.Empty;

        public string? NombreContacto { get; set; }

        public string? Direccion { get; set; }

        public string? Telefono { get; set; }

        public string? Correo { get; set; }
        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}
