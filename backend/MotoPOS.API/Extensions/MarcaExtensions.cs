using MotoPOS.API.DTOs.Marcas;
using MotoPOS.API.Entities.Catalogos;

namespace MotoPOS.API.Extensions;

public static class MarcaExtensions
{
    public static MarcaDTO ToDto(this Marca marca)
    {
        return new MarcaDTO
        {
            Id = marca.Id,
            Nombre = marca.Nombre,
            Activo = marca.Activo
        };
    }

    public static IEnumerable<MarcaDTO> ToDto(this IEnumerable<Marca> marcas)
    {
        return marcas.Select(m => m.ToDto());
    }
}