using MotoPOS.API.Entities.Security;

namespace MotoPOS.API.Interfaces.Auth
{
    public interface IJwtService
    {
        string GenerateAccessToken(Usuario usuario);
        string GenerateRefreshToken();
        string GetJwtTokenId(string token);
    }
}