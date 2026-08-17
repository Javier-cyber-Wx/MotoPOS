using Microsoft.AspNetCore.Mvc; 
using MotoPOS.API.DTOs.Marcas;
using MotoPOS.API.Interfaces.Marcas;    

namespace MotoPOS.API.Controllers.Marcas
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasController : ControllerBase
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
            try
            {
                var createdMarca = await _marcaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdMarca.Id }, createdMarca);
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
        public async Task<IActionResult> Update(int id, ActualizarMarcaDto dto)
        {
            try
            {
                var result = await _marcaService.UpdateAsync(id, dto);
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
            var deleted = await _marcaService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}   