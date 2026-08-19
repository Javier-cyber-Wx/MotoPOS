using MotoPOS.API.DTOs.Proveedores;

namespace MotoPOS.API.Interfaces.Proveedores
{
    public interface IProveedorService
    {
        Task<IEnumerable<ProveedorDto>> GetAllAsync();

        Task<ProveedorDto?> GetByIdAsync(int id);

        Task<ProveedorDto> CreateAsync(CrearProveedorDto dto);

        Task UpdateAsync(int id, ActualizarProveedorDto dto);

        Task DeleteAsync(int id);
    }
}