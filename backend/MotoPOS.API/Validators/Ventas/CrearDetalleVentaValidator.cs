using FluentValidation;
using MotoPOS.API.DTOs.Ventas;

namespace MotoPOS.API.Validators.Ventas
{
    public class CrearDetalleVentaValidator : AbstractValidator<CrearDetalleVentaDto>
    {
        public CrearDetalleVentaValidator()
        {
            RuleFor(x => x.ProductoId)
                .GreaterThan(0)
                .WithMessage("El ProductoId debe ser mayor que cero.");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0)
                .WithMessage("La cantidad debe ser mayor que cero.");
        }
    }
}
