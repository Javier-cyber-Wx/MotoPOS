using MotoPOS.API.DTOs.Clientes;

namespace MotoPOS.API.Interfaces.Clientes
{
    public interface IClientService
    {
        Task<IEnumerable<ClienteDto>> GetAllAsync();

        Task<ClienteDto?> GetByIdAsync(int id);

        Task<ClienteDto> CreateAsync(CrearClienteDto dto);

        Task<bool> UpdateAsync(int id, ActualizarClienteDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
