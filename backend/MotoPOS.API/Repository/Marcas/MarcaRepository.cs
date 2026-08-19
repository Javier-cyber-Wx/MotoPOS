using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.Interfaces.Marcas;

namespace MotoPOS.API.Repositories.Marcas
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly MotoPOSDbContext _context;
        public MarcaRepository(MotoPOSDbContext context)
        {
            _context = context; 
        }
        public async Task<IEnumerable<Marca>> GetAllAsync()
        {
            return await _context.Marcas.ToListAsync();
        }
        public async Task<Marca?> GetByIdAsync(int id)
        {
            return await _context.Marcas.FindAsync(id);
        }

        public async Task<Marca?> GetByNombreAsync(string nombre)
        {
            return await _context.Marcas.FirstOrDefaultAsync(m => m.Nombre == nombre);
        }

        public async Task AddAsync(Marca marca)
        {
            await _context.Marcas.AddAsync(marca);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Marca marca)
        {
            _context.Marcas.Update(marca);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Marca marca)
        {
            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNombreAsync(string nombre)
        {
            return await _context.Marcas.AnyAsync(m => m.Nombre == nombre);
        }

        public async Task<bool> ExistsByNombreExceptIdAsync(string nombre, int id)
        {
            return await _context.Marcas.AnyAsync(m => m.Nombre == nombre && m.Id != id);
        }

        public async Task<Marca> CreateAsync(Marca marca)
        {
            await _context.Marcas.AddAsync(marca);
            await _context.SaveChangesAsync();
            return marca;
        }
    }
}