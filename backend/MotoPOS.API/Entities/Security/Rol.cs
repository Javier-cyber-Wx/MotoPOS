using MotoPOS.API.Entities.Base;

namespace MotoPOS.API.Entities.Security;

public class Rol : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    //navegación
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}