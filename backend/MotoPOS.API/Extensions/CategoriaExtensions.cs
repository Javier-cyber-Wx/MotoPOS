using MotoPOS.API.DTOs.Categorias;
using MotoPOS.API.Entities.Catalogos;

namespace MotoPOS.API.Extensions;

public static class CategoriaExtensions
{
    public static CategoriaDTO ToDto(this Categoria categoria)
    {
        return new CategoriaDTO
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Activo = categoria.Activo
        };
    }

    public static IEnumerable<CategoriaDTO> ToDto(this IEnumerable<Categoria> categorias)
    {
        return categorias.Select(c => c.ToDto());
    }
}