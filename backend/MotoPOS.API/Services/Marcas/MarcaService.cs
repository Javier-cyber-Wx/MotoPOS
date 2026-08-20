using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Marcas;  
using MotoPOS.API.Entities.Catalogos;  
using MotoPOS.API.Extensions;
using MotoPOS.API.Interfaces.Marcas;
using MotoPOS.API.Exceptions;

namespace MotoPOS.API.Services.Marcas
{
    public class MarcaService : BaseService, IMarcaService
    {
        private readonly IMarcaRepository _repository;
        public MarcaService(IMarcaRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<MarcaDTO>> GetAllAsync()
        {
            var marcas = await _repository.GetAllAsync();
            return marcas.ToDto();
        }
        public async Task<MarcaDTO?> GetByIdAsync(int id)
        {
            var marca = await _repository.GetByIdAsync(id);
            if (marca == null)
            {
                return null;
            }
            return marca.ToDto();
        }
        public async Task<MarcaDTO> CreateAsync(CrearMarcaDto dto)
        {
            var nombreExistente = await _repository.ExistsByNombreAsync(dto.Nombre);
            ValidateDuplicate(nombreExistente, ErrorMessages.Marcas.NombreDuplicado);
            var marca = new Marca
            {
                Nombre = dto.Nombre,
                Activo = true
            };
            marca = await _repository.CreateAsync(marca);
            return await GetByIdAsync(marca.Id)
                ?? throw new InvalidOperationException(ErrorMessages.Marcas.MarcaNoRecuperada);
        }
        public async Task UpdateAsync(int id, ActualizarMarcaDTO dto)
        {
            var marca = await _repository.GetByIdAsync(id);
            ValidateEntityExists(marca, ErrorMessages.Marcas.MarcaNoEncontrada);

            var nombreExistente = await _repository
                .ExistsByNombreExceptIdAsync(dto.Nombre, id);

            ValidateDuplicate(nombreExistente, ErrorMessages.Marcas.NombreYaExiste);

            marca!.Nombre = dto.Nombre;
            marca!.Activo = dto.Activo;

            await _repository.UpdateAsync(marca);
        }
        public async Task DeleteAsync(int id)
        {
            var marca = await _repository.GetByIdAsync(id);
            ValidateEntityExists(marca, ErrorMessages.Marcas.MarcaNoEncontrada);
            await _repository.DeleteAsync(marca!);
        }
    }
}