using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MotoPOS.API.Configurations;
using MotoPOS.API.Data;
using MotoPOS.API.Interfaces.Auth;
using MotoPOS.API.Interfaces.Categorias;
using MotoPOS.API.Interfaces.Clientes;
using MotoPOS.API.Interfaces.Compras;
using MotoPOS.API.Interfaces.Marcas;
using MotoPOS.API.Interfaces.Movimiento_Inventario;
using MotoPOS.API.Interfaces.Productos;
using MotoPOS.API.Interfaces.Proveedores;
using MotoPOS.API.Interfaces.Usuarios;
using MotoPOS.API.Interfaces.Ventas;
using MotoPOS.API.Middlewares;
using MotoPOS.API.Repositories.Categorias;
using MotoPOS.API.Repositories.Clientes;
using MotoPOS.API.Repositories.Marcas;
using MotoPOS.API.Repositories.Productos;
using MotoPOS.API.Repositories.Proveedores;
using MotoPOS.API.Repositories.Usuarios;
using MotoPOS.API.Repository.Compras;
using MotoPOS.API.Repository.Movimiento_Inventario;
using MotoPOS.API.Repository.Ventas;
using MotoPOS.API.Services.Auth;
using MotoPOS.API.Services.Categorias;
using MotoPOS.API.Services.Clientes;
using MotoPOS.API.Services.Compras;
using MotoPOS.API.Services.Marcas;
using MotoPOS.API.Services.Movimiento_Inventario;
using MotoPOS.API.Services.Productos;
using MotoPOS.API.Services.Proveedores;
using MotoPOS.API.Services.Usuarios;
using MotoPOS.API.Services.Ventas;
using MotoPOS.API.Validators.Categorias;
using MotoPOS.API.Validators.Clientes;
using MotoPOS.API.Validators.Marca;
using MotoPOS.API.Validators.Productos;
using MotoPOS.API.Validators.Proveedores;
using MotoPOS.API.Validators.Ventas;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>() ?? throw new InvalidOperationException(
        "La configuración JwtSettings no fue encontrada.");
if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
{
    throw new InvalidOperationException("La clave secreta JWT no está configurada.");
}
if (Encoding.UTF8.GetByteCount(jwtSettings.SecretKey) < 32)
{
    throw new InvalidOperationException("La clave secreta JWT debe tener al menos 32 bytes.");
}
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };
});
// Base de datos
builder.Services.AddDbContext<MotoPOSDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
});
// Controllers
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CrearProductoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ActualizarProductoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CrearClienteValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ActualizarClienteValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CrearProveedorValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ActualizarProveedorValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CrearMarcaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ActualizarMarcaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CrearCategoriaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ActualizarCategoriaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CrearVentaValidator>();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT"

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
// Inyección de dependencias
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IMovimientoInventarioRepository, MovimientoInventarioRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<ICompraService, CompraService>();

builder.Services.AddScoped<
    IMovimientoInventarioService,
    MovimientoInventarioService>();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();