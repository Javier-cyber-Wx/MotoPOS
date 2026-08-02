using MotoPOS.API.Entities.Base;
using MotoPOS.API.Entities.Ventas;


namespace MotoPOS.API.Entities.Personas;

public class Cliente : AuditableEntity
{
    public string Nit { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }
    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}