using MotoPOS.API.Constants;
using MotoPOS.API.Data;
using MotoPOS.API.DTOs.Ventas;
using MotoPOS.API.Entities.Inventario;
using MotoPOS.API.Entities.Ventas;
using MotoPOS.API.Enums;
using MotoPOS.API.Exceptions;
using MotoPOS.API.Interfaces.Clientes;
using MotoPOS.API.Interfaces.Movimiento_Inventario;
using MotoPOS.API.Interfaces.Productos;
using MotoPOS.API.Interfaces.Ventas;

namespace MotoPOS.API.Services.Ventas
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IMovimientoInventarioRepository _movimientoInventarioRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly MotoPOSDbContext _context;
        public VentaService(IVentaRepository ventaRepository, IProductoRepository productoRepository, IMovimientoInventarioRepository movimientoInventarioRepository, IClienteRepository clienteRepository, MotoPOSDbContext context)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
            _movimientoInventarioRepository = movimientoInventarioRepository;
            _clienteRepository = clienteRepository;
            _context = context;
        }
        public async Task<VentaDto> CreateAsync(CrearVentaDto dto, int usuarioId)
        {
            var cliente = await _clienteRepository.GetByIdAsync(dto.ClienteId);
            if (cliente == null)
            {
                throw new NotFoundException(ErrorMessages.Clientes.ClienteNoEncontrado);
            }

            var detallesVenta = new List<DetalleVenta>();
            decimal total = 0;

            foreach (var detalle in dto.Detalles)
            {
                var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);

                if (producto == null)
                {
                    throw new NotFoundException(ErrorMessages.Productos.ProductoNoEncontrado);
                }

                if (!producto.Activo)
                {
                    throw new NotFoundException(ErrorMessages.Productos.ProductoInactivo);
                }

                if (producto.Stock < detalle.Cantidad)
                {
                    throw new NotFoundException(ErrorMessages.Productos.StockInsuficiente);
                }
                 
                var subtotal = producto.PrecioVenta * detalle.Cantidad;
                total += subtotal;

                detallesVenta.Add(new DetalleVenta
                {
                    ProductoId = producto.Id,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = producto.PrecioVenta,
                    Subtotal = subtotal
                });
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var venta = new Venta
                {
                    ClienteId = dto.ClienteId,
                    UsuarioId = usuarioId,
                    Total = total,
                    Detalles = detallesVenta
                };

                var ventaCreada = await _ventaRepository.CreateAsync(venta);

                foreach (var detalle in detallesVenta)
                {
                    var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                    producto!.Stock -= detalle.Cantidad;
                    await _productoRepository.UpdateAsync(producto);

                    var movimiento = new MovimientoInventario
                    {
                        ProductoId = producto.Id,
                        Cantidad = detalle.Cantidad,
                        TipoMovimiento = TipoMovimientoInventario.Venta,
                        Referencia = "Venta",
                        ReferenciaId = ventaCreada.Id,
                        UsuarioId = usuarioId,
                        Observaciones = $"Salida por venta #{ventaCreada.Id}"
                    };

                    await _movimientoInventarioRepository.CreateAsync(movimiento);
                }
                await transaction.CommitAsync();
                return new VentaDto
                {
                    Id = ventaCreada.Id,
                    Total = ventaCreada.Total,
                    ClienteId = ventaCreada.ClienteId,
                    UsuarioId = ventaCreada.UsuarioId,
                    Detalle = ventaCreada.Detalles.Select(detalle => new DetalleVentaDto
                    {
                        Id = detalle.Id,
                        VentaId = detalle.VentaId,
                        ProductoId = detalle.ProductoId,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Subtotal = detalle.Subtotal
                    }).ToList()
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<VentaDto?> GetByIdAsync(int id)
        {
            var venta = await _ventaRepository.GetByIdAsync(id);

            if (venta == null)
            {
                throw new NotFoundException(ErrorMessages.Ventas.VentaNoEncontrada);
            }

            return new VentaDto
            {
                Id = venta.Id,
                Total = venta.Total,
                ClienteId = venta.ClienteId,
                UsuarioId = venta.UsuarioId,
                Detalle = venta.Detalles.Select(detalle => new DetalleVentaDto
                {
                    Id = detalle.Id,
                    VentaId = detalle.VentaId,
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario,
                    Subtotal = detalle.Subtotal
                }).ToList()
            };
        }
        public async Task<IEnumerable<VentaDto>> GetAllAsync()
        {
            var ventas = await _ventaRepository.GetAllAsync();

            return ventas.Select(venta => new VentaDto
            {
                Id = venta.Id,
                Total = venta.Total,
                ClienteId = venta.ClienteId,
                UsuarioId = venta.UsuarioId,
                Detalle = venta.Detalles.Select(detalle => new DetalleVentaDto
                {
                    Id = detalle.Id,
                    VentaId = detalle.VentaId,
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario,
                    Subtotal = detalle.Subtotal
                }).ToList()
            });
        }
    }
}