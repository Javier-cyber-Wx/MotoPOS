using MotoPOS.API.Entities.Inventario;

namespace MotoPOS.API.Interfaces.Movimiento_Inventario
{
    public interface IMovimientoInventarioRepository
    {
        Task<MovimientoInventario> CreateAsync(MovimientoInventario movimiento);
        Task<IEnumerable<MovimientoInventario>> GetAllAsync();
        Task<MovimientoInventario?> GetByIdAsync(int id);
    }
}
