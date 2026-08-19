using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Productos;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.Extensions;
using MotoPOS.API.Interfaces.Productos;
using MotoPOS.API.Services;
using MotoPOS.API.Exceptions;

namespace MotoPOS.API.Services.Productos
{
    public class ProductoService : BaseService, IProductoService
    {
        private readonly IProductRepository _repository;

        public ProductoService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllAsync()
        {
            var productos = await _repository.GetAllAsync();
            return productos.ToDto();
        }

        public async Task<ProductoDTO?> GetByIdAsync(int id)
        {
            var producto = await _repository.GetByIdAsync(id);

            if (producto == null)
                return null;

            return producto.ToDto();
        }
        public async Task<ProductoDTO> CreateAsync(CrearProductoDto dto)
        {
            var codigoExistente = await _repository.ExistsByCodigoAsync(dto.Codigo);
            ValidateDuplicate(codigoExistente, ErrorMessages.Productos.CodigoDuplicado);

            var marcaExiste = await _repository.MarcaExistsAsync(dto.MarcaId);
            ValidateExists(marcaExiste, ErrorMessages.Productos.MarcaNoExiste);

            var categoriaExiste = await _repository.CategoriaExistsAsync(dto.CategoriaId);
            ValidateExists(categoriaExiste, ErrorMessages.Productos.CategoriaNoExiste);

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
                   ?? throw new InvalidOperationException(ErrorMessages.Productos.ProductoNoEncontrado);
        }
        public async Task UpdateAsync(int id, ActualizarProductoDto dto)
        {
            var producto = await _repository.GetByIdAsync(id);
            ValidateEntityExists(producto, ErrorMessages.Productos.ProductoNoEncontrado);

            var codigoExistente = await _repository
                .ExistsByCodigoExceptIdAsync(dto.Codigo, id);
            ValidateDuplicate(codigoExistente, ErrorMessages.Productos.CodigoDuplicado);

            var marcaExiste = await _repository.MarcaExistsAsync(dto.MarcaId);
            ValidateExists(marcaExiste, ErrorMessages.Productos.MarcaNoExiste);

            var categoriaExiste = await _repository.CategoriaExistsAsync(dto.CategoriaId);
            ValidateExists(categoriaExiste, ErrorMessages.Productos.CategoriaNoExiste);

            producto!.Codigo = dto.Codigo;
            producto!.Nombre = dto.Nombre;
            producto!.MarcaId = dto.MarcaId;
            producto!.CategoriaId = dto.CategoriaId;
            producto!.PrecioCompra = dto.PrecioCompra;
            producto!.PrecioVenta = dto.PrecioVenta;
            producto!.StockMinimo = dto.StockMinimo;
            producto!.Activo = dto.Activo;

            await _repository.UpdateAsync(producto);
        }
        public async Task DeleteAsync(int id)
        {
            var producto = await _repository.GetByIdAsync(id);
            ValidateEntityExists(producto, ErrorMessages.Productos.ProductoNoEncontrado);
            await _repository.DeleteAsync(producto!);
        }
    }
}
