using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.DTOs.MovimientoInventario;
using MotoPOS.API.Interfaces.Movimiento_Inventario;

namespace MotoPOS.API.Controllers.Movimiento_Inventario
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MovimientoInventarioController : ControllerBase
    {
        private readonly IMovimientoInventarioService _service;

        public MovimientoInventarioController(
            IMovimientoInventarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimentoInventarioDto>>> GetAll()
        {
            var movimientos = await _service.GetAllAsync();

            return Ok(movimientos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimentoInventarioDto>> GetById(int id)
        {
            var movimiento = await _service.GetByIdAsync(id);

            return Ok(movimiento);
        }
    }
}