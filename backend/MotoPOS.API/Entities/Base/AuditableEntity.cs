namespace MotoPOS.API.Entities.Base;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreadoEn { get; set; }
    public DateTime? Modificacion { get; set; }
    public bool Activo { get; set; } = true;
}