using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Data;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.Interfaces.Categorias;

namespace MotoPOS.API.Repositories.Categorias;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly MotoPOSDbContext _context;
    public CategoriaRepository(MotoPOSDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await _context.Categorias.ToListAsync();
    }
    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }
    public async Task AddAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> ExistsByNombreAsync(string nombre)
    {
        return await _context.Categorias.AnyAsync(c => c.Nombre == nombre);
    }
    public async Task<bool> ExistsByNombreExceptIdAsync(string nombre, int id)
    {
        return await _context.Categorias.AnyAsync(c => c.Nombre == nombre && c.Id != id);
    }
}