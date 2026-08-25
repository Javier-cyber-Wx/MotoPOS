using MotoPOS.API.Entities.Security;

namespace MotoPOS.API.Interfaces.Auth
{
    public interface IJwtService
    {
        string GenerateToken(Usuario usuario);
    }
}