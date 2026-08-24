using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Security;
using MotoPOS.API.Interfaces.Usuarios;

namespace MotoPOS.API.Repositories.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly MotoPOSDbContext _context;

    public UsuarioRepository(MotoPOSDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .ToListAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> GetByUsuarioLoginAsync(string usuarioLogin)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.UsuarioLogin == usuarioLogin);
    }

    public async Task<bool> ExistsByUsuarioLoginAsync(string usuarioLogin)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.UsuarioLogin == usuarioLogin);
    }

    public async Task<bool> ExistsByUsuarioLoginExceptIdAsync(
        string usuarioLogin,
        int id)
    {
        return await _context.Usuarios
            .AnyAsync(u =>
                u.UsuarioLogin == usuarioLogin &&
                u.Id != id);
    }

    public async Task<bool> RolExistsAsync(int rolId)
    {
        return await _context.Roles
            .AnyAsync(r => r.Id == rolId);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }
}