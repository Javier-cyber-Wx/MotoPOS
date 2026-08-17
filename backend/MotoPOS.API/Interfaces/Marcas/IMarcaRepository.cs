using MotoPOS.API.Entities.Catalogos;  

namespace MotoPOS.API.Interfaces.Marcas
{
    public interface IMarcaRepository
    {
        Task<IEnumerable<Marca>> GetAllAsync();
        Task<Marca?> GetByIdAsync(int id);
        Task<Marca?> GetByNombreAsync(string nombre);
        Task AddAsync(Marca marca);
        Task UpdateAsync(Marca marca);
        Task DeleteAsync(Marca marca);
        Task<bool> ExistsByNombreAsync(string nombre);
        Task<bool> ExistsByNombreExceptIdAsync(string nombre, int id);
        Task<Marca> CreateAsync(Marca marca);
    }
}   

