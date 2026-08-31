using MotoPOS.API.Data;
using MotoPOS.API.Interfaces.Movimiento_Inventario;
using MotoPOS.API.Entities.Inventario;


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
    }
}
