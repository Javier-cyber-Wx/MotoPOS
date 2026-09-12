using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.Controllers.Base;
using MotoPOS.API.DTOs.Clientes;
using MotoPOS.API.Interfaces.Clientes;

namespace MotoPOS.API.Controllers.Clientes
{
    [Authorize]
    [Route("api/Clientes")]
    public class ClientesController : BaseController
    {
        private readonly IClienteService _clienteService;
        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador, Vendedor")]
        public async Task<ActionResult<ClienteDto>> GetById(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador, Vendedor")]
        public async Task<ActionResult<ClienteDto>> Create(CrearClienteDto dto)
        {
            var cliente = await _clienteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador, Vendedor")]
        public async Task<IActionResult> Update(int id, ActualizarClienteDTO dto)
        {
            await _clienteService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clienteService.DeleteAsync(id);
            return NoContent();
        }
    }
}