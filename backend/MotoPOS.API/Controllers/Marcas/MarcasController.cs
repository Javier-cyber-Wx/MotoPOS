using Microsoft.AspNetCore.Mvc; 
using MotoPOS.API.Controllers.Base;
using MotoPOS.API.DTOs.Marcas;
using MotoPOS.API.Interfaces.Marcas;    

namespace MotoPOS.API.Controllers.Marcas
{
    [Route("api/[controller]")]
    public class MarcasController : BaseController
    {
        private readonly IMarcaService _marcaService;
        public MarcasController(IMarcaService marcaService)
        {
            _marcaService = marcaService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcaDTO>>> GetAll()
        {
            var marcas = await _marcaService.GetAllAsync();
            return Ok(marcas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MarcaDTO>> GetById(int id)
        {
            var marca = await _marcaService.GetByIdAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            return Ok(marca);
        }
        [HttpPost]
        public async Task<ActionResult<MarcaDTO>> Create(CrearMarcaDto dto)
        {
                var createdMarca = await _marcaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdMarca.Id }, createdMarca);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ActualizarMarcaDTO dto)
        {
            await _marcaService.UpdateAsync(id, dto);
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _marcaService.DeleteAsync(id);
            return NoContent();
        }
    }
}   