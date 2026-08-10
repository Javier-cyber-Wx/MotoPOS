using MotoPOS.API.DTOs.Productos;

namespace MotoPOS.API.Interfaces.Productos;

public interface IProductService
{
    Task<IEnumerable<ProductoDTO>> GetAllAsync();

    Task<ProductoDTO?> GetByIdAsync(int id);

    Task<ProductoDTO> CreateAsync(CrearProductoDto dto);

    Task<bool> UpdateAsync(int id, ActualizarProductoDto dto);

    Task<bool> DeleteAsync(int id);
}