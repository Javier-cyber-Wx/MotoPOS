using MotoPOS.API.DTOs;
using MotoPOS.API.DTOs.Productos;

namespace MotoPOS.API.Interfaces.Productos;

public interface IProductoService
{
    Task<IEnumerable<ProductoDTO>> GetAllAsync();
    Task<PaginatedResponseDTO<ProductoDTO>> GetPaginatedAsync(int pageNumber, int pageSize);

    Task<ProductoDTO?> GetByIdAsync(int id);

    Task<ProductoDTO> CreateAsync(CrearProductoDto dto);

    Task UpdateAsync(int id, ActualizarProductoDTO dto);

    Task DeleteAsync(int id);
}