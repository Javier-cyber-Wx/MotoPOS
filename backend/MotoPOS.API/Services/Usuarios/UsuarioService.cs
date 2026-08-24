using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Usuarios;
using MotoPOS.API.Entities.Security;
using MotoPOS.API.Exceptions;
using MotoPOS.API.Extensions;
using MotoPOS.API.Interfaces.Usuarios;

namespace MotoPOS.API.Services.Usuarios;
public class UsuarioService : BaseService, IUsuarioService
{
    private readonly IUsuarioRepository _repository;
    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
    {
        var usuarios = await _repository.GetAllAsync();
        return usuarios.ToDto();
    }
    public async Task<UsuarioDTO?> GetByIdAsync(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario == null)
        {
            return null;
        }
        return usuario.ToDto();
    }
    public async Task<UsuarioDTO> CreateAsync(CrearUsuarioDTO dto)
    {
        var usuarioExistente = await _repository.ExistsByUsuarioLoginAsync(dto.UsuarioLogin);
        ValidateDuplicate(usuarioExistente, ErrorMessages.Usuarios.UsuarioLoginDuplicado);
        var rolExiste = await _repository.RolExistsAsync(dto.RolId);
        ValidateExists(rolExiste, ErrorMessages.Usuarios.RolNoExiste);
        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            UsuarioLogin = dto.UsuarioLogin,
            ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
            RolId = dto.RolId,
            Activo = true
        };
        usuario = await _repository.CreateAsync(usuario);
        return await GetByIdAsync(usuario.Id)
            ?? throw new NotFoundException(ErrorMessages.Usuarios.UsuarioNoRecuperado);
    }
    public async Task UpdateAsync(int id, ActualizarUsuarioDTO dto)
    {
        var usuario = await _repository.GetByIdAsync(id);
        ValidateEntityExists(usuario, ErrorMessages.Usuarios.UsuarioNoEncontrado);
        var usuarioExistente = await _repository.ExistsByUsuarioLoginExceptIdAsync(dto.UsuarioLogin,id);
        ValidateDuplicate(usuarioExistente,ErrorMessages.Usuarios.UsuarioLoginDuplicado);
        var rolExiste = await _repository.RolExistsAsync(dto.RolId);
        ValidateExists(rolExiste, ErrorMessages.Usuarios.RolNoExiste);
        usuario!.Nombre = dto.Nombre;
        usuario.UsuarioLogin = dto.UsuarioLogin;
        usuario.RolId = dto.RolId;
        usuario.Activo = dto.Activo;
        await _repository.UpdateAsync(usuario);
    }
    public async Task DeleteAsync(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        ValidateEntityExists(usuario, ErrorMessages.Usuarios.UsuarioNoEncontrado);
        await _repository.DeleteAsync(usuario!);
    }
}