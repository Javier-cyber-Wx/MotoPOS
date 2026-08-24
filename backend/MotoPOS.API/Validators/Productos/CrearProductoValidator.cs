using FluentValidation;
using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Productos;

namespace MotoPOS.API.Validators.Productos
{
    public class CrearProductoValidator : AbstractValidator<CrearProductoDto>
    {
        public CrearProductoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty()
                .WithMessage(ValidationMessages.Productos.CodigoRequerido)
                .MaximumLength(50)
                .WithMessage(ValidationMessages.Productos.CodigoMaximo);
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage(ValidationMessages.Productos.NombreRequerido)
                .MaximumLength(150)
                .WithMessage(ValidationMessages.Productos.NombreMaximo);
            RuleFor(x => x.MarcaId)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage(ValidationMessages.Productos.MarcaIdRequerido);
            RuleFor(x => x.CategoriaId)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage(ValidationMessages.Productos.CategoriaIdRequerido);
            RuleFor(x => x.PrecioCompra)
                .NotEmpty()
                .WithMessage(ValidationMessages.Productos.PrecioCompraRequerido)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.Productos.PrecioCompraPositivo);
            RuleFor(x => x.PrecioVenta)
                .NotEmpty()
                .WithMessage(ValidationMessages.Productos.PrecioVentaRequerido)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.Productos.PrecioVentaPositivo);
            RuleFor(x => x.StockMinimo)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.Productos.StockMinimoNoNegativo);
        }

    }
}