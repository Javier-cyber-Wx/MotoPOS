using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Interfaces.Clientes;
using MotoPOS.API.Interfaces.Productos;
using MotoPOS.API.Repositories.Productos;
using MotoPOS.API.Services.Productos;
using MotoPOS.API.Validators;
using MotoPOS.API.Repositories.Clientes;

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
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias
builder.Services.AddScoped<IProductRepository, ProductoRepository>();
builder.Services.AddScoped<IProductService, ProductoService>();
builder.Services.AddScoped<IClientRepository, ClienteRepository>();

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