using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.DTOs.Proveedores; 
using MotoPOS.API.Interfaces.Proveedores;

namespace MotoPOS.API. Controllers.Proveedores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorService _proveedorService;
        public ProveedoresController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDto>>> GetAll()
        {
            var proveedores = await _proveedorService.GetAllAsync();
            return Ok(proveedores);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDto>> GetById(int id)
        {
            var proveedor = await _proveedorService.GetByIdAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return Ok(proveedor);
        }
        [HttpPost]
        public async Task<ActionResult<ProveedorDto>> Create(CrearProveedorDto dto)
        {
            try
            {
                var proveedor = await _proveedorService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = proveedor.Id }, proveedor);
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
        public async Task<IActionResult> Update(int id, ActualizarProveedorDto dto)
        {
            try
            {
                var result = await _proveedorService.UpdateAsync(id, dto);
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
            var result = await _proveedorService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}   