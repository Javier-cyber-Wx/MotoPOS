namespace MotoPOS.API.DTOs.Usuarios
{
    public class CrearUsuarioDTO
    {
        public string Nombre { get; set; } = null!;
        public string UsuarioLogin { get; set; } = null!;    
        public string Contrasena { get; set; } = null!;
        public int RolId { get; set; }
    }
}   