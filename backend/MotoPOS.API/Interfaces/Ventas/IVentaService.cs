using MotoPOS.API.DTOs.Ventas;

namespace MotoPOS.API.Interfaces.Ventas
{
    public interface IVentaService
    {
        Task<VentaDto> CreateAsync(CrearVentaDto dto, int usuarioId);
        Task<VentaDto?> GetByIdAsync(int id);
        Task<IEnumerable<VentaDto>> GetAllAsync();
    }
}
