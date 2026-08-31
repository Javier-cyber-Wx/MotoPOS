using MotoPOS.API.Entities.Inventario;

namespace MotoPOS.API.Interfaces.Movimiento_Inventario
{
    public interface IMovimientoInventarioRepository
    {
        Task<MovimientoInventario> CreateAsync(MovimientoInventario movimiento);
    }
}
