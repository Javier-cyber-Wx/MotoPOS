using MotoPOS.API.DTOs.Proveedores; 
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Interfaces.Proveedores;   

namespace MotoPOS.API.Services.Proveedores
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;
        public ProveedorService(IProveedorRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ProveedorDto>> GetAllAsync()
        {
            var proveedores = await _repository.GetAllAsync();
            return proveedores.Select(p => new ProveedorDto
            {
                Id = p.Id,
                Nit = p.Nit,
                NombreEmpresa = p.NombreEmpresa,
                NombreContacto = p.NombreContacto,  
                Direccion = p.Direccion,
                Telefono = p.Telefono,
                Correo = p.Correo,
                Activo = p.Activo
            });
        }
        public async Task<ProveedorDto?> GetByIdAsync(int id)
        {
            var proveedor = await _repository.GetByIdAsync(id);
            if (proveedor == null)
            {
                return null;
            }
            return new ProveedorDto
            {
                Id = proveedor.Id,
                Nit = proveedor.Nit,
                NombreEmpresa = proveedor.NombreEmpresa,
                NombreContacto = proveedor.NombreContacto,
                Direccion = proveedor.Direccion,
                Telefono = proveedor.Telefono,
                Correo = proveedor.Correo,
                Activo = proveedor.Activo
            };
        }
        public async Task<ProveedorDto> CreateAsync(CrearProveedorDto dto)
        {
            var nitExistente = await _repository.ExistsByNitAsync(dto.Nit);
            if (nitExistente)
                throw new InvalidOperationException(
                    "Ya existe un proveedor con el mismo NIT.");
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
            return new ProveedorDto
            {
                Id = createdProveedor.Id,
                Nit = createdProveedor.Nit,
                NombreEmpresa = createdProveedor.NombreEmpresa,
                NombreContacto = createdProveedor.NombreContacto,
                Direccion = createdProveedor.Direccion,
                Telefono = createdProveedor.Telefono,
                Correo = createdProveedor.Correo,
                Activo = createdProveedor.Activo
            };
        }
        public async Task<bool> UpdateAsync(int id, ActualizarProveedorDto dto)
        {
            var proveedor = await _repository.GetByIdAsync(id);
            if (proveedor == null)
            {
                return false;
            }
            var nitExistente = await _repository.ExistsByNitExceptIdAsync(dto.Nit, id);
            if (nitExistente)
                throw new InvalidOperationException(
                    "Ya existe un proveedor con el mismo NIT.");
            proveedor.Nit = dto.Nit;
            proveedor.NombreEmpresa = dto.NombreEmpresa;
            proveedor.NombreContacto = dto.NombreContacto;
            proveedor.Direccion = dto.Direccion;
            proveedor.Telefono = dto.Telefono;
            proveedor.Correo = dto.Correo;
            proveedor.Activo = dto.Activo;
            await _repository.UpdateAsync(proveedor);
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var proveedor = await _repository.GetByIdAsync(id);
            if (proveedor == null)
            {
                return false;
            }
            await _repository.DeleteAsync(proveedor);
            return true;
        }

    }
}   