using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.MovimientoInventario;
using MotoPOS.API.Exceptions;
using MotoPOS.API.Interfaces.Movimiento_Inventario;

namespace MotoPOS.API.Services.Movimiento_Inventario
{
    public class MovimientoInventarioService : IMovimientoInventarioService
    {
        private readonly IMovimientoInventarioRepository _repository;

        public MovimientoInventarioService(
            IMovimientoInventarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MovimentoInventarioDto>> GetAllAsync()
        {
            var movimientos = await _repository.GetAllAsync();

            return movimientos.Select(MapToDto).ToList();
        }

        public async Task<MovimentoInventarioDto> GetByIdAsync(int id)
        {
            var movimiento = await _repository.GetByIdAsync(id);

            if (movimiento == null)
            {
                throw new NotFoundException(
                    ErrorMessages.MovimientosInventario.MovimientoNoEncontrado);
            }

            return MapToDto(movimiento);
        }

        private static MovimentoInventarioDto MapToDto(
            Entities.Inventario.MovimientoInventario movimiento)
        {
            return new MovimentoInventarioDto
            {
                Id = movimiento.Id,
                Fecha = movimiento.Fecha,
                ProductoId = movimiento.ProductoId,
                Cantidad = movimiento.Cantidad,
                TipoMovimiento = (int)movimiento.TipoMovimiento,
                Referencia = movimiento.Referencia,
                ReferenciaId = movimiento.ReferenciaId,
                UsuarioId = movimiento.UsuarioId,
                Observaciones = movimiento.Observaciones
            };
        }
    }
}