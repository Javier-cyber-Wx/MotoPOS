using MotoPOS.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace MotoPOS.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio una excepcion inesperada.");
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                DuplicateException => HttpStatusCode.Conflict,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                StockInsuficienteException => HttpStatusCode.Conflict,
                InvalidOperationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError 
            };
            var message = exception switch
            {
                NotFoundException => exception.Message,
                DuplicateException => exception.Message,
                UnauthorizedException => exception.Message,
                StockInsuficienteException => exception.Message,
                InvalidOperationException => exception.Message,
                _ => "Ocurrio un error inesperado en el servidor. Por favor, intente nuevamente mas tarde."
            };  
            var result = JsonSerializer.Serialize(new { status = (int)code, message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}   