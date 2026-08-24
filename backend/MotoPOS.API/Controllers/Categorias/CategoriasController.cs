using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.Controllers.Base;
using MotoPOS.API.DTOs.Categorias;
using MotoPOS.API.Interfaces.Categorias;

namespace MotoPOS.API.Controllers.Categorias
{
    [Route("api/[controller]")]
    public class CategoriasController : BaseController
	{   
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAll()
        {
            var categorias = await _categoriaService.GetAllAsync();
            return Ok(categorias);
        }   
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetById(int id)
        {
            var categoria = await _categoriaService.GetById(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Create(CrearCategoriaDTO dto)
        {
            var createdCategoria = await _categoriaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdCategoria.Id }, createdCategoria);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ActualizarCategoriaDTO dto)
        {
             await _categoriaService.UpdateAsync(id, dto);
             return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
             await _categoriaService.DeleteAsync(id);
             return NoContent();
        }
    } 
}