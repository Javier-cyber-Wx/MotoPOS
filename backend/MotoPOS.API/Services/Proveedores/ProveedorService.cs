using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Proveedores; 
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Extensions;
using MotoPOS.API.Interfaces.Proveedores;
using MotoPOS.API.Services;
using MotoPOS.API.Exceptions;

namespace MotoPOS.API.Services.Proveedores
{
    public class ProveedorService : BaseService, IProveedorService
    {
        private readonly IProveedorRepository _repository;
        public ProveedorService(IProveedorRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ProveedorDto>> GetAllAsync()
        {
            var proveedores = await _repository.GetAllAsync();
            return proveedores.ToDto();
        }
        public async Task<ProveedorDto?> GetByIdAsync(int id)
        {
            var proveedor = await _repository.GetByIdAsync(id);
            if (proveedor == null)
            {
                return null;
            }
            return proveedor.ToDto();
        }
        public async Task<ProveedorDto> CreateAsync(CrearProveedorDto dto)
        {
            var nitExistente = await _repository.ExistsByNitAsync(dto.Nit);
            ValidateDuplicate(nitExistente, ErrorMessages.Proveedores.NitDuplicado);
            var proveedor = new Proveedor
            {
                Nit = dto.Nit,
                NombreEmpresa = dto.NombreEmpresa,
                NombreContacto = dto.NombreContacto,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Activo = true
            };
            var createdProveedor = await _repository.CreateAsync(proveedor);
            return createdProveedor.ToDto();
        }
        public async Task UpdateAsync(int id, ActualizarProveedorDto dto)
        {
            var proveedor = await _repository.GetByIdAsync(id);
            ValidateEntityExists(proveedor, ErrorMessages.Proveedores.ProveedorNoEncontrado);
            var nitExistente = await _repository.ExistsByNitExceptIdAsync(dto.Nit, id);
            ValidateDuplicate(nitExistente, ErrorMessages.Proveedores.NitDuplicado);
            proveedor!.Nit = dto.Nit;
            proveedor!.NombreEmpresa = dto.NombreEmpresa;
            proveedor!.NombreContacto = dto.NombreContacto;
            proveedor!.Direccion = dto.Direccion;
            proveedor!.Telefono = dto.Telefono;
            proveedor!.Correo = dto.Correo;
            proveedor!.Activo = dto.Activo;
            await _repository.UpdateAsync(proveedor);
        }
        public async Task DeleteAsync(int id)
        {
            var proveedor = await _repository.GetByIdAsync(id);
            ValidateEntityExists(proveedor, ErrorMessages.Proveedores.ProveedorNoEncontrado);
            await _repository.DeleteAsync(proveedor!);
        }
    }
}   