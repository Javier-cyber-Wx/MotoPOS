using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.Interfaces.Productos;

namespace MotoPOS.API.Repositories.Productos
{
    public class ProductoRepository : IProductRepository
    {
        private readonly MotoPOSDbContext _context;
        public ProductoRepository(MotoPOSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .ToListAsync();
        }
        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Producto> CreateAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);

            await _context.SaveChangesAsync();

            return producto;
        }
        public async Task UpdateAsync(Producto producto)
        {
            _context.Productos.Update(producto);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Producto producto)
        {
            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();
        }
    }
}