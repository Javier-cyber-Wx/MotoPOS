using MotoPOS.API.DTOs.Usuarios;

namespace MotoPOS.API.Interfaces.Usuarios;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDTO>> GetAllAsync();
    Task<UsuarioDTO?> GetByIdAsync(int id);
    Task<UsuarioDTO> CreateAsync(CrearUsuarioDTO dto);
    Task UpdateAsync(int id, ActualizarUsuarioDTO dto);
    Task DeleteAsync(int id);
}