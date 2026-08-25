namespace MotoPOS.API.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string UsuarioLogin { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;

    }
}