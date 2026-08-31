using MotoPOS.API.Entities.Ventas;

namespace MotoPOS.API.Interfaces.Ventas
{
    public interface IVentaRepository
    {
        Task<Venta> CreateAsync(Venta venta);
        Task<Venta?> GetByIdAsync(int id);
        Task<IEnumerable<Venta>> GetAllAsync();
    }
}
