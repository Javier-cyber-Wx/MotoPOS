using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.Controllers.Base;
using MotoPOS.API.DTOs.Clientes;
using MotoPOS.API.Interfaces.Clientes;

namespace MotoPOS.API.Controllers.Clientes;

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
    public async Task<ActionResult<ClienteDto>> Create(CrearClienteDto dto)
    {
        try
        {
            var cliente = await _clienteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente); 
        }
        catch (InvalidOperationException ex)
        {
            return HandleError(ex);
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        ActualizarClienteDto dto)
    {
        try
        {
            await _clienteService.UpdateAsync(id, dto);
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
            await _clienteService.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return HandleError(ex);
        }
    }
}

