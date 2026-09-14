using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.Interfaces.Productos;

namespace MotoPOS.API.Repositories.Productos
{
    public class ProductoRepository : IProductoRepository
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
        public async Task<Producto?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Productos
                .FromSqlInterpolated($@"
            SELECT *
            FROM productos
            WHERE Id = {id}
            FOR UPDATE")
                .FirstOrDefaultAsync();
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
        public async Task<bool> ExistsByCodigoAsync(string codigo)
        {
            return await _context.Productos.AnyAsync(p => p.Codigo == codigo);
        }
        public async Task<bool> MarcaExistsAsync(int marcaId)
        {
            return await _context.Marcas.AnyAsync(m => m.Id == marcaId);
        }
        public async Task<bool> CategoriaExistsAsync(int categoriaId)
        {
            return await _context.Categorias.AnyAsync(c => c.Id == categoriaId);
        }
        public async Task<bool> ExistsByCodigoExceptIdAsync(string codigo, int id)
        {
            return await _context.Productos
                .AnyAsync(p => p.Codigo == codigo && p.Id != id);
        }
    }
}