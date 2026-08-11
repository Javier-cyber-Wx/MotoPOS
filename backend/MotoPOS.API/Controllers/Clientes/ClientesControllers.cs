using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.DTOs.Clientes;
using MotoPOS.API.Interfaces.Clientes;

namespace MotoPOS.API.Controllers.Clientes;

[ApiController]
[Route("api/Clientes")]
public class ClientesController : ControllerBase
{
    private readonly IClientService _clientService;
    public ClientesController(IClientService clientService)
    {
        _clientService = clientService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
    {
        var clientes = await _clientService.GetAllAsync();
        return Ok(clientes);    
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var cliente = await _clientService.GetByIdAsync(id);

        if (cliente == null)
        {
            return NotFound();
        }

        return Ok(cliente);
    }
    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create(CrearClienteDto dto)
    {
        try
        {
            var cliente = await _clientService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente); 
        }
        catch (InvalidOperationException e)
        {
            return Conflict(new { message = e.Message });   
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        ActualizarClienteDto dto)
    {
        try
        {
            var result = await _clientService.UpdateAsync(id, dto);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _clientService.DeleteAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}

