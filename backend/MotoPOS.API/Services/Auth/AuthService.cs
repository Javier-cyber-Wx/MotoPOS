using MotoPOS.API.DTOs.Auth;
using MotoPOS.API.DTOs.Usuarios;
using MotoPOS.API.Interfaces.Auth;
using MotoPOS.API.Interfaces.Usuarios;
namespace MotoPOS.API.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService;
        public AuthService(IUsuarioRepository usuarioRepository, IJwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }
        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
        {
            var usuario = await _usuarioRepository
                .GetByUsuarioLoginAsync(dto.UsuarioLogin);

            if (usuario == null)
            {
                throw new UnauthorizedAccessException(
                    "Usuario o contraseña incorrectos.");
            }

            if (!usuario.Activo)
            {
                throw new UnauthorizedAccessException(
                    "El usuario se encuentra inactivo.");
            }

            var passwordValida = BCrypt.Net.BCrypt.Verify(
                dto.Contrasena,
                usuario.ContrasenaHash);

            if (!passwordValida)
            {
                throw new UnauthorizedAccessException(
                    "Usuario o contraseña incorrectos.");
            }

            var token = _jwtService.GenerateToken(usuario);

            return new LoginResponseDTO
            {
                Token = token,
                UsuarioId = usuario.Id,
                UsuarioLogin = usuario.UsuarioLogin,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol.Nombre
            };
        }
    }
}
