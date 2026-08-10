using FluentValidation;
using MotoPOS.API.DTOs.Productos;

namespace MotoPOS.API.Validators
{
    public class CrearProductoValidator : AbstractValidator<CrearProductoDto>
    {
        public CrearProductoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty()
                .WithMessage("El código del producto es obligatorio.")
                .MaximumLength(50)
                .WithMessage("El código del producto no puede exceder los 50 caracteres.");
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage("El nombre del producto es obligatorio.")
                .MaximumLength(150)
                .WithMessage("El nombre del producto no puede exceder los 150 caracteres.");
            RuleFor(x => x.MarcaId)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El ID de la marca del producto es obligatorio.");
            RuleFor(x => x.CategoriaId)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El ID de la categoría del producto es obligatorio.");
            RuleFor(x => x.PrecioCompra)
                .NotEmpty()
                .WithMessage("El precio de compra del producto es obligatorio.")
                .GreaterThan(0)
                .WithMessage("El precio de compra del producto debe ser un valor positivo.");
            RuleFor(x => x.PrecioVenta)
                .NotEmpty()
                .WithMessage("El precio de venta del producto es obligatorio.")
                .GreaterThan(0)
                .WithMessage("El precio de venta del producto debe ser un valor positivo.");
            RuleFor(x => x.StockMinimo)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .WithMessage("El stock mínimo del producto no debe ser negativo");
        }

    }
}