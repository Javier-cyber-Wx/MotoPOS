using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Inventario;
using MotoPOS.API.Interfaces.Movimiento_Inventario;


namespace MotoPOS.API.Repository.Movimiento_Inventario
{
    public class MovimientoInventarioRepository : IMovimientoInventarioRepository
    {
        private readonly MotoPOSDbContext _context;
        public MovimientoInventarioRepository(MotoPOSDbContext context)
        {
            _context = context;
        }
        public async Task<MovimientoInventario> CreateAsync(MovimientoInventario movimiento)
        {
            await _context.MovimientosInventario.AddAsync(movimiento);
            await _context.SaveChangesAsync();
            return movimiento;
        }
        public async Task<IEnumerable<MovimientoInventario>> GetAllAsync()
        {
            return await _context.MovimientosInventario
                            .OrderByDescending(m => m.Fecha)
                            .ToListAsync();
        }
        public async Task<MovimientoInventario?> GetByIdAsync(int id)
        {
            return await _context.MovimientosInventario
                .FirstOrDefaultAsync(m => m.Id == id);
        }

    }
}
