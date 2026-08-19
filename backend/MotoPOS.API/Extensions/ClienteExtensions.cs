using MotoPOS.API.DTOs.Clientes;
using MotoPOS.API.Entities.Personas;

namespace MotoPOS.API.Extensions;

public static class ClienteExtensions
{
    public static ClienteDto ToDto(this Cliente cliente)
    {
        return new ClienteDto
        {
            Id = cliente.Id,
            Nit = cliente.Nit,
            Nombre = cliente.Nombre,
            Direccion = cliente.Direccion,
            Telefono = cliente.Telefono,
            Correo = cliente.Correo,
            Activo = cliente.Activo
        };
    }

    public static IEnumerable<ClienteDto> ToDto(this IEnumerable<Cliente> clientes)
    {
        return clientes.Select(c => c.ToDto());
    }
}