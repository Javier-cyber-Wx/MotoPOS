using FluentValidation;
using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Proveedores;

namespace MotoPOS.API.Validators;

public class CrearProveedorValidator : AbstractValidator<CrearProveedorDto>
{
    public CrearProveedorValidator()
    {
        RuleFor(x => x.Nit)
            .NotEmpty()
            .WithMessage(ValidationMessages.Proveedores.NitRequerido)
            .MaximumLength(20)
            .WithMessage(ValidationMessages.Proveedores.NitMaximo);
        RuleFor(x => x.NombreEmpresa)
            .NotEmpty()
            .WithMessage(ValidationMessages.Proveedores.NombreEmpresaRequerido)
            .MaximumLength(200)
            .WithMessage(ValidationMessages.Proveedores.NombreEmpresaMaximo);
        RuleFor(x => x.NombreContacto)
            .MaximumLength(100)
            .WithMessage(ValidationMessages.Proveedores.NombreContactoMaximo);
        RuleFor(x => x.Direccion)
            .MaximumLength(300)
            .WithMessage(ValidationMessages.Proveedores.DireccionMaximo);
        RuleFor(x => x.Telefono)
            .MaximumLength(20)
            .WithMessage(ValidationMessages.Proveedores.TelefonoMaximo);
        RuleFor(x => x.Correo)
            .MaximumLength(100)
            .WithMessage(ValidationMessages.Proveedores.CorreoMaximo)
            .EmailAddress()
            .WithMessage(ValidationMessages.Proveedores.CorreoFormato);
    }
}