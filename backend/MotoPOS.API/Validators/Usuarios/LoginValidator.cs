using FluentValidation;
using MotoPOS.API.DTOs.Usuarios;
using MotoPOS.API.Constants;
namespace MotoPOS.API.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UsuarioLogin)
                .NotEmpty()
                .WithMessage(ValidationMessages.Usuarios.UsuarioLoginRequerido)
                .MaximumLength(50)
                .WithMessage(ValidationMessages.Usuarios.UsuarioLoginMaximo);
            RuleFor(x => x.Contrasena)
                .NotEmpty()
                .WithMessage(ValidationMessages.Usuarios.ContrasenaRequerida)
                .MaximumLength(100)
                .WithMessage(ValidationMessages.Usuarios.ContrasenaMinima);
        }
    }
}