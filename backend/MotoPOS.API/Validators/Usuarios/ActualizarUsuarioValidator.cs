using FluentValidation;
using MotoPOS.API.DTOs.Usuarios;
using MotoPOS.API.Constants;
namespace MotoPOS.API.Validators
{
    public class ActualizarUsuarioValidator : AbstractValidator<ActualizarUsuarioDTO>
    {
        public ActualizarUsuarioValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage(ValidationMessages.Usuarios.NombreRequerido)
                .MaximumLength(100).WithMessage(ValidationMessages.Usuarios.NombreMaximo);
            RuleFor(x => x.UsuarioLogin)
                .NotEmpty().WithMessage(ValidationMessages.Usuarios.UsuarioLoginRequerido)
                .MaximumLength(50).WithMessage(ValidationMessages.Usuarios.UsuarioLoginMaximo);
            RuleFor(x => x.RolId)
                .GreaterThan(0).WithMessage(ValidationMessages.Usuarios.RolIdRequerido);
        }
    }
}