namespace MotoPOS.API.Entities.Base;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreadoEnd { get; set; }
    public DateTime? Modificacion { get; set; }
    public bool Activo { get; set; } = true;
}