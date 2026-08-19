using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Interfaces.Clientes;
using MotoPOS.API.Interfaces.Productos;
using MotoPOS.API.Repositories.Clientes;
using MotoPOS.API.Repositories.Productos;
using MotoPOS.API.Services.Clientes;
using MotoPOS.API.Services.Productos;
using MotoPOS.API.Validators;
using MotoPOS.API.Interfaces.Proveedores;
using MotoPOS.API.Repositories.Proveedores;
using MotoPOS.API.Services.Proveedores;
using MotoPOS.API.Interfaces.Marcas;
using MotoPOS.API.Repositories.Marcas;
using MotoPOS.API.Services.Marcas;
using MotoPOS.API.Interfaces.Categorias;
using MotoPOS.API.Repositories.Categorias;
using MotoPOS.API.Services.Categorias;
using MotoPOS.API.DTOs.Productos;
using MotoPOS.API.DTOs.Clientes;
using MotoPOS.API.DTOs.Proveedores;
using MotoPOS.API.DTOs.Marcas;
using MotoPOS.API.DTOs.Categorias;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddValidatorsFromAssemblyContaining<CrearClienteValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CrearProveedorValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CrearMarcaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CrearCategoriaValidator>();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Inyección de dependencias
builder.Services.AddScoped<IProductRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IClientRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();  
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();  

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();