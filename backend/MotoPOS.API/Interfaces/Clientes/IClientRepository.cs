using MotoPOS.API.Entities.Personas;

namespace MotoPOS.API.Interfaces.Clientes;

public interface IClientRepository
{
    Task<IEnumerable<Cliente>> GetAllAsync();

    Task<Cliente?> GetByIdAsync(int id);

    Task<Cliente> CreateAsync(Cliente cliente);

    Task UpdateAsync(Cliente cliente);

    Task DeleteAsync(Cliente cliente);

    Task<bool> ExistsByNitAsync(string nit);

    Task<bool> ExistsByNitExceptIdAsync(string nit, int id);
} 