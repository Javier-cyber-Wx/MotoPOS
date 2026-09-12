using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Compras;
using MotoPOS.API.Interfaces.Compras;

namespace MotoPOS.API.Repository.Compras
{
    public class CompraRepository : ICompraRepository
    {
        private readonly MotoPOSDbContext _context;
        public CompraRepository (MotoPOSDbContext context)
        {
            _context = context;
        }
        public async Task<Compra> CreateAsync(Compra compra)
        {
            await _context.AddAsync(compra);
            await _context.SaveChangesAsync();
            return compra;
        }
        public async Task<Compra?> GetByIdAsync(int id)
        {
            return await _context.Compras.Include(c => c.Detalles).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<IEnumerable<Compra>> GetAllAsync()
        {
            return await _context.Compras.Include(c => c.Detalles).ToListAsync();
        }
    }
}
