using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MotoPOS.API.Interfaces.Auth;

namespace MotoPOS.API.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    var jti = jwtToken.Id;

                    using var scope = context.RequestServices.CreateScope();
                    var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();

                    var refreshToken = await refreshTokenRepository.GetActiveByUsuarioIdAsync(
                        int.Parse(jwtToken.Subject));

                    if (refreshToken != null && refreshToken.JwtTokenId == jti && !refreshToken.EstaActivo)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsJsonAsync(new 
                        { 
                            status = 401, 
                            message = "Token ha sido revocado. Por favor, inicie sesión nuevamente." 
                        });
                        return;
                    }
                }
                catch
                {
                    // Continuar con el siguiente middleware
                }
            }

            await _next(context);
        }
    }
}