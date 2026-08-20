using MotoPOS.API.Entities.Catalogos;

namespace MotoPOS.API.Interfaces.Categorias;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task AddAsync(Categoria categoria);
    Task UpdateAsync(Categoria categoria);
    Task DeleteAsync(Categoria categoria);
    Task<bool> ExistsByNombreAsync(string nombre);
    Task<bool> ExistsByNombreExceptIdAsync(string nombre, int id);
}