using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.Controllers.Base;
using MotoPOS.API.DTOs.Proveedores; 
using MotoPOS.API.Interfaces.Proveedores;

namespace MotoPOS.API.Controllers.Proveedores
{
    [Route("api/[controller]")]
    public class ProveedoresController : BaseController
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
            catch (InvalidOperationException ex)
            {
                return HandleError(ex);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ActualizarProveedorDto dto)
        {
            try
            {
                await _proveedorService.UpdateAsync(id, dto);
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
                await _proveedorService.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return HandleError(ex);
            }
        }
    }
}   