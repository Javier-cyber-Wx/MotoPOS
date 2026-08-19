using MotoPOS.API.DTOs.Productos;

namespace MotoPOS.API.Interfaces.Productos;

public interface IProductoService
{
    Task<IEnumerable<ProductoDTO>> GetAllAsync();

    Task<ProductoDTO?> GetByIdAsync(int id);

    Task<ProductoDTO> CreateAsync(CrearProductoDto dto);

    Task UpdateAsync(int id, ActualizarProductoDto dto);

    Task DeleteAsync(int id);
}