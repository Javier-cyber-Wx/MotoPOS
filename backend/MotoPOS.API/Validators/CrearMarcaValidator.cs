using FluentValidation;
using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Marcas;

namespace MotoPOS.API.Validators;

public class CrearMarcaValidator : AbstractValidator<CrearMarcaDto>
{
    public CrearMarcaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage(ValidationMessages.Marcas.NombreRequerido)
            .MaximumLength(100)
            .WithMessage(ValidationMessages.Marcas.NombreMaximo);
    }
}