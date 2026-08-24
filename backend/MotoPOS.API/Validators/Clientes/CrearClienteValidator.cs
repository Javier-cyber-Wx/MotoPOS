using FluentValidation;
using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Clientes;

namespace MotoPOS.API.Validators.Clientes;

public class CrearClienteValidator : AbstractValidator<CrearClienteDto>
{
    public CrearClienteValidator()
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
            .WithMessage(ValidationMessages.Clientes.DireccionMaximo);
        RuleFor(x => x.Telefono)
            .MaximumLength(20)
            .WithMessage(ValidationMessages.Clientes.TelefonoMaximo);
        RuleFor(x => x.Correo)
            .MaximumLength(100)
            .WithMessage(ValidationMessages.Clientes.CorreoMaximo)
            .EmailAddress()
            .WithMessage(ValidationMessages.Clientes.CorreoFormato);
    }
}