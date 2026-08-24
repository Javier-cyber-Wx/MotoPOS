using FluentValidation;
using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Categorias;

namespace MotoPOS.API.Validators.Categorias
{
    public class ActualizarCategoriaValidator : AbstractValidator<ActualizarCategoriaDTO>
    {
        public ActualizarCategoriaValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage(ValidationMessages.Categorias.NombreRequerido)
                .MaximumLength(100)
                .WithMessage(ValidationMessages.Categorias.NombreMaximo);
        }
    }
}