using MotoPOS.API.Constants;
using MotoPOS.API.DTOs.Clientes;
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Extensions;
using MotoPOS.API.Interfaces.Clientes;
using MotoPOS.API.Services;
using MotoPOS.API.Exceptions;

namespace MotoPOS.API.Services.Clientes;

public class ClienteService : BaseService, IClienteService
{
    private readonly IClientRepository _repository;

    public ClienteService(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ClienteDto>> GetAllAsync()
    {
        var clients = await _repository.GetAllAsync();
        return clients.ToDto();
    }

    public async Task<ClienteDto?> GetByIdAsync(int id)
    {
        var cliente = await _repository.GetByIdAsync(id);
        if (cliente == null)
        {
            return null;
        }
        return cliente.ToDto();
    }

    public async Task<ClienteDto> CreateAsync(CrearClienteDto dto)
    {
        var nitExistente = await _repository.ExistsByNitAsync(dto.Nit);
        ValidateDuplicate(nitExistente, ErrorMessages.Clientes.NitDuplicado);

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
               ?? throw new InvalidOperationException(ErrorMessages.Clientes.ClienteNoRecuperado);
    }

    public async Task UpdateAsync(int id, ActualizarClienteDto dto)
    {
        var cliente = await _repository.GetByIdAsync(id);
        ValidateEntityExists(cliente, ErrorMessages.Clientes.ClienteNoEncontrado);

        var nitExistente = await _repository
            .ExistsByNitExceptIdAsync(dto.Nit, id);

        ValidateDuplicate(nitExistente, ErrorMessages.Clientes.NitYaExiste);

        cliente!.Nit = dto.Nit;
        cliente!.Nombre = dto.Nombre;
        cliente!.Direccion = dto.Direccion;
        cliente!.Telefono = dto.Telefono;
        cliente!.Correo = dto.Correo;
        cliente!.Activo = dto.Activo;

        await _repository.UpdateAsync(cliente);
    }

    public async Task DeleteAsync(int id)
    {
        var cliente = await _repository.GetByIdAsync(id);
        ValidateEntityExists(cliente, ErrorMessages.Clientes.ClienteNoEncontrado);
        await _repository.DeleteAsync(cliente!);
    }
}