using MotoPOS.API.Entities.Security;

namespace MotoPOS.API.Interfaces.Auth;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<RefreshToken?> GetActiveByUsuarioIdAsync(int usuarioId);
    Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
    Task RevokeAsync(RefreshToken refreshToken, string? motivo = null);
    Task RevokeAllByUsuarioIdAsync(int usuarioId, string? motivo = null);
}