using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.Controllers.Base;
using MotoPOS.API.DTOs.Productos;
using MotoPOS.API.Interfaces.Productos;

namespace MotoPOS.API.Controllers.Productos
{
    [Route("api/[controller]")]
    public class ProductosController : BaseController
    {
        private readonly IProductoService _productoService;
        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetAll()
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDTO>> GetById(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }
        [HttpPost]
        public async Task<ActionResult<ProductoDTO>> Create(CrearProductoDto dto)
        {
            try
            {
                var producto = await _productoService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
            }
            catch (InvalidOperationException ex)
            {
                return HandleError(ex);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ActualizarProductoDto dto)
        {
            try
            {
                await _productoService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return HandleError(ex);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productoService.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return HandleError(ex);
            }
        }
    }
}
