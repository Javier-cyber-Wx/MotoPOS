using MotoPOS.API.DTOs.Productos;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.Interfaces.Productos;
using MotoPOS.API.Repositories.Productos;

namespace MotoPOS.API.Services.Productos
{
    public class ProductoService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductoService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllAsync()
        {
            var productos = await _repository.GetAllAsync();

            return productos.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Marca = p.Marca.Nombre,
                Categoria = p.Categoria.Nombre,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                PrecioCompra = p.PrecioCompra,
                PrecioVenta = p.PrecioVenta,
                Activo = p.Activo
            });
        }

        public async Task<ProductoDTO?> GetByIdAsync(int id)
        {
            var producto = await _repository.GetByIdAsync(id);

            if (producto == null)
                return null;

            return new ProductoDTO
            {
                Id = producto.Id,
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Marca = producto.Marca.Nombre,
                Categoria = producto.Categoria.Nombre,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo,
                PrecioCompra = producto.PrecioCompra,
                PrecioVenta = producto.PrecioVenta,
                Activo = producto.Activo
            };
        }

        public async Task<ProductoDTO> CreateAsync(CrearProductoDto dto)
        {
            var producto = new Producto
            {
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                MarcaId = dto.MarcaId,
                CategoriaId = dto.CategoriaId,
                PrecioCompra = dto.PrecioCompra,
                PrecioVenta = dto.PrecioVenta,
                StockMinimo = dto.StockMinimo,
                Stock = 0,
                Activo = true
            };

            producto = await _repository.CreateAsync(producto);

            return await GetByIdAsync(producto.Id)
                   ?? throw new Exception("No se pudo recuperar el producto creado.");
        }

        public async Task<bool> UpdateAsync(int id, ActualizarProductoDto dto)
        {
            var producto = await _repository.GetByIdAsync(id);

            if (producto == null)
                return false;

            producto.Codigo = dto.Codigo;
            producto.Nombre = dto.Nombre;
            producto.MarcaId = dto.MarcaId;
            producto.CategoriaId = dto.CategoriaId;
            producto.PrecioCompra = dto.PrecioCompra;
            producto.PrecioVenta = dto.PrecioVenta;
            producto.StockMinimo = dto.StockMinimo;
            producto.Activo = dto.Activo;

            await _repository.UpdateAsync(producto);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _repository.GetByIdAsync(id);

            if (producto == null)
                return false;

            await _repository.DeleteAsync(producto);

            return true;
        }
    }
}
