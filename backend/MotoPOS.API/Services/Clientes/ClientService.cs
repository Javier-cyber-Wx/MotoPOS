using MotoPOS.API.DTOs.Clientes;
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Interfaces.Clientes; 
namespace MotoPOS.API.Services.Clientes
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            var clients = await _repository.GetAllAsync();
            return clients.Select(c => new ClienteDto
            {
                Id = c.Id,
                Nit = c.Nit,
                Nombre = c.Nombre,
                Direccion = c.Direccion,
                Telefono = c.Telefono,
                Correo = c.Correo,
                Activo = c.Activo
            });
        }
        public async Task<ClienteDto?> GetByIdAsync(int id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null)
            {
                return null;
            }
            return new ClienteDto
            {
                Id = cliente.Id,
                Nit = cliente.Nit,
                Nombre = cliente.Nombre,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono,
                Correo = cliente.Correo,
                Activo = cliente.Activo
            };
        }
        public async Task<ClienteDto> CreateAsync(CrearClienteDto dto)
        {
            var nitExistente = await _repository.ExistsByNitAsync(dto.Nit);

            if (nitExistente)
                throw new InvalidOperationException(
                    "Ya existe un cliente con el mismo NIT.");

            var cliente = new Cliente
            {
                Nit = dto.Nit,
                Nombre = dto.Nombre,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Activo = true
            };

            cliente = await _repository.CreateAsync(cliente);

            return await GetByIdAsync(cliente.Id)
                   ?? throw new Exception(
                       "No se pudo recuperar el cliente creado.");
        }
        public async Task<bool> UpdateAsync(
        int id,
        ActualizarClienteDto dto)
        {
            var cliente = await _repository.GetByIdAsync(id);

            if (cliente == null)
                return false;

            var nitExistente = await _repository
                .ExistsByNitExceptIdAsync(dto.Nit, id);

            if (nitExistente)
                throw new InvalidOperationException(
                    "Ya existe otro cliente con ese NIT.");

            cliente.Nit = dto.Nit;
            cliente.Nombre = dto.Nombre;
            cliente.Direccion = dto.Direccion;
            cliente.Telefono = dto.Telefono;
            cliente.Correo = dto.Correo;
            cliente.Activo = dto.Activo;

            await _repository.UpdateAsync(cliente);

            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _repository.GetByIdAsync(id);

            if (cliente == null)
                return false;

            await _repository.DeleteAsync(cliente);

            return true;
        }
    }
}
 