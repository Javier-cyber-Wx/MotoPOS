namespace MotoPOS.API.DTOs.Auth
{
    public class LogoutRequestDTO
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}