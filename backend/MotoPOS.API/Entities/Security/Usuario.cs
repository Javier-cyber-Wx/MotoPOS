using MotoPOS.API.Entities.Base;
namespace MotoPOS.API.Entities.Security

{
    public class Usuario : AuditableEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string UsuarioLogin { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;

        //relación con Rol
        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;
    }
}
