using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.DTOs.Productos;
using MotoPOS.API.Interfaces.Productos;

namespace MotoPOS.API.Controllers.Productos
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductosController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetAll()
        {
            var productos = await _productService.GetAllAsync();
            return Ok(productos);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDTO>> GetById(int id)
        {
            var producto = await _productService.GetByIdAsync(id);
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

                var producto = await _productService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(new
                {
                    message = e.Message
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ActualizarProductoDto dto)
        {
            try
            {
                var result = await _productService.UpdateAsync(id, dto);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return Conflict(new
                {
                    message = e.Message
                }); 
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
