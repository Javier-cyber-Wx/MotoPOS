using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.DTOs.Categorias;
using MotoPOS.API.Interfaces.Categorias;

namespace MotoPOS.API.Controllers.Categorias
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
	{   
        private readonly ICategoriaService _CategoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _CategoriaService = categoriaService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAll()
        {
            var categorias = await _CategoriaService.GetAllSync();
            return Ok(categorias);
        }   
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetById(int id)
        {
            var categoria = await _CategoriaService.GetById(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Create(CrearCategoriaDTO dto)
        {
            try
            {
                var createdCategoria = await _CategoriaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdCategoria.Id }, createdCategoria);
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
        public async Task<IActionResult> Update(int id, ActualizarCategoriaDTO dto)
        {
            try
            {
                var result = await _CategoriaService.UpdateAsync(id, dto);
                if(!result)
                {
                    return NotFound();
                } return NoContent();
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
            var result = await _CategoriaService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    } 
}