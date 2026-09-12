
using MotoPOS.API.Entities.Compras;

namespace MotoPOS.API.Interfaces.Compras
{
    public interface ICompraRepository
    {
        Task<IEnumerable<Compra>> GetAllAsync();
        Task<Compra?> GetByIdAsync(int id);
        Task<Compra> CreateAsync(Compra compra);
    }
}
