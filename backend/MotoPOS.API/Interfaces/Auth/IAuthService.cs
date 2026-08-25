using MotoPOS.API.DTOs.Auth;
using MotoPOS.API.DTOs.Usuarios;

namespace MotoPOS.API.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO dto);
    }
}