using FluentValidation;
using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Clientes;

namespace MotoPOS.API.Validators
{
    public class ActualizarClienteValidator : AbstractValidator<ActualizarClienteDTO>
    {
        public ActualizarClienteValidator()
        {
            RuleFor(x => x.Nit)
                .NotEmpty()
                .WithMessage(ValidationMessages.Clientes.NitRequerido)
                .MaximumLength(20)
                .WithMessage(ValidationMessages.Clientes.NitMaximo);

            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage(ValidationMessages.Clientes.NombreRequerido)
                .MaximumLength(200)
                .WithMessage(ValidationMessages.Clientes.NombreMaximo);

            RuleFor(x => x.Direccion)
                .MaximumLength(300)
                .WithMessage(ValidationMessages.Clientes.DireccionMaximo)
                .When(x => !string.IsNullOrEmpty(x.Direccion));

            RuleFor(x => x.Telefono)
                .MaximumLength(20)
                .WithMessage(ValidationMessages.Clientes.TelefonoMaximo)
                .When(x => !string.IsNullOrEmpty(x.Telefono));

            RuleFor(x => x.Correo)
                .MaximumLength(100)
                .WithMessage(ValidationMessages.Clientes.CorreoMaximo)
                .EmailAddress()
                .WithMessage(ValidationMessages.Clientes.CorreoFormato)
                .When(x => !string.IsNullOrEmpty(x.Correo));
        }
    }
}