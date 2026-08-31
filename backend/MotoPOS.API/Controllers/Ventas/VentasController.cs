using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.DTOs.Ventas;
using MotoPOS.API.Interfaces.Ventas;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MotoPOS.API.Controllers.Ventas
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var venta = await _ventaService.GetByIdAsync(id);
            return Ok(venta);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetAll()
        {
            var ventas = await _ventaService.GetAllAsync();
            return Ok(ventas);  
        }
        [HttpPost]
        public async Task<ActionResult<VentaDto>> Create(CrearVentaDto dto)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var venta = await _ventaService.CreateAsync(dto, usuarioId);
            return CreatedAtAction(nameof(GetById), new { id = venta.Id }, venta);
        }
    }
}