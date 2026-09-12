using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.DTOs.Compras;
using MotoPOS.API.Interfaces.Compras;
using System.Security.Claims;

namespace MotoPOS.API.Controllers.Compras
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class CompraController : ControllerBase
    {
        private readonly ICompraService _compraService;

        public CompraController(ICompraService compraService)
        {
            _compraService = compraService;
        }

        [HttpPost]
        public async Task<ActionResult<CompraDto>> Create(CrearCompraDto compraDto)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var compra = await _compraService.CreateAsync(compraDto, usuarioId);
            return Ok(compra);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompraDto>>> GetAll()
        {
            var compras = await _compraService.GetAllAsync();
            return Ok(compras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompraDto>> Get(int id)
        {
            var compra = await _compraService.GetByIdAsync(id);
            return Ok(compra);
        }
    }
}
