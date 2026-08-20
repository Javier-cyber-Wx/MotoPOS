using FluentValidation;
using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Proveedores;

namespace MotoPOS.API.Validators
{
    public class ActualizarProveedorValidator : AbstractValidator<ActualizarProveedorDTO>
    {
        public ActualizarProveedorValidator()
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
                .WithMessage(ValidationMessages.Proveedores.NombreContactoMaximo)
                .When(x => !string.IsNullOrEmpty(x.NombreContacto));

            RuleFor(x => x.Direccion)
                .MaximumLength(300)
                .WithMessage(ValidationMessages.Proveedores.DireccionMaximo)
                .When(x => !string.IsNullOrEmpty(x.Direccion));

            RuleFor(x => x.Telefono)
                .MaximumLength(20)
                .WithMessage(ValidationMessages.Proveedores.TelefonoMaximo)
                .When(x => !string.IsNullOrEmpty(x.Telefono));

            RuleFor(x => x.Correo)
                .MaximumLength(100)
                .WithMessage(ValidationMessages.Proveedores.CorreoMaximo)
                .EmailAddress()
                .WithMessage(ValidationMessages.Proveedores.CorreoFormato)
                .When(x => !string.IsNullOrEmpty(x.Correo));
        }
    }
}