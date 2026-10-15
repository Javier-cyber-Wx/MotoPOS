using MotoPOS.API.DTOs.Auth;
using MotoPOS.API.DTOs.Usuarios;

namespace MotoPOS.API.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO dto);
        Task<RefreshTokenResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO dto);
        Task LogoutAsync(LogoutRequestDTO dto);
    }
}