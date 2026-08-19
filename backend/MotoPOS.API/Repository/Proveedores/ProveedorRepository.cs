using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Interfaces.Proveedores;

namespace MotoPOS.API.Repositories.Proveedores
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly MotoPOSDbContext _context;
        public ProveedorRepository(MotoPOSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Proveedor>> GetAllAsync()
        {
            return await _context.Proveedores.ToListAsync();
        }
        public async Task<Proveedor?> GetByIdAsync(int id)
        {
            return await _context.Proveedores.FindAsync(id);
        }
        public async Task<Proveedor?> GetByNitAsync(string nit)
        {
            return await _context.Proveedores.FirstOrDefaultAsync(p => p.Nit == nit);
        }
        public async Task AddAsync(Proveedor proveedor)
        {
            await _context.Proveedores.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Proveedor proveedor)
        {
            _context.Proveedores.Update(proveedor);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Proveedor proveedor)
        {
            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsByNitAsync(string nit)
        {
            return await _context.Proveedores.AnyAsync(p => p.Nit == nit);
        }
        public async Task<bool> ExistsByNitExceptIdAsync(string nit, int id)
        {
            return await _context.Proveedores.AnyAsync(p => p.Nit == nit && p.Id != id);
        }
        public async Task<Proveedor> CreateAsync(Proveedor proveedor)
        {
            await _context.Proveedores.AddAsync(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }   
    }
}   