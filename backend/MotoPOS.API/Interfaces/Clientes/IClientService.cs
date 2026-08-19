using MotoPOS.API.DTOs.Clientes;

namespace MotoPOS.API.Interfaces.Clientes
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> GetAllAsync();

        Task<ClienteDto?> GetByIdAsync(int id);

        Task<ClienteDto> CreateAsync(CrearClienteDto dto);

        Task UpdateAsync(int id, ActualizarClienteDto dto);

        Task DeleteAsync(int id);
    }
}
