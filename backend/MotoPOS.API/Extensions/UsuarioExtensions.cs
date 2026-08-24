using MotoPOS.API.DTOs.Usuarios;
using MotoPOS.API.Entities.Security;

namespace MotoPOS.API.Extensions;

public static class UsuarioExtensions
{
    public static UsuarioDTO ToDto(this Usuario usuario)
    {
        return new UsuarioDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            UsuarioLogin = usuario.UsuarioLogin,
            RolId = usuario.RolId,
            Rol = usuario.Rol.Nombre
        };
    }

    public static IEnumerable<UsuarioDTO> ToDto(
        this IEnumerable<Usuario> usuarios)
    {
        return usuarios.Select(u => u.ToDto());
    }
}