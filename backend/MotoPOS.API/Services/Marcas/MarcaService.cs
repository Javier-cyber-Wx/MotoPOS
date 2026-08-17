using MotoPOS.API.DTOs.Marcas;  
using MotoPOS.API.Entities.Catalogos;  
using MotoPOS.API.Interfaces.Marcas;

namespace MotoPOS.API.Services.Marcas
{
    public class MarcaService : IMarcaService
    {
        private readonly IMarcaRepository _repository;
        public MarcaService(IMarcaRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<MarcaDTO>> GetAllAsync()
        {
            var marcas = await _repository.GetAllAsync();
            return marcas.Select(m => new MarcaDTO
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Activo = m.Activo
            });
        }
        public async Task<MarcaDTO?> GetByIdAsync(int id)
        {
            var marca = await _repository.GetByIdAsync(id);
            if (marca == null)
            {
                return null;
            }
            return new MarcaDTO
            {
                Id = marca.Id,
                Nombre = marca.Nombre,
                Activo = marca.Activo
            };
        }
        public async Task<MarcaDTO> CreateAsync(CrearMarcaDto dto)
        {
            var nombreExistente = await _repository.ExistsByNombreAsync(dto.Nombre);
            if (nombreExistente)
                throw new InvalidOperationException(
                    "Ya existe una marca con el mismo nombre.");
            var marca = new Marca
            {
                Nombre = dto.Nombre,
                Activo = true
            };
            marca = await _repository.CreateAsync(marca);
            return await GetByIdAsync(marca.Id)
                ?? throw new InvalidOperationException(
                    "No se pudo recuperar la marca creada.");
        }
        public async Task<bool> UpdateAsync(int id, ActualizarMarcaDto dto)
        {
            var marca = await _repository.GetByIdAsync(id);

            if (marca == null)
                return false;

            var nombreExistente = await _repository
                .ExistsByNombreExceptIdAsync(dto.Nombre, id);

            if (nombreExistente)
                throw new InvalidOperationException(
                    "Ya existe otra marca con el mismo nombre.");

            marca.Nombre = dto.Nombre;
            marca.Activo = dto.Activo;

            await _repository.UpdateAsync(marca);

            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var marca = await _repository.GetByIdAsync(id);
            if (marca == null)
            {
                return false;
            }
            await _repository.DeleteAsync(marca);
            return true;
        }
    }
}