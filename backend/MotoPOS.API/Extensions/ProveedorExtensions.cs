using MotoPOS.API.DTOs.Proveedores;
using MotoPOS.API.Entities.Personas;

namespace MotoPOS.API.Extensions;

public static class ProveedorExtensions
{
    public static ProveedorDto ToDto(this Proveedor proveedor)
    {
        return new ProveedorDto
        {
            Id = proveedor.Id,
            Nit = proveedor.Nit,
            NombreEmpresa = proveedor.NombreEmpresa,
            NombreContacto = proveedor.NombreContacto,
            Direccion = proveedor.Direccion,
            Telefono = proveedor.Telefono,
            Correo = proveedor.Correo,
            Activo = proveedor.Activo
        };
    }

    public static IEnumerable<ProveedorDto> ToDto(this IEnumerable<Proveedor> proveedores)
    {
        return proveedores.Select(p => p.ToDto());
    }
}