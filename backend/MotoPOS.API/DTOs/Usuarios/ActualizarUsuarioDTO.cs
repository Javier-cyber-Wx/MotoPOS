namespace MotoPOS.API.DTOs.Usuarios
{
    public class ActualizarUsuarioDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string UsuarioLogin { get; set; } = string.Empty;
        public int RolId { get; set; }  
        public bool Activo { get; set; }
    }
}
