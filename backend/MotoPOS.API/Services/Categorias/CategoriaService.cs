using MotoPOS.API.Constants;
using MotoPOS.API.Interfaces.Categorias;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.DTOs.Categorias;
using MotoPOS.API.Extensions;
using MotoPOS.API.Exceptions;


namespace MotoPOS.API.Services.Categorias
{
    public class CategoriaService : BaseService, ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<CategoriaDTO>> GetAllAsync()
        {
            var categorias = await _repository.GetAllAsync();
            return categorias.ToDto();
        }
        public async Task<CategoriaDTO?> GetById(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);
            if (categoria == null) return null;
            return categoria.ToDto();
        }
        public async Task<CategoriaDTO> CreateAsync(CrearCategoriaDTO dto)
        {
            var nombreExistente = await _repository.ExistsByNombreAsync(dto.Nombre);
            ValidateDuplicate(nombreExistente, ErrorMessages.Categorias.NombreDuplicado);
            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                Activo = true
            };
            await _repository.AddAsync(categoria);
            return await GetById(categoria.Id)
                ?? throw new InvalidOperationException(ErrorMessages.Categorias.ErrorCreacion);
        }
        public async Task UpdateAsync(int id, ActualizarCategoriaDTO dto)
        {
            var categoria = await _repository.GetByIdAsync(id);
            ValidateEntityExists(categoria, ErrorMessages.Categorias.CategoriaNoEncontrada);

            var nombreExistente = await _repository.ExistsByNombreExceptIdAsync(dto.Nombre, id);
            ValidateDuplicate(nombreExistente, ErrorMessages.Categorias.NombreDuplicado);

            categoria!.Nombre = dto.Nombre;
            categoria!.Activo = dto.Activo;
            await _repository.UpdateAsync(categoria);
        }
        public async Task DeleteAsync(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);
            ValidateEntityExists(categoria, ErrorMessages.Categorias.CategoriaNoEncontrada);
            await _repository.DeleteAsync(categoria!);
        }
    }
}
