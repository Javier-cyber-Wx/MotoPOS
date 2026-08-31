using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Interfaces.Clientes;

namespace MotoPOS.API.Repositories.Clientes
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly MotoPOSDbContext _context;
        public ClienteRepository(MotoPOSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);

            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsByNitAsync(string nit)
        {
            return await _context.Clientes
                .AnyAsync(c => c.Nit == nit);
        }
        public async Task<bool> ExistsByNitExceptIdAsync(string nit, int id)
        {
            return await _context.Clientes
                .AnyAsync(c => c.Nit == nit && c.Id != id);
        }
    }
}
