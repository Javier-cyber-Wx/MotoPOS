using MotoPOS.API.Entities.Base;

namespace MotoPOS.API.Entities.Catalogos;

public class Producto : AuditableEntity
{
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }


    // Inventario

    public int Stock { get; set; }


    // Precios

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }


    // Relaciones

    public int MarcaId { get; set; }

    public Marca Marca { get; set; } = null!;


    public int CategoriaId { get; set; }

    public Categoria Categoria { get; set; } = null!;
}