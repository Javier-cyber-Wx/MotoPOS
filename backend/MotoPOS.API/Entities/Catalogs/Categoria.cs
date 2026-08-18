using MotoPOS.API.Entities.Base;

namespace MotoPOS.API.Entities.Catalogos;

public class Categoria : AuditableEntity
{
    public string Nombre { get; set; } = string.Empty;
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}