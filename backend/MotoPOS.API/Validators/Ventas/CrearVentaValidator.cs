using FluentValidation;
using MotoPOS.API.DTOs.Ventas;

namespace MotoPOS.API.Validators.Ventas
{
    public class CrearVentaValidator : AbstractValidator<CrearVentaDto>
    {
        public CrearVentaValidator()
        {
            RuleFor(x => x.ClienteId)
                .GreaterThan(0)
                .WithMessage("El ClienteId debe ser mayor que cero.");

            RuleFor(x => x.Detalles)
                .NotNull()
                .WithMessage("La venta debe contener detalles.")
                .NotEmpty()
                .WithMessage("La venta debe contener al menos un detalle.");

            RuleForEach(x => x.Detalles)
                .SetValidator(new CrearDetalleVentaValidator());
        }
    }
}
