using MotoPOS.API.DTOs.Marcas;  

namespace MotoPOS.API.Interfaces.Marcas
{
    public interface IMarcaService
    {
        Task<IEnumerable<MarcaDTO>> GetAllAsync();
        Task<MarcaDTO?> GetByIdAsync(int id);
        Task<MarcaDTO> CreateAsync(CrearMarcaDto dto);
        Task<bool> UpdateAsync(int id, ActualizarMarcaDto dto);
        Task<bool> DeleteAsync(int id); 
    }
}
