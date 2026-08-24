using MotoPOS.API.Entities.Security;

namespace MotoPOS.API.Interfaces.Usuarios;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario?> GetByUsuarioLoginAsync(string usuarioLogin);
    Task<bool> ExistsByUsuarioLoginAsync(string usuarioLogin);
    Task<bool> ExistsByUsuarioLoginExceptIdAsync(string usuarioLogin, int id);
    Task<bool> RolExistsAsync(int rolId);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
    Task DeleteAsync(Usuario usuario);
}