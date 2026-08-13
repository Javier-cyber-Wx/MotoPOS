using MotoPOS.API.Entities.Personas;

namespace MotoPOS.API.Interfaces.Proveedores
{
    public interface IProveedorRepository
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<Proveedor?> GetByIdAsync(int id);
        Task<Proveedor?> GetByNitAsync(string nit);
        Task AddAsync(Proveedor proveedor);
        Task UpdateAsync(Proveedor proveedor);
        Task DeleteAsync(Proveedor proveedor);
        Task<bool> ExistsByNitAsync(string nit);
        Task<bool> ExistsByNitExceptIdAsync(string nit, int id);
        Task<Proveedor> CreateAsync(Proveedor proveedor);
    }
}   