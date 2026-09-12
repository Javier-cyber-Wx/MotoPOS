using MotoPOS.API.Constants;
using MotoPOS.API.Data;
using MotoPOS.API.DTOs.Compras;
using MotoPOS.API.Entities.Inventario;
using MotoPOS.API.Entities.Compras;
using MotoPOS.API.Enums;
using MotoPOS.API.Exceptions;
using MotoPOS.API.Interfaces.Compras;
using MotoPOS.API.Interfaces.Movimiento_Inventario;
using MotoPOS.API.Interfaces.Productos;
using MotoPOS.API.Interfaces.Proveedores;
using System.ComponentModel.DataAnnotations;
namespace MotoPOS.API.Services.Compras
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IMovimientoInventarioRepository _movimientoInventarioRepository;
        private readonly MotoPOSDbContext _context;
        public CompraService(
            ICompraRepository compraRepository,
            IProveedorRepository proveedorRepository,
            IProductoRepository productoRepository,
            IMovimientoInventarioRepository movimientoInventarioRepository,
            MotoPOSDbContext context)
        {
            _compraRepository = compraRepository;
            _proveedorRepository = proveedorRepository;
            _productoRepository = productoRepository;
            _movimientoInventarioRepository = movimientoInventarioRepository;
            _context = context;
        }
        public async Task<CompraDto> CreateAsync(CrearCompraDto dto, int usuarioId)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(dto.ProveedorId);
            if (proveedor == null)
            {
                throw new NotFoundException(ErrorMessages.Proveedores.ProveedorNoEncontrado);
            }
            if (dto.Detalles == null || !dto.Detalles.Any())
            {
                throw new ValidationException("La compra debe tener al menos un detalle.");
            }
            var detallesCompra = new List<DetalleCompra>();
            decimal total = 0;

            foreach (var detalleDto in dto.Detalles)
            {
                if (detalleDto.Cantidad <= 0)
                {
                    throw new ValidationException("La cantidad debe ser mayor a cero.");
                }
                if (detalleDto.CostoUnitario <= 0)
                {
                    throw new ValidationException("El costo unitario debe ser mayor a cero.");
                }
                var producto = await _productoRepository.GetByIdAsync(detalleDto.ProductoId);
                if (producto == null)
                {
                    throw new NotFoundException(ErrorMessages.Productos.ProductoNoEncontrado);
                }
                var subtotal = detalleDto.Cantidad * detalleDto.CostoUnitario;
                total += subtotal;
                detallesCompra.Add(new DetalleCompra
                {
                    ProductoId = detalleDto.ProductoId,
                    Cantidad = detalleDto.Cantidad,
                    CostoUnitario = detalleDto.CostoUnitario,
                    Subtotal = subtotal
                });
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var compra = new Compra
                {
                    ProveedorId = dto.ProveedorId,
                    UsuarioId = usuarioId,
                    Total = total,
                    Detalles = detallesCompra
                };
                var compraCreada = await _compraRepository.CreateAsync(compra);
                foreach (var detalle in detallesCompra)
                {
                    var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                    producto!.Stock += detalle.Cantidad;
                    producto.PrecioCompra = detalle.CostoUnitario;
                    await _productoRepository.UpdateAsync(producto);
                    var movimientoInventario = new MovimientoInventario
                    {
                        ProductoId = producto.Id,
                        Cantidad = detalle.Cantidad,
                        TipoMovimiento = TipoMovimientoInventario.Compras,
                        Referencia = "Compra",
                        ReferenciaId = compraCreada.Id,
                        UsuarioId = usuarioId,
                        Observaciones = $"Entrada por compra #{compraCreada.Id}"
                    };
                    await _movimientoInventarioRepository.CreateAsync(movimientoInventario);
                }
                await transaction.CommitAsync();
                return new CompraDto
                {
                    Id = compraCreada.Id,
                    Fecha = compraCreada.Fecha,
                    Total = compraCreada.Total,
                    ProveedorId = compraCreada.ProveedorId,
                    UsuarioId = compraCreada.UsuarioId,
                    Detalles = compraCreada.Detalles.Select(detalle => new DetalleCompraDto
                    {
                        Id = detalle.Id,
                        CompraId = detalle.CompraId,
                        ProductoId = detalle.ProductoId,
                        Cantidad = detalle.Cantidad,
                        CostoUnitario = detalle.CostoUnitario,
                        Subtotal = detalle.Subtotal,
                    }).ToList(),
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<IEnumerable<CompraDto>> GetAllAsync()
        {
            var compras = await _compraRepository.GetAllAsync();
            return compras.Select(compra => new CompraDto
            {
                Id = compra.Id,
                Fecha = compra.Fecha,
                Total = compra.Total,
                ProveedorId = compra.ProveedorId,
                UsuarioId = compra.UsuarioId,
                Detalles = compra.Detalles.Select(detalle => new DetalleCompraDto
                {
                    Id = detalle.Id,
                    CompraId = detalle.CompraId,
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    CostoUnitario = detalle.CostoUnitario,
                    Subtotal = detalle.Subtotal,
                }).ToList(),
            }).ToList();
        }
        public async Task<CompraDto?> GetByIdAsync(int id)
        {
            var compra = await _compraRepository.GetByIdAsync(id);
            if (compra == null)
            {
                throw new NotFoundException(ErrorMessages.Compras.CompraNoEncontrada);
            }
            return new CompraDto
            {
                Id = compra.Id,
                Fecha = compra.Fecha,
                Total = compra.Total,
                ProveedorId = compra.ProveedorId,
                UsuarioId = compra.UsuarioId,
                Detalles = compra.Detalles.Select(detalle => new DetalleCompraDto
                {
                    Id = detalle.Id,
                    CompraId = detalle.CompraId,
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    CostoUnitario = detalle.CostoUnitario,
                    Subtotal = detalle.Subtotal,
                }).ToList(),
            };
        }
    }
}
