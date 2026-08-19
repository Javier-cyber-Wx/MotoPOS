using MotoPOS.API.DTOs.Productos;
using MotoPOS.API.Entities.Catalogos;

namespace MotoPOS.API.Extensions;

public static class ProductoExtensions
{
    public static ProductoDTO ToDto(this Producto producto)
    {
        return new ProductoDTO
        {
            Id = producto.Id,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Marca = producto.Marca?.Nombre ?? string.Empty,
            Categoria = producto.Categoria?.Nombre ?? string.Empty,
            Stock = producto.Stock,
            StockMinimo = producto.StockMinimo,
            PrecioCompra = producto.PrecioCompra,
            PrecioVenta = producto.PrecioVenta,
            Activo = producto.Activo
        };
    }

    public static IEnumerable<ProductoDTO> ToDto(this IEnumerable<Producto> productos)
    {
        return productos.Select(p => p.ToDto());
    }
}