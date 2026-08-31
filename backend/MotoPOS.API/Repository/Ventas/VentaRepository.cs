using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Ventas;
using MotoPOS.API.Interfaces.Ventas;

namespace MotoPOS.API.Repository.Ventas
{
    public class VentaRepository : IVentaRepository
    {
        private readonly MotoPOSDbContext _context;
        public VentaRepository(MotoPOSDbContext context)
        {
            _context = context;
        }
        public async Task<Venta> CreateAsync(Venta venta)
        {
            await _context.AddAsync(venta);
            await _context.SaveChangesAsync();
            return venta;
        } 
        public async Task<Venta?> GetByIdAsync(int id)
        {
            return await _context.Ventas.Include(v => v.Detalles).FirstOrDefaultAsync(v => v.Id == id);
        }
        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            return await _context.Ventas.Include(v => v.Detalles).ToListAsync();
        }
    }
}
