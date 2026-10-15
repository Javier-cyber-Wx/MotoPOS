using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.Controllers.Base;
using MotoPOS.API.DTOs.Auth;
using MotoPOS.API.DTOs.Usuarios;
using MotoPOS.API.Interfaces.Auth;

namespace MotoPOS.API.Controllers.Auth
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginDTO dto)
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO dto)
        {
            var response = await _authService.RefreshTokenAsync(dto);
            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(LogoutRequestDTO dto)
        {
            await _authService.LogoutAsync(dto);
            return Ok(new { message = "Sesión cerrada exitosamente" });
        }
    }
}