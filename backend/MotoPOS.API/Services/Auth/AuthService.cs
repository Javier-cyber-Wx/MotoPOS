using MotoPOS.API.DTOs.Auth;
using MotoPOS.API.DTOs.Usuarios;
using MotoPOS.API.Entities.Security;
using MotoPOS.API.Interfaces.Auth;
using MotoPOS.API.Interfaces.Usuarios;
using MotoPOS.API.Exceptions;

namespace MotoPOS.API.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(
            IUsuarioRepository usuarioRepository, 
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
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

            // Revocar refresh tokens anteriores del usuario (una sesión por usuario)
            await _refreshTokenRepository.RevokeAllByUsuarioIdAsync(
                usuario.Id, 
                "Nuevo login iniciado");

            var accessToken = _jwtService.GenerateAccessToken(usuario);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var jwtTokenId = _jwtService.GetJwtTokenId(accessToken);

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiraEn = DateTime.UtcNow.AddDays(7), // 7 días
                CreadoEn = DateTime.UtcNow,
                UsuarioId = usuario.Id,
                JwtTokenId = jwtTokenId
            };

            await _refreshTokenRepository.CreateAsync(refreshTokenEntity);

            return new LoginResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(30), // 30 minutos
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7), // 7 días
                UsuarioId = usuario.Id,
                UsuarioLogin = usuario.UsuarioLogin,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol.Nombre
            };
        }

        public async Task<RefreshTokenResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO dto)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);

            if (refreshToken == null)
            {
                throw new UnauthorizedException("Refresh token inválido.");
            }

            if (!refreshToken.EstaActivo)
            {
                throw new UnauthorizedException("Refresh token expirado o revocado.");
            }

            var usuario = refreshToken.Usuario;

            if (!usuario.Activo)
            {
                throw new UnauthorizedException("Usuario inactivo.");
            }

            // Validar que el access token pertenezca a este refresh token
            var currentJwtTokenId = _jwtService.GetJwtTokenId(dto.AccessToken);
            if (refreshToken.JwtTokenId != currentJwtTokenId)
            {
                throw new UnauthorizedException("Los tokens no coinciden.");
            }

            // Revocar el refresh token actual
            await _refreshTokenRepository.RevokeAsync(refreshToken, "Token utilizado para refresh");

            // Generar nuevos tokens
            var newAccessToken = _jwtService.GenerateAccessToken(usuario);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            var newJwtTokenId = _jwtService.GetJwtTokenId(newAccessToken);

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                ExpiraEn = DateTime.UtcNow.AddDays(7),
                CreadoEn = DateTime.UtcNow,
                UsuarioId = usuario.Id,
                JwtTokenId = newJwtTokenId
            };

            await _refreshTokenRepository.CreateAsync(newRefreshTokenEntity);

            return new RefreshTokenResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(30),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7)
            };
        }

        public async Task LogoutAsync(LogoutRequestDTO dto)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);

            if (refreshToken != null && refreshToken.EstaActivo)
            {
                await _refreshTokenRepository.RevokeAsync(refreshToken, "Logout realizado por usuario");
            }
        }
    }
}
