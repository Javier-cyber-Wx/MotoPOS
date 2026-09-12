using MotoPOS.API.DTOs.Compras;

namespace MotoPOS.API.Interfaces.Compras
{
    public interface ICompraService
    {
        Task<IEnumerable<CompraDto>> GetAllAsync();
        Task<CompraDto> CreateAsync(CrearCompraDto dto, int usuarioId);
        Task<CompraDto?> GetByIdAsync(int id);
    }
}
