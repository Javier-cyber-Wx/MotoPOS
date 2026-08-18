using MotoPOS.API.Interfaces.Categorias;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.DTOs.Categorias;


namespace MotoPOS.API.Services.Categorias
{
    public class CategoriaService : ICategoriaService
    {
        public readonly ICategoriaRepository _repository;
        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<CategoriaDTO>> GetAllSync()
        {
            var categorias = await _repository.GetAllAsync();
            return categorias.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Activo = c.Activo
            });
        }
        public async Task<CategoriaDTO?> GetById(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);
            if (categoria == null) return null;
            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Activo = categoria.Activo
            };
        }
        public async Task<CategoriaDTO> CreateAsync(CrearCategoriaDTO dto)
        {
            var nombreExistente = await _repository.ExistsByNombreAsync(dto.Nombre);
            if (nombreExistente)
                throw new InvalidOperationException("Ya existe una categoría con el mismo nombre.");   
            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                Activo = true
            };
            await _repository.AddAsync(categoria);
            return await GetById(categoria.Id)
                ?? throw new InvalidOperationException("Error al crear la categoría.");
        }
        public async Task<bool> UpdateAsync(int id, ActualizarCategoriaDTO dto)
        {
            var categoria = await _repository.GetByIdAsync(id);
            if (categoria == null)
            {
                return false;
            }
            var nombreExistente = await _repository.ExistsByNombreExceptIdAsync(dto.Nombre, id);
            if (nombreExistente)
                throw new InvalidOperationException("Ya existe una categoría con el mismo nombre.");
            categoria.Nombre = dto.Nombre;
            categoria.Activo = dto.Activo;
            await _repository.UpdateAsync(categoria);
            return true;
        }   
        public async Task<bool> DeleteAsync(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);
            if (categoria == null) return false;
            await _repository.DeleteAsync(categoria);
            return true;
        }   
    }
}
