using MotoPOS.API.DTOs.MovimientoInventario;

namespace MotoPOS.API.Interfaces.Movimiento_Inventario
{
    public interface IMovimientoInventarioService
    {
        Task<IEnumerable<MovimentoInventarioDto>> GetAllAsync();
        Task<MovimentoInventarioDto> GetByIdAsync(int id);
    }
}
