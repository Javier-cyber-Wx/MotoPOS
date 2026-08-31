using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.Controllers.Base;
using MotoPOS.API.DTOs.Proveedores; 
using MotoPOS.API.Interfaces.Proveedores;

namespace MotoPOS.API.Controllers.Proveedores
{
    [Authorize]
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
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ProveedorDto>> Create(CrearProveedorDto dto)
        {
            var proveedor = await _proveedorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = proveedor.Id }, proveedor);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(int id, ActualizarProveedorDTO dto)
        {
             await _proveedorService.UpdateAsync(id, dto);
             return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
             await _proveedorService.DeleteAsync(id);
             return NoContent();
        }
    }
}   