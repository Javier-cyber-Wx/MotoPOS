using FluentValidation;
using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Usuarios;

namespace MotoPOS.API.Validators.Usuarios
{
    public class CrearUsuarioValidator : AbstractValidator<CrearUsuarioDTO>
    {
        public CrearUsuarioValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage(ValidationMessages.Usuarios.NombreRequerido)
                .MaximumLength(100).WithMessage(ValidationMessages.Usuarios.NombreMaximo);
            RuleFor(x => x.UsuarioLogin)
                .NotEmpty().WithMessage(ValidationMessages.Usuarios.UsuarioLoginRequerido)
                .MaximumLength(50).WithMessage(ValidationMessages.Usuarios.UsuarioLoginMaximo);
            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage(ValidationMessages.Usuarios.ContrasenaRequerida)
                .MinimumLength(6).WithMessage(ValidationMessages.Usuarios.ContrasenaMinima);
            RuleFor(x => x.RolId)
                .GreaterThan(0).WithMessage(ValidationMessages.Usuarios.RolIdRequerido);
        }
    }
}