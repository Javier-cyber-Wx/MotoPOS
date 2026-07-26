using MotoPOS.API.Entities.Base;

namespace MotoPOS.API.Entities.Catalogos;

public class Marca : AuditableEntity
{
    public string Nombre { get; set; } = string.Empty;


    // Navegación

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}