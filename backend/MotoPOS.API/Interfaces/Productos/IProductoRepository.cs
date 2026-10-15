using MotoPOS.API.Entities.Catalogos;

namespace MotoPOS.API.Interfaces.Productos;
public interface IProductoRepository
{
    Task<IEnumerable<Producto>> GetAllAsync();
    Task<(IEnumerable<Producto> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<Producto?> GetByIdAsync(int id);
    Task<Producto?> GetByIdForUpdateAsync(int id);
    Task<Producto> CreateAsync(Producto producto);
    Task UpdateAsync(Producto producto);
    Task DeleteAsync(Producto producto);
    Task<bool> ExistsByCodigoAsync(string codigo);
    Task<bool> ExistsByCodigoExceptIdAsync(string codigo, int id);
    Task<bool> MarcaExistsAsync(int marcaId);
    Task<bool> CategoriaExistsAsync(int categoriaId);
}
