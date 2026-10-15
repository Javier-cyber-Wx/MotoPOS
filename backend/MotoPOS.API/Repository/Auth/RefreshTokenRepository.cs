using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Security;
using MotoPOS.API.Interfaces.Auth;

namespace MotoPOS.API.Repository.Auth;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly MotoPOSDbContext _context;

    public RefreshTokenRepository(MotoPOSDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.Usuario)
            .ThenInclude(u => u.Rol)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task<RefreshToken?> GetActiveByUsuarioIdAsync(int usuarioId)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.Usuario)
            .ThenInclude(u => u.Rol)
            .Where(rt => rt.UsuarioId == usuarioId
                && rt.RevocadoEn == null
                && rt.ExpiraEn > DateTime.UtcNow)
            .FirstOrDefaultAsync();
    }

    public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task RevokeAsync(RefreshToken refreshToken, string? motivo = null)
    {
        refreshToken.RevocadoEn = DateTime.UtcNow;
        refreshToken.MotivoRevocacion = motivo;
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task RevokeAllByUsuarioIdAsync(int usuarioId, string? motivo = null)
    {
        var activeTokens = await _context.RefreshTokens
            .Where(rt => rt.UsuarioId == usuarioId
                && rt.RevocadoEn == null
                && rt.ExpiraEn > DateTime.UtcNow)
            .ToListAsync();

        foreach (var token in activeTokens)
        {
            token.RevocadoEn = DateTime.UtcNow;
            token.MotivoRevocacion = motivo;
        }

        await _context.SaveChangesAsync();
    }

}